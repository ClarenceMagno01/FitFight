using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string characterName;
    public int maxHP;
    public int currentHP;
    public int baseAttackDamage;
    public int attackDamage;
    public int baseBlock;
    public int block;
    public int energy = 0;
    
    private Dictionary<string, int> statusEffectStacks = new Dictionary<string, int>();
    public StatusEffectManager statusEffectManager;

    protected virtual void Start()
    {
        currentHP = maxHP;
        statusEffectManager = GetComponent<StatusEffectManager>();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;

        Debug.Log($"{characterName} takes {damage} damage. HP: {currentHP}/{maxHP}");

        if (currentHP == 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHP += healAmount;
        if (currentHP > maxHP) currentHP = maxHP;

        Debug.Log($"Player heals {healAmount} HP. Current HP: {currentHP}/{maxHP}");
    }

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

    protected virtual void Die()
    {
        Debug.Log($"{characterName} has died.");
    }
}
