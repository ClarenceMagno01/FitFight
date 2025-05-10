using _Project.Scripts.Main.Game.Commands.Base;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Main.Game.Commands.PlayerTarget
{
    public class DoubleStrengthCommand: BaseCommand
    { 
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            await PlayerEntity.AddStrength(PlayerEntity.Strength); 
        }
    }
}