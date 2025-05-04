using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands.PlayerTarget
{
    public class HealCommand : BaseCommand
    {
        [SerializeField] private int _amount;
        [SerializeField] private bool _saveToProgress;
        
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            if (_saveToProgress)
                GameManager.Instance.AddHealth(_amount);
            await PlayerEntity.AddHealth(_amount);
        }
    }
}