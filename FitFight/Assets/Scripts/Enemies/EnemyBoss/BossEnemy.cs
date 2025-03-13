using UnityEngine;

public class BossEnemy : EnemyStats
{
    public override void Start()
    {
        base.Start();
        maxHP = Mathf.RoundToInt(maxHP * 2f); // Bosses have double HP
        currentHP = maxHP;
        baseAttackDamage = Mathf.RoundToInt(baseAttackDamage * 1.5f); // 50% more attack damage
        goldReward = Mathf.RoundToInt(goldReward * 2f); // Bosses give double the gold
    }
}
