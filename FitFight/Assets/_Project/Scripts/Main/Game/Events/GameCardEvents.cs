using _Project.Scripts.Main.Utilities.Scripts.EventBus;

namespace _Project.Scripts.Main.Game.Events
{
    public struct GameCardEvents
    {
        public struct EnableColliderEvent : IEvent
        {
            public bool IsEnable;
        }
    }
}