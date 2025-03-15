using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public string enemyName;
    public int maxHP;
    public int currentHP;
    public int baseAttackDamage;
    public int block;
    public int goldReward; // Gold given to the player on death

    public PlayerStats player;
    public TurnManager turnManager; // Reference to Turn Manager

    public virtual void Start()
    {
        currentHP = maxHP;
    }

    public virtual void StartTurn()
    {
        AttackPlayer();
        EndTurn();
    }

    public virtual void TakeDamage(int damage)
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
        turnManager.RemoveEnemy(this); // Removes the enemy from the turn list
    }

    protected void Die() // Change from private to protected
    {
        OnDeath();
    }

    public void EndTurn()
    {
        Debug.Log($"{enemyName} ends its turn.");
    }
}
