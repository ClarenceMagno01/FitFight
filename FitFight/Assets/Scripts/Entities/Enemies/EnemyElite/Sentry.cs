using UnityEngine;

public class Sentry : EliteEnemy
{
    private int turnCounter = 0;

    public override void Start()
    {
        enemyName = "Sentry";
        maxHP = 38;
        base.Start();
        baseAttackDamage = 10;
        goldReward = Random.Range(40, 50);
    }

    public override void StartTurn()
    {
        turnCounter++;

        if (turnCounter % 2 == 0)
        {
            // Every second turn: Inflicts Lethargy (reduces player attack)
            if (player.TryGetComponent<StatusEffectManager>(out StatusEffectManager playerStatusManager))
            {
                playerStatusManager.ApplyStatusEffect("Lethargy", 1);
                Debug.Log($"{enemyName} emits a weakening pulse! Player gains 1 Lethargy stack.");
            }
            else
            {
                Debug.LogWarning("Player is missing a StatusEffectManager component!");
            }
        }
        else
        {
            // Normal attack turn
            AttackPlayer();
            Debug.Log($"{enemyName} attacks for {baseAttackDamage} damage.");
        }

        EndTurn();
    }
}
