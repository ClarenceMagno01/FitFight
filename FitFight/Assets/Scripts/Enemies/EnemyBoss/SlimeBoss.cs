using UnityEngine;

public class SlimeBoss : BossEnemy
{
    private bool hasSplit = false;
    private int turnCounter = 0;

    public GameObject mediumSlimePrefab; // Assign this in Unity to spawn smaller slimes

    public override void Start()
    {
        enemyName = "Slime Boss";
        maxHP = 240;
        base.Start();
        baseAttackDamage = 16; // Default attack
        goldReward = Random.Range(90, 110);
    }

    public override void StartTurn()
    {
        turnCounter++;

        if (currentHP <= maxHP / 2 && !hasSplit)
        {
            Split();
        }
        else
        {
            PerformAttackPattern();
        }

        EndTurn();
    }

    private void PerformAttackPattern()
    {
        if (turnCounter % 3 == 1)
        {
            // Turn 1: Strong attack
            baseAttackDamage = 35;
            AttackPlayer();
            Debug.Log($"{enemyName} uses SLIMY CRUSH for {baseAttackDamage} damage!");
        }
        else if (turnCounter % 3 == 2)
        {
            // Turn 2: Normal attack
            baseAttackDamage = 16;
            AttackPlayer();
            Debug.Log($"{enemyName} uses a regular SLIME ATTACK for {baseAttackDamage} damage.");
        }
        else
        {
            // Turn 3: Preparing for a split (Does nothing)
            Debug.Log($"{enemyName} jiggles menacingly...");
        }
    }

    private void Split()
    {
        hasSplit = true;
        Debug.Log($"{enemyName} splits into two Medium Slimes!");

        if (mediumSlimePrefab != null)
        {
            Instantiate(mediumSlimePrefab, transform.position + Vector3.left, Quaternion.identity);
            Instantiate(mediumSlimePrefab, transform.position + Vector3.right, Quaternion.identity);
        }

        Die(); // Slime Boss disappears after splitting
    }
}
