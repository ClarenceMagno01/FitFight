using _Project.Scripts.Main.Game.Commands.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands.PlayerTarget
{
    public class GainBlockCommand : BaseCommand
    {
        [SerializeField] private int _bonusBlock;
        
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            await PlayerEntity.AddBlock(_bonusBlock);
        }
    }
}