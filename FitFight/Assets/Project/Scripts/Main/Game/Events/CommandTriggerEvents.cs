using _Project.Scripts.Main.Game.PlayerTurn.Systems;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;

namespace _Project.Scripts.Main.Game.Events
{
    public struct CommandTriggerEvents
    {
        #region Game State Events
        public struct GameStartEvent : IEvent { }
        public struct GameWonEvent : IEvent { }
        #endregion

        #region Player/Enemy Events
        public struct PlayerDamagedEvent : IEvent
        {
            public int Damage;
        }

        public struct PlayerReduceHealthEvent : IEvent
        {
            public int Health;
        }
        
        public struct PlayerBlockGainedEvent : IEvent
        {
            public int Amount;
        }
        
        #endregion

        #region Turn Events
        public struct StartPlayerTurnEvent : IEvent { }
        public struct EndPlayerTurnEvent : IEvent { }
        #endregion

        #region Card Events
        public struct ShuffledDeckEvent : IEvent { }

        public struct CardDrawnEvent : IEvent
        {
            public CardInstance Card;
        }

        public struct AttackCardPlayedEvent : IEvent { }
        #endregion
    }
}