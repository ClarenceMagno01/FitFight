using _Project.Scripts.Main.Game.Commands.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Main.Game.Commands.PlayerTarget
{
    public class OrichalcumCommand : BaseCommand
    {
        [SerializeField] private int _blockAmount;
        
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            if (PlayerEntity.Block <= 0)
                await PlayerEntity.AddBlock(_blockAmount);
        }
    }
}