using _Project.Scripts.Main.Game.Entity;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;

namespace _Project.Scripts.Main.Game.Events
{
    public struct EnemyDiedEvent : IEvent
    {
        public Enemy Enemy;
    }
    
}