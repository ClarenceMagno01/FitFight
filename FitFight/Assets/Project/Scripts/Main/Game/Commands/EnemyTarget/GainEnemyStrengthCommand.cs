using System.Collections.Generic;
using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Game.Entity.Component;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands.EnemyTarget
{
    public class GainEnemyStrengthCommand : EnemyTargetBaseCommand
    {
        [SerializeField] private int _amount = 1;

        List<UniTask> _tasks = new();
        
        public override async UniTask  ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            _tasks.Clear();
            foreach (var enemy in GetTargets())
            {
                if (GetTargetType == TargetType.All)
                    _tasks.Add(enemy.AddStrength(_amount));
                else
                    await enemy.AddStrength(_amount);
            }
            await UniTask.WhenAll(_tasks);
        }
    }
}