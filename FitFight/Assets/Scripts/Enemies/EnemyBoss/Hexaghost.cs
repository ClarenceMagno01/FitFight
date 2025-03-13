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

        if (turnCounter % 6 == 0)
        {
            baseAttackDamage = flameDamage * 6; // Burst attack
            flameDamage += 2; // Increases for next time
            Debug.Log($"{enemyName} unleashes a massive flame attack!");
        }
        else
        {
            AttackPlayer();
        }
    }
}
