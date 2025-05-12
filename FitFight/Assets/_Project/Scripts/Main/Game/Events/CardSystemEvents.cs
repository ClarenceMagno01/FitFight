using System;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;

namespace _Project.Scripts.Main.Game.Events
{
    public struct CardSystemEvents
    {
        public struct StartNewCardSetEvent : IEvent
        {
            public Action OnStartNewCardSet;
        }
        
        public struct StartExerciseEvent : IEvent
        {
            public Action OnStartExercise;
        }
        
        public struct EndCardPhaseEvent : IEvent { }
        
        public struct CopyCardToDiscardPileEvent : IEvent
        {
            public CardData Data;
        }
        
        public struct StopInteractionEvent : IEvent
        {
            public bool IsStop;
        }
  
    }
}