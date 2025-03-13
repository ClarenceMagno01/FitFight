using UnityEngine;

public class Guardian : BossEnemy
{
    private bool isInDefensiveMode = false;

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
        if (isInDefensiveMode)
        {
            GainBlock(20);
            isInDefensiveMode = false;
            Debug.Log($"{enemyName} enters defensive mode and gains block.");
        }
        else
        {
            AttackPlayer();
            isInDefensiveMode = true;
            Debug.Log($"{enemyName} attacks and prepares for defense.");
        }
    }
}
