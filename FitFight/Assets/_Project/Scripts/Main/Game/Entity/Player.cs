using _Project.Scripts.Main.Game.Entity.Component;
using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static _Project.Scripts.Main.Game.Events.CommandTriggerEvents;
using static _Project.Scripts.Main.Game.Events.OverlayUIEvents;

namespace _Project.Scripts.Main.Game.Entity
{
    [RequireComponent(typeof(PlayerFeedbackComponent))]
    public class Player : EntityBase
    {
        [HideInInspector] public BuffComponent buff;
        private PlayerFeedbackComponent _feedback;
        
        public int Health => health;
        public int MaxHealth => maxHealth;
        public int Strength => IsRedSkullEffect ? strength + 3 : strength;
        public int Block => block;

        private bool IsRedSkullEffect => buff && buff.isRedSkull && health <= maxHealth / 2;
        
        protected override void Awake()
        {
            base.Awake();
            buff = gameObject.AddComponent<BuffComponent>();
            _feedback = GetComponent<PlayerFeedbackComponent>();
        }

        private void Start()
        {
            health = GameManager.Instance.CurrentPlayerHealth;
            maxHealth = GameManager.Instance.CurrentMaxHealth;
            UpdateHealthUI();
            Debug.Log("Player Stats is overwritten from GameManager");
        }

        protected override void UpdateHealthUI()
        {
            base.UpdateHealthUI();
            EventBus<UpdateHealthEvent>.Raise(new UpdateHealthEvent
            {
                CurrentHealth = health,
                MaxHealth = maxHealth,
            });
        }

        public override async UniTask AddHealth(int amount)
        {
            await base.AddHealth(amount);
            await _feedback.PlayHealFeedback(amount);
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
            EventBus<PlayerBlockGainedEvent>.Raise(new PlayerBlockGainedEvent{ Amount = val });;
        }
        
        public override async UniTask ApplyDebuff(DebuffType debuffType, int amount)
        {
            await base.ApplyDebuff(debuffType, amount);
            await _feedback.PlayDebuffFeedback($"{amount} {debuffType.ToString()}");
        }

        public async UniTask ApplyBarricade()
        {
            buff.ApplyBarricade();
            await _feedback.PlayStatBonusFeedback("Barricade");
        }

        public void TryResetBlock()
        {
            if (!buff.isBarricade)
                ResetBlock();
        }

        private int CalculateDamageWithBlock(int damage)
        {
            int finalDamage;
            if (block >= damage)
            {
                block-=damage;
                finalDamage = 0;
            }
            else
            {
                finalDamage = damage - block;
                block = 0;
            }
            return finalDamage;
        }
        
        public override async UniTask TakeDamage(int damage)
        {
            int finalDamage = CalculateDamageWithBlock(damage);
            if (finalDamage > 0)
            {
                finalDamage = buff.TryAbsorbDamageWithFossilizedHelix(finalDamage);
                await TryDecreaseHealth(finalDamage);
                EventBus<PlayerDamagedEvent>.Raise(new PlayerDamagedEvent{ Damage = finalDamage });
                await TryLizardTail();
                if (IsDead)
                {
                    Destroy(gameObject);
                    EventBus<GameEvents.PlayerLoseEvent>.Raise(new GameEvents.PlayerLoseEvent());
                }
            }
            else
            {
                await _feedback.PlayStatBonusFeedback($"BlOCKED");
            }
        }
 
        private async UniTask TryLizardTail()
        {
            if (IsDead && buff.isLizardTail)
            {
                buff.isLizardTail = false;
                int heal = maxHealth >> 1;
                await AddHealth(heal);
            }
        }
        
        public async UniTask TryDecreaseHealth(int val)
        {
            if (!IsDead)
            {
                DecreaseHealth(val);
                await _feedback.PlayDamageFeedback(val);
                EventBus<PlayerReduceHealthEvent>.Raise(new PlayerReduceHealthEvent{Health = health});
            }
        }
        
    }
}