using System;
using _Project.Scripts.Main.Game.Entity;
using _Project.Scripts.Main.Game.PlayerTurn.Systems;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands.Base
{
    public abstract class BaseCommand : ICommand
    {
        private Player _player;
        protected Player PlayerEntity => _player ?? throw new NullReferenceException($"{GetType().Name}: Player should be assigned first");
    
        public void SetPlayer(Player player)
        {
            _player = player;
        }

        public virtual bool ExecuteOnGameStart()
        {
            return false;
        }

        public virtual bool ExecuteOnGameWon()
        {
            return false;
        }

        public virtual bool ExecuteOnPlayerDamaged(int damage)
        {
            return false;
        }

        public virtual bool ExecuteOnPlayerReduceHealth(int health)
        {
            return false;
        }

        public virtual bool ExecuteOnPlayerDie()
        {
            return false;
        }

        public virtual bool ExecuteOnPlayerBlockGained(int amount)
        {
            return false;
        }

        public virtual bool ExecuteOnPlayerStartTurn()
        {
            return false;
        }

        public virtual bool ExecuteOnPlayerEndTurn()
        {
            return false;
        }

        public virtual bool ExecuteOnShuffledDeck()
        {
            return false;
        }

        public virtual bool ExecuteOnCardDrawn(CardInstance card)
        {
            return false;
        }

        public virtual bool ExecuteOnAttackCardPlayed()
        {
            return false;
        }

        public virtual async UniTask ExecuteImmediate()
        {
            await UniTask.Yield();
            Debug.Log($"{GetType().Name} Executed Immediately");
        }
    }
}