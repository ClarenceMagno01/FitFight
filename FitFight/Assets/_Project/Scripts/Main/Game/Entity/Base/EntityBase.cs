using _Project.Scripts.Main.Game.Entity.Component;
using Cysharp.Threading.Tasks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Entity
{
    public abstract class EntityBase : MonoBehaviour, IEntity
    {
        [Header("Stats")] 
        [SerializeField] protected string entityName;
        [SerializeField] protected int health = 100;
        protected int maxHealth;
        [SerializeField] protected int strength;
        [SerializeField] protected int block;
      
        [Header("Health")]
        [SerializeField] private MMHealthBar _healthDrawer;
        [SerializeField] private TMP_Text _txtHealth;
        
        [HideInInspector] public DebuffComponent debuff;
        
        
        protected virtual void Awake()
        {
            debuff = gameObject.AddComponent<DebuffComponent>();
        }
        public virtual async UniTask ApplyDebuff(DebuffType debuffType,int amount)
        {
            debuff.Apply(debuffType,amount);
            await UniTask.Yield();
        }

        public virtual async UniTask AddHealth(int amount)
        {
            health = Mathf.Clamp(health + amount, 0, maxHealth);
            UpdateHealthUI();
            await UniTask.Yield();
        }
        
        protected void DecreaseHealth(int val)
        {
            health -= val;
            UpdateHealthUI();
        }

        protected virtual void UpdateHealthUI()
        {
            health = Mathf.Clamp(health,0, maxHealth);
            _healthDrawer.UpdateBar(health,0, maxHealth, true); 
            _txtHealth.text = $"{health}/{maxHealth}";
        }
        
        public abstract UniTask TakeDamage(int damage);

        public virtual async UniTask AddBlock(int val)
        {
            await UniTask.Yield();
            block += val;
        }

        public virtual async UniTask AddStrength(int val)
        {
            await UniTask.Yield();
            strength += val;
        }

        public virtual async UniTask DecreaseStrength(int val)
        {
            await UniTask.Yield();
            strength -= val;
        }

        public void ResetBlock()
        {
            block = 0;
        }
        
        public bool IsDead => health <= 0;
    }
}