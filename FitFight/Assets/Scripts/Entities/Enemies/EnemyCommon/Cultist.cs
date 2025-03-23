using UnityEngine;

public class Cultist : EnemyStats
{
    private int ritualStacks = 1;
    private bool hasBuffed = false; // Used to check if Ritual has been used

    public override void Start()
    {
        enemyName = "Cultist";
        maxHP = Random.Range(48, 54);
        currentHP = maxHP;
        baseAttackDamage = 6; // Initial attack damage
        goldReward = Random.Range(15, 20);
    }

    public override void StartTurn()
    {
        if (!hasBuffed)
        {
            // First turn: Apply Ritual (Gains Pumped every turn)
            StatusEffectManager enemyStatusManager = GetComponent<StatusEffectManager>();
            if (enemyStatusManager != null)
            {
                enemyStatusManager.ApplyStatusEffect("Pumped", ritualStacks);
                Debug.Log($"{enemyName} chants Ritual! Gains {ritualStacks} Pumped per turn.");
            }
            else
            {
                Debug.LogWarning($"{enemyName} is missing a StatusEffectManager component!");
            }

            hasBuffed = true;
        }
        else
        {
            // Every turn after the first: Attack
            AttackPlayer();
            Debug.Log($"{enemyName} attacks for {baseAttackDamage} damage!");
        }

        EndTurn();
    }
}
