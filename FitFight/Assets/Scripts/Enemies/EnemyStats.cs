using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public string enemyName;
    public int maxHP;
    public int currentHP;
    public int baseAttackDamage;
    public int block;
    public int goldReward; // Gold given to the player on death

    public PlayerStats player; // Reference to the player

    public virtual void Start()
    {
        currentHP = maxHP;
    }

    public virtual void StartTurn() { }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;

        Debug.Log($"{enemyName} takes {damage} damage. HP: {currentHP}/{maxHP}");

        if (currentHP == 0)
        {
            Die();
        }
    }

    public void GainBlock(int amount)
    {
        block += amount;
        Debug.Log($"{enemyName} gains {amount} block.");
    }

    public void AttackPlayer()
    {
        player.TakeDamage(baseAttackDamage);
        Debug.Log($"{enemyName} attacks for {baseAttackDamage} damage!");
    }

    public virtual void OnDeath()
    {
        Debug.Log($"{enemyName} has been defeated!");
        player.GainGold(goldReward);
    }

    private void Die()
    {
        OnDeath();
        Destroy(gameObject); // Removes enemy from scene
    }
}
