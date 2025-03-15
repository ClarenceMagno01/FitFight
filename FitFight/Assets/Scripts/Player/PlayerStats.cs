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
    private List<string> equippedArtifacts = new List<string>(); // Multiple artifacts

    public TurnManager turnManager; // Reference to Turn Manager

    void Start()
    {
        currentHP = maxHP;
        currentMP = maxMP;

        // Auto-assign TurnManager if not manually assigned
        if (turnManager == null)
        {
            turnManager = FindObjectOfType<TurnManager>();
        }

        ResetStats();
    }

    // Reset stats before applying status effects
    private void ResetStats()
    {
        attackDamage = baseAttackDamage;
        block = baseBlock;
    }

    // Apply Burn damage at the start of the player's turn
    private void ApplyBurnDamage()
    {
        if (HasStatusEffect("Burn"))
        {
            int burnStacks = GetStatusEffectStacks("Burn");
            TakeDamage(2 * burnStacks); // Each Burn stack deals 2 damage
            RemoveStatusEffect("Burn", 1);
            Debug.Log($"Burn deals {2 * burnStacks} damage. Remaining Burn stacks: {GetStatusEffectStacks("Burn")}");
        }
    }

    // Called at the start of the player's turn
    public void StartTurn()
    {
        ApplyBurnDamage();
    }

    // Apply a status effect (stackable)
    public void ApplyStatusEffect(string effect, int stacks = 1)
    {
        if (HasStatusEffect("Immune"))
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
        ResetStats();

        if (HasStatusEffect("Lethargy"))
        {
            attackDamage = Mathf.RoundToInt(baseAttackDamage * (1 - (0.25f * GetStatusEffectStacks("Lethargy"))));
        }

        if (HasStatusEffect("Exposed"))
        {
            float increase = 1 + (0.5f * GetStatusEffectStacks("Exposed"));
            currentHP -= Mathf.RoundToInt(currentHP * (increase - 1));
        }

        if (HasStatusEffect("Pumped"))
        {
            attackDamage += GetStatusEffectStacks("Pumped");
        }

        if (HasStatusEffect("Reinforced"))
        {
            block += GetStatusEffectStacks("Reinforced");
        }

        if (HasStatusEffect("Feeble"))
        {
            block = Mathf.RoundToInt(block * (1 - (0.25f * GetStatusEffectStacks("Feeble"))));
        }

        if (HasStatusEffect("Warmup"))
        {
            energy = Mathf.RoundToInt(energy / 2);
        }

        Debug.Log($"Updated Stats - Attack: {attackDamage}, Block: {block}, HP: {currentHP}/{maxHP}");
    }

    // Prevent Attacks if Entangled
    public bool CanAttack()
    {
        if (HasStatusEffect("Entangled"))
        {
            Debug.Log("You are Entangled and cannot attack!");
            return false;
        }
        return true;
    }

    // Take damage (affected by Exposed)
    public void TakeDamage(int damage)
    {
        if (HasStatusEffect("Exposed"))
        {
            damage = Mathf.RoundToInt(damage * (1 + (0.5f * GetStatusEffectStacks("Exposed"))));
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
        if (HasStatusEffect("Warmup"))
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

    // Equip an artifact (now supports multiple artifacts)
    public void EquipArtifact(string artifact)
    {
        if (!equippedArtifacts.Contains(artifact))
        {
            equippedArtifacts.Add(artifact);
            Debug.Log($"Player equipped {artifact}!");
        }
        else
        {
            Debug.Log($"Player already has {artifact} equipped.");
        }
    }

    // Use an artifact (each artifact has its own effect)
    public void UseArtifact(string artifact)
    {
        if (!equippedArtifacts.Contains(artifact))
        {
            Debug.Log($"Player does not have {artifact} equipped.");
            return;
        }

        Debug.Log($"Using artifact: {artifact}");

        // Example artifact effects
        if (artifact == "Healing Relic")
        {
            Heal(15);
        }
        else if (artifact == "Gold Doubler")
        {
            gold *= 2;
        }

        equippedArtifacts.Remove(artifact); // Remove after use
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
        Debug.Log("Player has died! Game Over.");
        GameOver();
    }

    // Game Over Logic
    private void GameOver()
    {
        Debug.Log("Game Over! Restart or Exit.");
    }

    // End the player's turn and switch to enemy turn
    public void EndTurn()
    {
        Debug.Log("Player ends turn.");
        turnManager.EndPlayerTurn();
    }
}
