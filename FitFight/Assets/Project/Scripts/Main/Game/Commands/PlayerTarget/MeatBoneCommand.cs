using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Main.Game.Commands.PlayerTarget
{
    public class MeatBoneCommand : BaseCommand
    {
        [SerializeField] private int _healAmount;
        
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            int halfMax = PlayerEntity.MaxHealth >> 1;
            bool isHealthBelow50 = PlayerEntity.Health < halfMax;
            if (isHealthBelow50)
            {
                GameManager.Instance.AddHealth(_healAmount);
                await PlayerEntity.AddHealth(_healAmount);
            }
        }
    }
}