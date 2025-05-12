using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Managers;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Main.Game.Commands.Acquisition
{
    public class RestoreAllHealthCommand : AcquisitionBaseCommand
    {
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            GameManager gm = GameManager.Instance;
            gm.SetCurrentPlayerHealth(gm.CurrentMaxHealth);
        }
    }
}