namespace _Project.Scripts.Main.Managers
{
    public enum CommandTriggerType
    {
        // GameState
        GameStart,
        GameWon,

        // Player/Enemy
        PlayerDamaged,
        PlayerReduceHealth,
        PlayerDie,
        PlayerBlockGained,

        // Turns
        StartPlayerTurn,
        EndPlayerTurn,

        // Card
        ShuffledDeck,
        CardDrawn,
        AttackCardPlayed
    }
}