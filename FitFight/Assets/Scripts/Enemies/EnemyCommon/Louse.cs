using UnityEngine;

public class Louse : EnemyStats
{
    public enum LouseType { Red, Green }
    public LouseType type;

    public override void Start()
    {
        enemyName = type == LouseType.Red ? "Red Louse" : "Green Louse";
        maxHP = Random.Range(10, 20);
        currentHP = maxHP;
        baseAttackDamage = type == LouseType.Red ? 5 : 3;
        goldReward = Random.Range(8, 12);
    }

    public override void StartTurn()
    {
        if (Random.value < 0.5f)
        {
            AttackPlayer();
        }
        else
        {
            player.ApplyStatusEffect(type == LouseType.Red ? "Weak" : "Feeble", 1);
            Debug.Log($"{enemyName} applies a debuff!");
        }
    }
}
