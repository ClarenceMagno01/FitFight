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
            if (statusEffectManager != null)
            {
                statusEffectManager.ApplyStatusEffect("Reinforced", 1);
                statusEffectManager.ApplyStatusEffect("Pumped", 1);
                Debug.Log($"{enemyName} applies 1 Reinforced (boosts block gain) and 1 Pumped (boosts attack) to itself.");
            }
            else
            {
                Debug.LogError($"{enemyName} is missing a StatusEffectManager!");
            }
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
