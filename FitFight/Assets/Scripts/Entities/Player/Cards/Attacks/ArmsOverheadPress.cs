using UnityEngine;

public class ArmsOverheadPress : Card
{
    private int damage = 6;
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
        Debug.Log($"Arms - Overhead Press deals {damage} damage to {target.enemyName}!");

        // End player's turn (if applicable)
        player.EndTurn();
    }

    // Method to upgrade the card
    public override void UpgradeCard()
    {
        if (!isUpgraded)
        {
            isUpgraded = true;
            damage = 9; // Increase damage
            Debug.Log("Arms - Overhead Press upgraded! Now deals 9 damage.");
        }
    }
}
