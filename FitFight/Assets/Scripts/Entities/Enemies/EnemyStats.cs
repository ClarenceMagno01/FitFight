using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public string enemyName;
    public int maxHP;
    public int currentHP;
    public int baseAttackDamage;
    public int attackDamage; // Attack damage affected by status effects
    public int block;
    public int goldReward; // Gold given to the player on death

    public PlayerStats player;
    public TurnManager turnManager; // Reference to Turn Manager

    public StatusEffectManager statusEffectManager;
    private Dictionary<string, int> statusEffectStacks = new Dictionary<string, int>();

    public virtual void Start()
    {
        currentHP = maxHP;
        attackDamage = baseAttackDamage;
        statusEffectManager = GetComponent<StatusEffectManager>();

        if (statusEffectManager == null)
        {
            statusEffectManager = gameObject.AddComponent<StatusEffectManager>(); // Attach dynamically if missing
        }
    }

    public virtual void StartTurn()
    {
        // Apply turn-based effects (like Burn)
        ApplyOngoingEffects();

        // Check if enemy is Entangled (prevents attacking)
        if (!HasStatusEffect("Entangled"))
        {
            AttackPlayer();
        }
        else
        {
            Debug.Log($"{enemyName} is Entangled and cannot attack!");
        }

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
    public void Heal(int healAmount)
    {
        currentHP += healAmount;
        if (currentHP > maxHP) currentHP = maxHP;

        Debug.Log($"{enemyName} heals {healAmount} HP. Current HP: {currentHP}/{maxHP}");
    }


    public void GainBlock(int amount)
    {
        block += amount;
        Debug.Log($"{enemyName} gains {amount} block.");
    }

    public void AttackPlayer()
    {
        player.TakeDamage(attackDamage);
        Debug.Log($"{enemyName} attacks for {attackDamage} damage!");
    }

    public virtual void OnDeath()
    {
        Debug.Log($"{enemyName} has been defeated!");
        player.GainGold(goldReward);
        turnManager.RemoveEnemy(this); // Removes the enemy from the turn list
    }

    protected void Die()
    {
        OnDeath();
    }

    public void EndTurn()
    {
        Debug.Log($"{enemyName} ends its turn.");
    }

    // === Status Effect System ===
    public void AddStatusEffect(string effect, int stacks = 1)
    {
        if (statusEffectStacks.ContainsKey(effect))
        {
            statusEffectStacks[effect] += stacks;
        }
        else
        {
            statusEffectStacks[effect] = stacks;
        }

        statusEffectManager?.ApplyStatusEffect(effect, stacks);
    }

    public void RemoveStatusEffect(string effect, int stacks = 1)
    {
        if (statusEffectStacks.ContainsKey(effect))
        {
            statusEffectStacks[effect] -= stacks;
            if (statusEffectStacks[effect] <= 0)
            {
                statusEffectStacks.Remove(effect);
            }
        }

        statusEffectManager?.RemoveStatusEffect(effect, stacks);
    }

    public bool HasStatusEffect(string effect)
    {
        return statusEffectStacks.ContainsKey(effect);
    }

    public int GetStatusEffectStacks(string effect)
    {
        return statusEffectStacks.ContainsKey(effect) ? statusEffectStacks[effect] : 0;
    }

    private void ApplyOngoingEffects()
    {
        if (HasStatusEffect("Burn"))
        {
            int burnStacks = GetStatusEffectStacks("Burn");
            int burnDamage = 2 * burnStacks;
            TakeDamage(burnDamage);
            RemoveStatusEffect("Burn", 1); // Burn reduces by 1 stack each turn
            Debug.Log($"{enemyName} suffers {burnDamage} Burn damage. Remaining Burn stacks: {burnStacks - 1}");
        }
    }
}
