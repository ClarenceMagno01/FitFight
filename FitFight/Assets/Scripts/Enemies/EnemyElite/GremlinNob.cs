using UnityEngine;

public class GremlinNob : EliteEnemy
{
    public override void Start()
    {
        enemyName = "Gremlin Nob";
        maxHP = 82;
        base.Start();
        baseAttackDamage = 14;
        goldReward = Random.Range(45, 55);
    }

    public override void StartTurn()
    {
        AttackPlayer();
    }

    public void OnPlayerUsesSkill()
    {
        player.ApplyStatusEffect("Pumped", 2);
        Debug.Log($"{enemyName} gains strength when the player uses a skill!");
    }
}
