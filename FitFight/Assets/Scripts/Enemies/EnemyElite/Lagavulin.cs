using UnityEngine;

public class Lagavulin : EliteEnemy
{
    private bool isAwake = false;
    private int turnCounter = 0;

    public override void Start()
    {
        enemyName = "Lagavulin";
        maxHP = 112;
        base.Start();
        baseAttackDamage = 18;
        goldReward = Random.Range(50, 60);
    }

    public override void StartTurn()
    {
        turnCounter++;

        if (!isAwake)
        {
            if (turnCounter >= 3)
            {
                isAwake = true;
                Debug.Log($"{enemyName} wakes up and prepares to attack!");
            }
            else
            {
                Debug.Log($"{enemyName} is still asleep...");
                return;
            }
        }

        AttackPlayer();
    }
}
