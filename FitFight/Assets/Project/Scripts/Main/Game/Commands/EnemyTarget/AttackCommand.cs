using System.Collections.Generic;
using _Project.Scripts.Main.Game.Commands.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands.EnemyTarget
{
    public class AttackCommand : EnemyTargetBaseCommand
    {
        [SerializeField] private int _damage;
        [SerializeField] private int _repeatCount = 1;
        [SerializeField] private int _strengthMultiplier = 1;
        
        List<UniTask> _tasks = new();
        
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            for (int i = 0; i < _repeatCount; ++i)
            {
                _tasks.Clear();
                foreach (var enemy in GetTargets())
                {
                    int finalDamage = _damage + (PlayerEntity.Strength * _strengthMultiplier);
                    if (GetTargetType == TargetType.All)
                        _tasks.Add(enemy.TakeDamage(finalDamage));
                    else
                        await enemy.TakeDamage(finalDamage);
                }
                await UniTask.WhenAll(_tasks);
            }
        }
    }
}