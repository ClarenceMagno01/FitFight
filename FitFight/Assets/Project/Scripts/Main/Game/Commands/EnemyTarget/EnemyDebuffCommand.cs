using System.Collections.Generic;
using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Game.Entity.Component;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands.EnemyTarget
{
    public class EnemyDebuffCommand : EnemyTargetBaseCommand
    {
        [SerializeField] private DebuffType _debuffType;
        [SerializeField] private int _amount = 1;

        List<UniTask> _tasks = new();
        
        public override async UniTask  ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            _tasks.Clear();
            foreach (var enemy in GetTargets())
            {
                if (GetTargetType == TargetType.All)
                    _tasks.Add(enemy.ApplyDebuff(_debuffType,_amount));
                else
                    await enemy.ApplyDebuff(_debuffType,_amount);
            }
            await UniTask.WhenAll(_tasks);
        }
    }
}