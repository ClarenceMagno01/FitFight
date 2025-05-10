using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using UnityEngine;

namespace _Project.Scripts.Main.Game.States
{
    public class GameLoseState : GameStateController.GameState
    {
        public GameLoseState(GameStateController ctrl) : base(ctrl)
        {
        }

        public override void OnEnter(object args)
        {
            Debug.Log("GameLoseState: OnEnter");
            EventBus<CardSystemEvents.StopInteractionEvent>.Raise(new CardSystemEvents.StopInteractionEvent{IsStop = true});
            GetGameController.PlayLoseFeedback();
            GetGameController.ShowLosePanel();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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