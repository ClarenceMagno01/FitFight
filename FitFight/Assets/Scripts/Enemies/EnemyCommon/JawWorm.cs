using UnityEngine;

public class JawWorm : EnemyStats
{
    private bool isDefensive = false;

    public override void Start()
    {
        enemyName = "Jaw Worm";
        maxHP = Random.Range(40, 44);
        currentHP = maxHP;
        baseAttackDamage = 11;
        goldReward = Random.Range(20, 25);
    }

    public override void StartTurn()
    {
        if (isDefensive)
        {
            player.ApplyStatusEffect("Reinforced", 1);
            player.ApplyStatusEffect("Pumped", 1);
            Debug.Log($"{enemyName} applies 1 Reinforced (boosts block gain) and 1 Pumped (boosts attack) to itself.");
        }
        else
        {
            AttackPlayer();
            Debug.Log($"{enemyName} attacks for {baseAttackDamage} damage.");
        }

        isDefensive = !isDefensive;
        EndTurn();
    }
}
