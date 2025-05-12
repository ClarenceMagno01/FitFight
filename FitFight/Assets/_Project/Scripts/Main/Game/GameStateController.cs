using _Project.Scripts.Main.Game.States;
using _Project.Scripts.Main.Utilities.Scripts.StateMachine;
using UnityEngine;

namespace _Project.Scripts.Main.Game
{
    public class GameStateController : StateController
    {
        public GameController gameController;
        
        public abstract class GameState : IState
        {
            public GameState(GameStateController ctrl)
            {
                Controller = ctrl;
            }

            protected GameStateController Controller { get; }

            protected GameController GetGameController => Controller.gameController;
            
            public abstract void OnEnter(object args);
            public abstract void OnExec();
            public abstract void OnExit();
            public abstract void OnPause();
            public abstract void OnResume();
            
        }

        internal override void Start()
        {
            base.Start();
            
            RegisterStateHandler<GameInitState>();
            RegisterStateHandler<GamePlayerTurnState>();
            RegisterStateHandler<GameEnemyTurnState>();
            RegisterStateHandler<GameWonState>();
            RegisterStateHandler<GameLoseState>();
            
            ChangeState<GameInitState>();
        }
    }
}