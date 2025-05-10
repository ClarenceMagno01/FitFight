using _Project.Scripts.Main.Game.Commands.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands.PlayerTarget
{
    public class RedSkullCommand : BaseCommand
    {
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            PlayerEntity.buff.ApplyRedSkull();
        }
    }
}