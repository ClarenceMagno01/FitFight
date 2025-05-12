using System;
using _Project.Scripts.Main.Game.Entity.Component;
using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Entity
{
    [RequireComponent(typeof(EnemyFeedbackBaseComponent))]
    public class Enemy :  EntityBase
    {
        [Header("Attack Damage")]
        [SerializeField] private int _attackDamage;
        [Space]
        [Header("Spawn Index")]
        public int spawnIndex;
        
        private EnemyFeedbackBaseComponent _feedback;
        
        protected override void Awake()
        {
            base.Awake();
            _feedback = GetComponent<EnemyFeedbackBaseComponent>();
        }

        private void Start()
        {
            maxHealth = health;
            UpdateHealthUI();
        }

        public override async UniTask AddStrength(int val)
        {
            await base.AddStrength(val);
            await _feedback.PlayStatBonusFeedback($"+{val} Strength");
        }

        public override async UniTask AddBlock(int val)
        {
            await base.AddBlock(val);
            await _feedback.PlayStatBonusFeedback($"+{val} Block");
        }
        
        public override async UniTask ApplyDebuff(DebuffType debuffType, int amount)
        {
            await base.ApplyDebuff(debuffType, amount);
            await _feedback.PlayDebuffFeedback($"{amount} {debuffType.ToString()}");
        }
        
        public async UniTask AttackWithFeedback()
        {
            await _feedback.PlayAttackFeedback(); //Note: Feedback event will call the Attack Method
        }
        
        public void Attack()
        {
            int finalDamage = _attackDamage + strength;
            if(debuff.IsWeak)
                finalDamage = Mathf.FloorToInt(_attackDamage * 0.75f);
            Debug.Log($"Enemy attack with {finalDamage} damage");
            Player player = EntityManager.Instance.PlayerEntity;
            player?.TakeDamage(finalDamage);
            _feedback.ResumeAttackFeedbacks();
        }

        public override async UniTask TakeDamage(int damage)
        {
            int finalDamage = damage;
            if (debuff.IsVulnerable)
            {
                float multiplier = !debuff.isPaperPhrog ? 1.5f : 1.75f;
                finalDamage = Mathf.FloorToInt(damage * multiplier);
            }

            await TryDecreaseHealth(finalDamage);
            if (IsDead)
            {
                EventBus<EnemyDiedEvent>.Raise(new EnemyDiedEvent
                {
                    Enemy = this
                });
            }
        }
        
        public async UniTask TryDecreaseHealth(int val)
        {
            if (!IsDead)
            {
                DecreaseHealth(val);
                await _feedback.PlayDamageFeedback(val);
            }
        }
    }
}