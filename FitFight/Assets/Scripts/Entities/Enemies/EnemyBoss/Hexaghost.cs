using UnityEngine;

public class Hexaghost : BossEnemy
{
    private int turnCounter = 0;
    private int flameDamage = 6; // Starts low, scales up

    public override void Start()
    {
        enemyName = "Hexaghost";
        maxHP = 250;
        base.Start();
        baseAttackDamage = 6;
        goldReward = Random.Range(100, 120);
    }

    public override void StartTurn()
    {
        turnCounter++;

        if (turnCounter == 1)
        {
            // First Turn: Applies 3 stacks of Burn
            player.ApplyStatusEffect("Burn", 3);
            Debug.Log($"{enemyName} fills the battlefield with flames! Player gains 3 Burn stacks.");
        }
        else if (turnCounter % 6 == 0)
        {
            // Every 6 turns: Unleash a powerful Flame Burst attack
            MultiHitAttack(6, flameDamage);
            Debug.Log($"{enemyName} unleashes a massive Flame Burst! ({flameDamage} x 6 hits)");

            // Increase future Flame Burst damage
            flameDamage += 2;
        }
        else
        {
            // Normal attack turn
            AttackPlayer();
            Debug.Log($"{enemyName} attacks for {baseAttackDamage} damage.");
        }

        EndTurn();
    }

    private void MultiHitAttack(int hits, int damagePerHit)
    {
        for (int i = 0; i < hits; i++)
        {
            player.TakeDamage(damagePerHit);
        }
    }
}
