using UnityEngine;

public class Slaver : EnemyStats
{
    public enum SlaverType { Blue, Red }
    public SlaverType type;
    private int turnCounter = 0;

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
        turnCounter++;

        if (turnCounter % 2 == 0)
        {
            ApplyDebuff();
        }
        else
        {
            AttackPlayer();
            Debug.Log($"{enemyName} attacks for {baseAttackDamage} damage.");
        }

        EndTurn();
    }

    private void ApplyDebuff()
    {
        if (type == SlaverType.Blue)
        {
            player.ApplyStatusEffect("Lethargy", 1);
            Debug.Log($"{enemyName} applies Lethargy (reduces player attack by 25%).");
        }
        else if (type == SlaverType.Red)
        {
            player.ApplyStatusEffect("Entangle", 1);
            Debug.Log($"{enemyName} applies Entangle (prevents player from attacking).");
        }
    }
}
