using UnityEngine;

public class Slaver : EnemyStats
{
    public enum SlaverType { Blue, Red }
    public SlaverType type;

    public override void Start()
    {
        enemyName = type == SlaverType.Blue ? "Blue Slaver" : "Red Slaver";
        maxHP = type == SlaverType.Blue ? 46 : 50;
        currentHP = maxHP;
        baseAttackDamage = type == SlaverType.Blue ? 7 : 12;
        goldReward = Random.Range(25, 30);
    }

    public override void StartTurn()
    {
        if (type == SlaverType.Blue)
        {
            player.ApplyStatusEffect("Weak", 1);
            Debug.Log($"{enemyName} applies Weak.");
        }
        else if (type == SlaverType.Red)
        {
            player.ApplyStatusEffect("Entangle", 1);
            Debug.Log($"{enemyName} applies Entangle (No Attacks!).");
        }
        AttackPlayer();
    }
}
