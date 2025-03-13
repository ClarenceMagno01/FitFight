using UnityEngine;

public class FungiBeast : EnemyStats
{
    public override void Start()
    {
        enemyName = "Fungi Beast";
        maxHP = Random.Range(28, 32);
        currentHP = maxHP;
        baseAttackDamage = 6;
        goldReward = Random.Range(12, 16);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        player.ApplyStatusEffect("Exposed", 1);
        Debug.Log($"{enemyName} dies! Player becomes Exposed.");
    }
}
