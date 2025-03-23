using UnityEngine;

public class AbsOverheadLungeTwist : Card
{
    private int damage = 8;
    private int exposedStacks = 2;
    private bool isUpgraded = false;

    public override void PlayCard(PlayerStats player, EnemyStats target)
    {
        if (target == null)
        {
            Debug.Log("No target selected!");
            return;
        }

        // Deal damage
        target.TakeDamage(damage);
        Debug.Log($"Abs - Overhead Lunge Twist deals {damage} damage to {target.enemyName}!");

        // Apply Exposed status effect
        target.AddStatusEffect("Exposed", exposedStacks);
        Debug.Log($"{target.enemyName} gains {exposedStacks} stacks of Exposed!");

    }

    // Method to upgrade the card
    public override void UpgradeCard()
    {
        if (!isUpgraded)
        {
            isUpgraded = true;
            damage += 2;        // Increase damage by 2
            exposedStacks += 1; // Increase Exposed stacks by 1
            Debug.Log("Abs - Overhead Lunge Twist upgraded! Now deals more damage and applies more Exposed.");
        }
    }
}
