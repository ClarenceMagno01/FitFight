using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using UnityEngine;
using static _Project.Scripts.Main.Game.Events.CommandTriggerEvents;

namespace _Project.Scripts.Main.Game.States
{
    public class GameWonState : GameStateController.GameState
    {
        public GameWonState(GameStateController ctrl) : base(ctrl)
        {
        }

        public override void OnEnter(object args)
        {
            Debug.Log("GameWonState: OnEnter");
            EventBus<CardSystemEvents.StopInteractionEvent>.Raise(new CardSystemEvents.StopInteractionEvent{IsStop = true});
            GetGameController.PlayWinFeedback();
            GetGameController.SaveCurrentHealth();
            GetGameController.ShowWinPanel();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameManager.Instance.CurrentNodeState.Status = NodeStatus.Unlocked;
            EventBus<GameWonEvent>.Raise(new GameWonEvent());
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