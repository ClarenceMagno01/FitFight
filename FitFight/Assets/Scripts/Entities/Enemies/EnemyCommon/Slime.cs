using UnityEngine;

public class Slime : EnemyStats
{
    public enum SlimeType { Spike, Acid }
    public SlimeType type;
    private bool splitTriggered = false;

    public GameObject smallSlimePrefab; // Assign a prefab for small slimes in Unity

    public override void Start()
    {
        enemyName = type == SlimeType.Spike ? "Spike Slime" : "Acid Slime";
        maxHP = type == SlimeType.Spike ? 64 : 60;
        currentHP = maxHP;
        baseAttackDamage = type == SlimeType.Spike ? 12 : 7;
        goldReward = Random.Range(18, 22);
    }

    public override void StartTurn()
    {
        if (currentHP <= maxHP / 2 && !splitTriggered)
        {
            Split();
        }
        else
        {
            AttackPlayer();
            if (type == SlimeType.Acid)
            {
                player.ApplyStatusEffect("Feeble", 1);
                Debug.Log($"{enemyName} applies Feeble!");
            }
        }

        EndTurn();
    }

    private void Split()
    {
        splitTriggered = true;
        Debug.Log($"{enemyName} splits into smaller versions!");

        if (smallSlimePrefab != null)
        {
            Instantiate(smallSlimePrefab, transform.position + Vector3.left, Quaternion.identity);
            Instantiate(smallSlimePrefab, transform.position + Vector3.right, Quaternion.identity);
        }

        Die(); // The large slime disappears after splitting
    }
}
