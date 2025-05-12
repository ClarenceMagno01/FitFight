using _Project.Scripts.Main.Game.Commands.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands.PlayerTarget
{
    public class LoseStrengthCommand : BaseCommand
    {
        [SerializeField] private int _penalty;
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            await PlayerEntity.DecreaseStrength(_penalty);
        }
    }
}