using UnityEngine;

public class Guardian : BossEnemy
{
    private bool isInDefensiveMode = true;

    public override void Start()
    {
        enemyName = "Guardian";
        maxHP = 220;
        base.Start();
        baseAttackDamage = 14;
        goldReward = Random.Range(95, 115);
    }

    public override void StartTurn()
    {
        if (currentHP <= maxHP / 2)
        {
            MultiHitAttack(5, 4); // 5 hits of 4 damage each
            Debug.Log($"{enemyName} unleashes a powerful multi-hit attack!");
        }
        else if (isInDefensiveMode)
        {
            GainBlock(20);
            Debug.Log($"{enemyName} gains block and prepares to attack.");
            isInDefensiveMode = false;
        }
        else
        {
            AttackPlayer();
            Debug.Log($"{enemyName} attacks for {baseAttackDamage} damage!");
            isInDefensiveMode = true;
        }

        EndTurn();
    }

    // 🔹 Properly Overrides TakeDamage Now That It’s Virtual in EnemyStats
    public override void TakeDamage(int damage)
    {
        if (isInDefensiveMode)
        {
            GainBlock(5); // Gains extra block when attacked in Defensive Mode
            Debug.Log($"{enemyName} braces for impact and gains 5 block!");
        }

        base.TakeDamage(damage);
    }

    private void MultiHitAttack(int hits, int damagePerHit)
    {
        for (int i = 0; i < hits; i++)
        {
            player.TakeDamage(damagePerHit);
        }
    }
}
