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
        Debug.Log($"{enemyName} attacks for {baseAttackDamage} damage.");
        EndTurn();
    }

    public void OnPlayerUsesSkill()
    {
        if (player != null && player.TryGetComponent<StatusEffectManager>(out StatusEffectManager playerStatusManager))
        {
            playerStatusManager.ApplyStatusEffect("Pumped", 2); // Gains 2 Pumped
            Debug.Log($"{enemyName} becomes enraged! Gains 2 Pumped.");
        }
        else
        {
            Debug.LogError("StatusEffectManager is missing from PlayerStats!");
        }
    }
}
