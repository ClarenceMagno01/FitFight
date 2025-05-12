
using _Project.Scripts.Main.Utilities.Scripts.EventBus;

namespace _Project.Scripts.Main.Game.Events
{
    public struct GameEvents
    {
        public struct CardPhaseFinishedEvent : IEvent { } 
        public struct PlayerLoseEvent : IEvent { } 
        public struct PauseGameEvent : IEvent { } 
        public struct ResumeGameEvent : IEvent { } 
    }
}