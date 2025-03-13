using UnityEngine;

public class SlimeBoss : BossEnemy
{
    private bool hasSplit = false;

    public override void Start()
    {
        enemyName = "Slime Boss";
        maxHP = 240;
        base.Start();
        baseAttackDamage = 16;
        goldReward = Random.Range(90, 110);
    }

    public override void StartTurn()
    {
        if (currentHP <= maxHP / 2 && !hasSplit)
        {
            Split();
        }
        else
        {
            AttackPlayer();
        }
    }

    private void Split()
    {
        hasSplit = true;
        Debug.Log($"{enemyName} splits into two Slimes!");
        // Logic to spawn two smaller slimes can go here
    }
}
