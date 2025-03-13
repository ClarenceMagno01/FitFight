using UnityEngine;

public class Cultist : EnemyStats
{
    private int ritualStacks = 1;

    public override void Start()
    {
        enemyName = "Cultist";
        maxHP = Random.Range(48, 54);
        currentHP = maxHP;
        baseAttackDamage = 6;
        goldReward = Random.Range(15, 20);
    }

    public override void StartTurn()
    {
        player.ApplyStatusEffect("Pumped", ritualStacks); // Gains Strength each turn
        Debug.Log($"{enemyName} chants Ritual! Gains {ritualStacks} Pumped.");
    }
}
