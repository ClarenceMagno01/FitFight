using System.Collections.Generic;
using _Project.Scripts.Main.Game.Commands;
using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Game.Entity;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using _Project.Scripts.Main.Utilities.Scripts.Singleton;
using UnityEngine;
using static _Project.Scripts.Main.Game.Events.CommandTriggerEvents;

namespace _Project.Scripts.Main.Managers
{
    public class CommandTriggerManager: SingletonMonoBehaviour<CommandTriggerManager>
    {
        private readonly List<ICommand> _oneShotCommands = new();
        private readonly List<ICommand> _persistentCommands = new();
        
        private readonly List<ICommand> _toRemove = new();

        public void AddOneShotCommand(ICommand command) => _oneShotCommands.Add(command);
        public void AddPersistentCommand(ICommand command) => _persistentCommands.Add(command);

        private void OnEnable()
        {
            EventBus<StartPlayerTurnEvent>.Register(OnStartPlayerTurnEvent);
            EventBus<EndPlayerTurnEvent>.Register(OnEndPlayerTurnEvent);
            EventBus<GameStartEvent>.Register(OnGameStartEvent);
            EventBus<GameWonEvent>.Register(OnGameWonEvent);
            EventBus<PlayerDamagedEvent>.Register(OnPlayerDamagedEvent);
            EventBus<PlayerReduceHealthEvent>.Register(OnPlayerReduceHealthEvent);
            EventBus<PlayerBlockGainedEvent>.Register(OnPlayerBlockGainedEvent);
            EventBus<ShuffledDeckEvent>.Register(OnShuffledDeckEvent);
            EventBus<CardDrawnEvent>.Register(OnCardDrawnEvent);
            EventBus<AttackCardPlayedEvent>.Register(OnAttackCardPlayedEvent);
        }

        private void OnDisable()
        {
            EventBus<StartPlayerTurnEvent>.Deregister(OnStartPlayerTurnEvent);
            EventBus<EndPlayerTurnEvent>.Deregister(OnEndPlayerTurnEvent);
            EventBus<GameStartEvent>.Deregister(OnGameStartEvent);
            EventBus<GameWonEvent>.Deregister(OnGameWonEvent);
            EventBus<PlayerDamagedEvent>.Deregister(OnPlayerDamagedEvent);
            EventBus<PlayerReduceHealthEvent>.Deregister(OnPlayerReduceHealthEvent);
            EventBus<PlayerBlockGainedEvent>.Deregister(OnPlayerBlockGainedEvent);
            EventBus<ShuffledDeckEvent>.Deregister(OnShuffledDeckEvent);
            EventBus<CardDrawnEvent>.Deregister(OnCardDrawnEvent);
            EventBus<AttackCardPlayedEvent>.Deregister(OnAttackCardPlayedEvent);
        }
        
        // Turn Events
        private void OnStartPlayerTurnEvent(StartPlayerTurnEvent ev)
        {
            ExecuteAllCommandsByType(CommandTriggerType.StartPlayerTurn,ev);
        }

        private void OnEndPlayerTurnEvent(EndPlayerTurnEvent ev)
        {
            ExecuteAllCommandsByType(CommandTriggerType.EndPlayerTurn,ev);
        }

        // Game State Events
        private void OnGameStartEvent(GameStartEvent ev)
        {
            ExecuteAllCommandsByType(CommandTriggerType.GameStart,ev);
        }

        private void OnGameWonEvent(GameWonEvent ev)
        {
            ExecuteAllCommandsByType(CommandTriggerType.GameWon,ev);
        }

        // Player/Enemy Events
        private void OnPlayerDamagedEvent(PlayerDamagedEvent ev)
        {
            ExecuteAllCommandsByType(CommandTriggerType.PlayerDamaged,ev);
        }

        private void OnPlayerReduceHealthEvent(PlayerReduceHealthEvent ev)
        {
            ExecuteAllCommandsByType(CommandTriggerType.PlayerReduceHealth,ev);
        }
        
