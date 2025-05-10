using _Project.Scripts.Main.Game.Commands.Base;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Main.Game.Commands.PlayerTarget
{
    public class LizardTailCommand : BaseCommand
    {
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            PlayerEntity.buff.ApplyLizardTail();
        }
    }
}