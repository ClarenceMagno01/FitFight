using UnityEngine;

public class Sentry : EliteEnemy
{
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
        player.ApplyStatusEffect("Dazed", 1);
        Debug.Log($"{enemyName} inflicts Dazed!");
        AttackPlayer();
    }
}