        private void OnPlayerBlockGainedEvent(PlayerBlockGainedEvent ev)
        {
            ExecuteAllCommandsByType(CommandTriggerType.PlayerBlockGained,ev);
        }

        // Card Events
        private void OnShuffledDeckEvent(ShuffledDeckEvent ev)
        {
            ExecuteAllCommandsByType(CommandTriggerType.ShuffledDeck,ev);
        }

        private void OnCardDrawnEvent(CardDrawnEvent ev)
        {
            ExecuteAllCommandsByType(CommandTriggerType.CardDrawn,ev);
        }

        private void OnAttackCardPlayedEvent(AttackCardPlayedEvent ev)
        {
            ExecuteAllCommandsByType(CommandTriggerType.AttackCardPlayed,ev);
        }
        
        private void ExecuteAllCommandsByType<T>(CommandTriggerType triggerType,T ev) where T : struct,IEvent
        {
            ExecutePersistentCommands(_persistentCommands, triggerType,ev);
            ExecuteOneShotCommands(_oneShotCommands, triggerType,ev);
        }

        private void ExecuteOneShotCommands<T>(List<ICommand> commands, CommandTriggerType triggerType,T ev) where T : struct,IEvent
        {
            Player player = EntityManager.Instance.PlayerEntity;
            List<Enemy> targets = EntityManager.Instance.Enemies;
            
            _toRemove.Clear();
            for(int i = 0; i < commands.Count; ++i)
            {
                var command = commands[i];
                command.SetPlayer(player);
                if (command is LateCommand lateCommand)
                    lateCommand.SetTargets(targets);
                
                if (ExecuteByType(command, triggerType, ev))
                    _toRemove.Add(command);
            }
            _toRemove.ForEach(c => commands.Remove(c));
            _toRemove.Clear();
        }

        private void ExecutePersistentCommands<T>(List<ICommand> commands, CommandTriggerType triggerType,T ev) where T : struct,IEvent
        {
            Player player = EntityManager.Instance.PlayerEntity;
            List<Enemy> targets = EntityManager.Instance.Enemies;

            foreach (var command in commands)
            {
                command.SetPlayer(player);
                if (command is LateCommand lateCommand)
                    lateCommand.SetTargets(targets);
     
                ExecuteByType(command, triggerType,ev);
            }
        }

        private bool ExecuteByType<T>(ICommand command, CommandTriggerType triggerType,T ev) where T : struct,IEvent
        {
            var isTriggered = triggerType switch
            {
                CommandTriggerType.GameStart => command.ExecuteOnGameStart(),
                CommandTriggerType.GameWon => command.ExecuteOnGameWon(),

                CommandTriggerType.PlayerDamaged => command.ExecuteOnPlayerDamaged(((PlayerDamagedEvent)(object)ev).Damage),
                CommandTriggerType.PlayerReduceHealth => command.ExecuteOnPlayerReduceHealth(((PlayerReduceHealthEvent)(object)ev).Health),
                CommandTriggerType.PlayerBlockGained => command.ExecuteOnPlayerBlockGained(((PlayerBlockGainedEvent)(object)ev).Amount),

                CommandTriggerType.StartPlayerTurn => command.ExecuteOnPlayerStartTurn(),
                CommandTriggerType.EndPlayerTurn => command.ExecuteOnPlayerEndTurn(),

                CommandTriggerType.ShuffledDeck => command.ExecuteOnShuffledDeck(),
                CommandTriggerType.CardDrawn => command.ExecuteOnCardDrawn(((CardDrawnEvent)(object)ev).Card),
                CommandTriggerType.AttackCardPlayed => command.ExecuteOnAttackCardPlayed(),
                _ => false
            };
            if(isTriggered)
                Debug.Log($"Execute Command: Trigger Type: {triggerType}");
            return isTriggered;
        }

        private void OnDestroy()
        {
            _oneShotCommands.Clear();
            _persistentCommands.Clear();
        }
    }
}