using _Project.Scripts.Main.UI;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;

namespace _Project.Scripts.Main.Game.Events
{
    public struct OverlayUIEvents
    {
        public struct InvalidateRelicBarEvent : IEvent { }

        public struct UpdateHealthEvent : IEvent
        {
            public int CurrentHealth;
            public int MaxHealth;
        } 
        
        public struct UpdateGoldEvent : IEvent { } 
        public struct UpdateNumberOfCardsEvent : IEvent { } 
        
        public struct ShowLosePanelEvent : IEvent
        {
            public bool IsShow;
            public GameSummary Summary;
        }
    }
}