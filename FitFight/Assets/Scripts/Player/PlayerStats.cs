using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public int maxMP = 50;
    public int currentMP;
    public int baseAttackDamage = 10;
    public int baseBlock = 5;
    public int attackDamage;
    public int block;
    public int energy = 0;
    public int gold = 100; // Player's gold for shop purchases

    private Dictionary<string, int> statusEffects = new Dictionary<string, int>();
    private string equippedArtifact = null; // The player's held artifact

    void Start()
    {
        currentHP = maxHP;
        currentMP = maxMP;
        ResetStats();
    }

    // Reset stats before applying status effects
    private void ResetStats()
    {
        attackDamage = baseAttackDamage;
        block = baseBlock;
    }

    // Apply a status effect (stackable)
    public void ApplyStatusEffect(string effect, int stacks = 1)
    {
        if (statusEffects.ContainsKey("Immune"))
        {
            Debug.Log($"Player is immune to {effect}!");
            return;
        }

        if (statusEffects.ContainsKey(effect))
        {
            statusEffects[effect] += stacks;
        }
        else
        {
            statusEffects[effect] = stacks;
        }

        Debug.Log($"{effect} applied. Total stacks: {statusEffects[effect]}");
        ApplyStatusEffects();
    }

    // Remove a status effect (or reduce stacks)
    public void RemoveStatusEffect(string effect, int stacks = 1)
    {
        if (statusEffects.ContainsKey(effect))
        {
            statusEffects[effect] -= stacks;
            if (statusEffects[effect] <= 0)
            {
                statusEffects.Remove(effect);
                Debug.Log($"{effect} removed.");
            }
            else
            {
                Debug.Log($"{effect} reduced to {statusEffects[effect]} stacks.");
            }
        }
        ApplyStatusEffects();
    }

    // Calculate status effects
    private void ApplyStatusEffects()
    {
        ResetStats(); // Reset stats before reapplying effects

        if (statusEffects.ContainsKey("Lethargy"))
        {
            float reduction = 1 - (0.25f * statusEffects["Lethargy"]);
            attackDamage = Mathf.RoundToInt(baseAttackDamage * reduction);
        }

        if (statusEffects.ContainsKey("Exposed"))
        {
            float increase = 1 + (0.5f * statusEffects["Exposed"]);
            currentHP -= Mathf.RoundToInt(currentHP * (increase - 1));
        }

        if (statusEffects.ContainsKey("Pumped"))
        {
            attackDamage += statusEffects["Pumped"];
        }

        if (statusEffects.ContainsKey("Reinforced"))
        {
            block += statusEffects["Reinforced"];
        }

        if (statusEffects.ContainsKey("Feeble"))
        {
            float reduction = 1 - (0.25f * statusEffects["Feeble"]);
            block = Mathf.RoundToInt(block * reduction);
        }

        if (statusEffects.ContainsKey("Warmup"))
        {
            energy = Mathf.RoundToInt(energy / 2);
        }

        Debug.Log($"Updated Stats - Attack: {attackDamage}, Block: {block}, HP: {currentHP}/{maxHP}");
    }

    // Take damage (affected by Exposed)
    public void TakeDamage(int damage)
    {
        if (statusEffects.ContainsKey("Exposed"))
        {
            float increase = 1 + (0.5f * statusEffects["Exposed"]);
            damage = Mathf.RoundToInt(damage * increase);
        }

        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;

        Debug.Log($"Player takes {damage} damage. HP: {currentHP}/{maxHP}");

        if (currentHP == 0)
        {
            Die();
        }
    }

    // Gain block (affected by Reinforced & Feeble)
    public void GainBlock(int amount)
    {
        block += amount;
        ApplyStatusEffects();
        Debug.Log($"Player gains {amount} block. Total: {block}");
    }

    // Gain energy (affected by Warmup)
    public void GainEnergy(int amount)
    {
        if (statusEffects.ContainsKey("Warmup"))
        {
            amount /= 2;
        }
        energy += amount;
        Debug.Log($"Player gains {amount} energy. Total: {energy}");
    }

    // Check if a status effect exists
    public bool HasStatusEffect(string effect)
    {
        return statusEffects.ContainsKey(effect);
    }

    // Get the number of stacks of a status effect
    public int GetStatusEffectStacks(string effect)
    {
        return statusEffects.ContainsKey(effect) ? statusEffects[effect] : 0;
    }

    // Set or replace an artifact
    public void EquipArtifact(string artifact)
    {
        equippedArtifact = artifact;
        Debug.Log($"Player equipped {artifact}!");
    }

    // Use the equipped artifact
    public void UseArtifact()
    {
        if (equippedArtifact == null)
        {
            Debug.Log("No artifact equipped.");
            return;
        }

        Debug.Log($"Using artifact: {equippedArtifact}");

        // Example effect handling
        if (equippedArtifact == "Healing Relic")
        {
            Heal(15);
        }
        else if (equippedArtifact == "Gold Doubler")
        {
            gold *= 2;
        }

        equippedArtifact = null; // One-time use
    }

    // Heal the player
    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
        Debug.Log($"Player heals {amount} HP. Current HP: {currentHP}/{maxHP}");
    }

    // Spend gold at the shop
    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log($"Player spent {amount} gold. Remaining: {gold}");
            return true;
        }
        else
        {
            Debug.Log("Not enough gold!");
            return false;
        }
    }

    // Gain gold
    public void GainGold(int amount)
    {
        gold += amount;
        Debug.Log($"Player gained {amount} gold. Total: {gold}");
    }

    // Player death
    private void Die()
    {
        Debug.Log("Player has died!");
        // Trigger game over or reset logic here
    }
}
