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
            Debug.Log($"{enemyName} attacks for {baseAttackDamage} damage.");
        }
        else
        {
            ApplyDebuff();
        }

        EndTurn();
    }

    private void ApplyDebuff()
    {
        if (player.TryGetComponent<StatusEffectManager>(out StatusEffectManager playerStatusManager))
        {
            if (type == LouseType.Red)
            {
                playerStatusManager.ApplyStatusEffect("Lethargy", 1);
                Debug.Log($"{enemyName} applies Lethargy (reduces player attack by 25%).");
            }
            else if (type == LouseType.Green)
            {
                playerStatusManager.ApplyStatusEffect("Feeble", 1);
                Debug.Log($"{enemyName} applies Feeble (reduces block gain by 25%).");
            }
        }
        else
        {
            Debug.LogError("StatusEffectManager is missing from PlayerStats!");
        }
    }
}
