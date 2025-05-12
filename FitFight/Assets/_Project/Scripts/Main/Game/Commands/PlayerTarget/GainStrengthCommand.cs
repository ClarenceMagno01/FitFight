using _Project.Scripts.Main.Game.Commands.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands.PlayerTarget
{
    public class GainStrengthCommand : BaseCommand
    {
        [SerializeField] private int _bonusStrength;
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            await PlayerEntity.AddStrength(_bonusStrength);
        }
    }
}