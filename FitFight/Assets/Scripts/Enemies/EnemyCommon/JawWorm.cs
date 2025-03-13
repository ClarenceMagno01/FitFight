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
            GainBlock(6);
            player.ApplyStatusEffect("Pumped", 1);
            Debug.Log($"{enemyName} gains block and strength!");
        }
        else
        {
            AttackPlayer();
        }
        isDefensive = !isDefensive;
    }
}
