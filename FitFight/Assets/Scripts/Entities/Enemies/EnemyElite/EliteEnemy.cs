using UnityEngine;

public class EliteEnemy : EnemyStats
{
    public override void Start()
    {
        base.Start();
        maxHP = Mathf.RoundToInt(maxHP * 1.5f); // 50% more HP
        currentHP = maxHP;
        baseAttackDamage = Mathf.RoundToInt(baseAttackDamage * 1.3f); // 30% more Damage
        goldReward = Mathf.RoundToInt(goldReward * 1.5f); // 50% more Gold
    }
}
