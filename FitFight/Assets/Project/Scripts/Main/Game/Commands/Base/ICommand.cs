using _Project.Scripts.Main.Game.Entity;
using _Project.Scripts.Main.Game.PlayerTurn.Systems;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Main.Game.Commands.Base
{
    public interface ICommand
    {
        #region GameState
        bool ExecuteOnGameStart();
        bool ExecuteOnGameWon();
        #endregion

        #region Player/Enemy
        bool ExecuteOnPlayerDamaged(int damage);
        bool ExecuteOnPlayerReduceHealth(int health);
        bool ExecuteOnPlayerBlockGained(int amount);
        #endregion
        
        #region Turns
        bool ExecuteOnPlayerStartTurn();
        bool ExecuteOnPlayerEndTurn();
        #endregion

        #region Card
        bool ExecuteOnShuffledDeck();
        bool ExecuteOnCardDrawn(CardInstance card);
        bool ExecuteOnAttackCardPlayed();
        #endregion

 
        UniTask ExecuteImmediate();
        void SetPlayer(Player player);
    }
}