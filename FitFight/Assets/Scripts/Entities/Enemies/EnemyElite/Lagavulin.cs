using UnityEngine;

public class Lagavulin : EliteEnemy
{
    private bool isAwake = false;
    private int turnCounter = 0;

    public override void Start()
    {
        enemyName = "Lagavulin";
        maxHP = 112;
        base.Start();
        baseAttackDamage = 18;
        goldReward = Random.Range(50, 60);
    }

    public override void StartTurn()
    {
        turnCounter++;

        if (!isAwake)
        {
            if (turnCounter >= 3)
            {
                WakeUp();
            }
            else
            {
                Debug.Log($"{enemyName} is still asleep...");
                return;
            }
        }
        else
        {
            PerformAttackPattern();
        }

        EndTurn();
    }

    private void WakeUp()
    {
        isAwake = true;
        Debug.Log($"{enemyName} wakes up and prepares to attack!");
    }

    public override void TakeDamage(int damage)
    {
        if (!isAwake)
        {
            WakeUp(); // If attacked while asleep, Lagavulin wakes up
        }
        base.TakeDamage(damage);
    }

    private void PerformAttackPattern()
    {
        if (turnCounter % 2 == 0)
        {
            // Every second turn: Inflicts Weak on player
            player.ApplyStatusEffect("Weak", 2);
            Debug.Log($"{enemyName} uses a Debuff! Player gains 2 Weak stacks.");
        }
        else
        {
            // Standard attack turn
            AttackPlayer();
            Debug.Log($"{enemyName} attacks for {baseAttackDamage} damage.");
        }
    }
}
