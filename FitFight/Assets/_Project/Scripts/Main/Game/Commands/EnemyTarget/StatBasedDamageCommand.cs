using System;
using System.Collections.Generic;
using _Project.Scripts.Main.Game.Commands.Base;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Main.Game.Commands.EnemyTarget
{
    [Serializable]
    public enum StatType
    {
        Block,
        Strength,
    }
    
    /// <summary>
    /// Command that applies damage based and "equal" on a stat type.
    /// </summary>
    public class StatBasedDamageCommand : EnemyTargetBaseCommand
    {
        private StatType _statType;
        List<UniTask> _tasks = new();
        
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            _tasks.Clear();
            int damage = (_statType == StatType.Block) ? PlayerEntity.Block : PlayerEntity.Strength;
            foreach (var enemy in GetTargets())
            {
                if (GetTargetType == TargetType.All)
                    _tasks.Add(enemy.TakeDamage(damage));
                else
                    await enemy.TakeDamage(damage);
            }
            await UniTask.WhenAll(_tasks);
        }
    }
}