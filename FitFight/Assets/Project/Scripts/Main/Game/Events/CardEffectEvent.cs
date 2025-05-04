using _Project.Scripts.Main.Utilities.Scripts.EventBus;

namespace _Project.Scripts.Main.Game.Events
{
    public struct CardEffectEvent : IEvent
    {
        public bool IsEnable;
    }
}