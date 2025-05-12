using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands.Acquisition
{
    public class IncreaseMaxHPCommand : AcquisitionBaseCommand
    {
        [SerializeField] private int _amount;
        
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            GameManager.Instance.AddMaxHealth(_amount);
        }
    }
}