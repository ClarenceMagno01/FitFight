using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.States
{
    public class GameEnemyTurnState : GameStateController.GameState
    {
        public GameEnemyTurnState(GameStateController ctrl) : base(ctrl)
        {
            
        }

        public override void OnEnter(object args)
        {
            Debug.Log("GameEnemyTurnState: OnEnter");
            EnemyTurn().Forget();
        }
        
        private async UniTaskVoid EnemyTurn()
        {
            await GetGameController.DoEnemyAttacks();
            if(!GetGameController.IsPlayerDead)
                Controller.ChangeState<GamePlayerTurnState>();
        }

        public override void OnExec()
        {
            
        }

        public override void OnExit()
        {
            GetGameController.ResetEnemiesBlock();
            GetGameController.DecreaseEnemiesDebuff();
        }

        public override void OnPause()
        {
            
        }

        public override void OnResume()
        {
            
        }
    }

}