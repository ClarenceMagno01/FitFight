using System.Collections.Generic;
using _Project.Scripts.Main.Game.Commands.Base;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Main.Game.Commands.EnemyTarget
{
    public class PaperPhrogCommand : EnemyTargetBaseCommand
    {
        public override async UniTask  ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            foreach (var enemy in GetTargets())
                enemy.debuff.ApplyPaperPhrog();
        }
    }
}