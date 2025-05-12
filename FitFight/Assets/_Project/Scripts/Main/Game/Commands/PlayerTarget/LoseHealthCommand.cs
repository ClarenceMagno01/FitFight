using _Project.Scripts.Main.Game.Commands.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Main.Game.Commands.PlayerTarget
{
    public class LoseHealthCommand : BaseCommand
    {
        [SerializeField] private int _amount;
        
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            await PlayerEntity.TryDecreaseHealth(_amount);
        }
    }
}