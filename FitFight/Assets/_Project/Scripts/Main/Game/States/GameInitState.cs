using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static _Project.Scripts.Main.Game.Events.CommandTriggerEvents;

namespace _Project.Scripts.Main.Game.States
{
    public class GameInitState : GameStateController.GameState
    {
        public GameInitState(GameStateController ctrl) : base(ctrl)
        {
        }

        public override void OnEnter(object args)
        {
            Debug.Log("GameInitState: OnEnter");
            GetGameController.SpawnEntities();
            GetGameController.PrepareRelicCommands();
            GetGameController.isCombartStart = true;
            EventBus<GameStartEvent>.Raise(new GameStartEvent());
            Controller.ChangeState<GamePlayerTurnState>();
        }
        
        public override void OnExec()
        {
            
        }

        public override void OnExit()
        {
            
        }

        public override void OnPause()
        {
            
        }

        public override void OnResume()
        {
            
        }
    }
}