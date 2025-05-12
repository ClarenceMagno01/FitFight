using System.Collections.Generic;
using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Game.Entity;
using _Project.Scripts.Main.Game.PlayerTurn.Systems;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Main.Game.Commands
{
    public class LateCommand : BaseCommand
    {
        [Header("Execute On Event")]
        [Space]
        
        [SerializeField] private bool _gameStart;
        [ShowIf("_gameStart")]
        [SerializeReference] private ICommand _onGameStart;
        
        [SerializeField] private bool _gameWon;
        [ShowIf("_gameWon")]
        [SerializeReference] private ICommand _onGameWon;

        // [SerializeField] private bool _playerDamaged;
        // [ShowIf("_playerDamaged")]
        // [SerializeReference] private ICommand _onPlayerDamaged;
        //
        // [SerializeField] private bool _playerReduceHealth;
        // [ShowIf("_playerReduceHealth")]
        // [SerializeReference] private ICommand _onPlayerReduceHealth;
        
        [SerializeField] private bool _blockGained;
        [ShowIf("_blockGained")]
        [SerializeReference] private ICommand _onBlockGained;

        [SerializeField] private bool _startPlayerTurn;
        [ShowIf("_startPlayerTurn")]
        [SerializeReference] private ICommand _startPlayerTurnCommand;

        [SerializeField] private bool _endPlayerTurn;
        [ShowIf("_endPlayerTurn")]
        [SerializeReference] private ICommand _endPlayerTurnCommand;

        // [SerializeField] private bool _shuffledDeck;
        // [ShowIf("_shuffledDeck")]
        // [SerializeReference] private ICommand _onShuffledDeck;
        //
        // [SerializeField] private bool _cardDrawn;
        // [ShowIf("_cardDrawn")]
        // [SerializeReference] private ICommand _onCardDrawn;

        // [SerializeField] private bool _attackCardPlayed;
        // [ShowIf("_attackCardPlayed")]
        // [SerializeReference] private ICommand _onAttackCardPlayed;

        public bool isPersistent;
        
        private List<Enemy> _targets;
        public void SetTargets(List<Enemy> targets)
        {
            _targets = targets;
        }

        #region GameState

        public override bool ExecuteOnGameStart()
        {
            if (_gameStart)
            {
                ExecuteCommand(_onGameStart);
                return true;
            }
            return false;
        }
        
        public override bool ExecuteOnGameWon()
        {
            if (_gameWon)
            {
                ExecuteCommand(_onGameWon);
                return true;
            }
            return false;
        }

        #endregion
 
        #region Player/Enemy

        // public override bool ExecuteOnPlayerDamaged(int damage)
        // {
        //     // if(_playerDamaged)
        //     //     _onPlayerDamaged?.ExecuteImmediate();
        //     return true;
        // }

        // public override bool ExecuteOnPlayerReduceHealth(int health)
        // {
        //     if (_playerReduceHealth)
        //     {
        //         _onPlayerReduceHealth?.ExecuteImmediate();
        //         return true;
        //     }
        //     return false;
        // }
        
        public override bool ExecuteOnPlayerBlockGained(int amount)
        {
            if (_blockGained)
            {
                ExecuteCommand(_onBlockGained);
                return true;
            }
            return false;
        }
        #endregion

        #region Turns

        public override bool ExecuteOnPlayerStartTurn()
        {
            if (_startPlayerTurn)
            {
                ExecuteCommand(_startPlayerTurnCommand);
                return true;
            }
            return false;
        }

        public override bool ExecuteOnPlayerEndTurn()
        {
            if (_endPlayerTurn)
            {
                ExecuteCommand(_endPlayerTurnCommand);
                return true;
            }
            return false;
        }

        #endregion

        #region Card

        // public override bool ExecuteOnShuffledDeck()
        // {
        //     if (_shuffledDeck)
        //         _onShuffledDeck?.ExecuteImmediate();
        //     return true;
        // }
        //
        // public override bool ExecuteOnCardDrawn(CardInstance card)
        // {
        //     if (_cardDrawn)
        //         _onCardDrawn?.ExecuteImmediate();
        //     return true;
        // }

        // public override bool ExecuteOnAttackCardPlayed()
        // {
        //     if (_attackCardPlayed)
        //     {
        //         _onAttackCardPlayed?.ExecuteImmediate();
        //         return true;
        //     }
        //     return false;
        // }
        #endregion
        
        private void ExecuteCommand(ICommand command)
        {
            Debug.Log($"Late Command: {command.GetType().Name} executed");
            command.SetPlayer(PlayerEntity);
            if (command is EnemyTargetBaseCommand targetCommand)
            {
                targetCommand.SetPriority(-1);
                targetCommand.SetTargets(_targets);
            }
            command.ExecuteImmediate().Forget();
        }
    }
}