using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static _Project.Scripts.Main.Game.Events.CommandTriggerEvents;

namespace _Project.Scripts.Main.Game.States
{
    public class GamePlayerTurnState : GameStateController.GameState
    {
        public GamePlayerTurnState(GameStateController ctrl) : base(ctrl)
        {
        }

        public override void OnEnter(object args)
        {
            Debug.Log("GamePlayerTurnState: OnEnter");
            GetGameController.StartNewCardSet(() =>
            {
                GetGameController.TryResetPlayerBlock();
                GetGameController.ShowEndTurnNoSelectedButton();
                EventBus<StartPlayerTurnEvent>.Raise(new StartPlayerTurnEvent());
            });
        }
        
        public override void OnExec()
        {
            
        }

        public override void OnExit()
        {
            GetGameController.DecreasePlayerDebuff();
            EventBus<EndPlayerTurnEvent>.Raise(new EndPlayerTurnEvent());
        }

        public override void OnPause()
        {
            
        }

        public override void OnResume()
        {
            
        }
    }
}