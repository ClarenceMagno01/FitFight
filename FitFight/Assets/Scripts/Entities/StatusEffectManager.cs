using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private CharacterStats characterStats; // Works for both PlayerStats and EnemyStats

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    public void ApplyStatusEffect(string effect, int stacks)
    {
        switch (effect)
        {
            case "Burn":
                ApplyBurnEffect(stacks);
                break;
            case "Lethargy":
                characterStats.attackDamage = Mathf.RoundToInt(characterStats.baseAttackDamage * (1 - (0.25f * stacks)));
                break;
            case "Exposed":
                float increase = 1 + (0.5f * stacks);
                characterStats.currentHP -= Mathf.RoundToInt(characterStats.currentHP * (increase - 1));
                break;
            case "Pumped":
                characterStats.attackDamage += stacks;
                break;
            case "Reinforced":
                characterStats.block += stacks;
                break;
            case "Feeble":
                characterStats.block = Mathf.RoundToInt(characterStats.block * (1 - (0.25f * stacks)));
                break;
            case "Warmup":
                characterStats.energy = Mathf.RoundToInt(characterStats.energy / 2);
                break;
            case "Entangled":
                Debug.Log($"{characterStats.name} is Entangled and cannot attack!");
                break;
        }

        Debug.Log($"{characterStats.name} applied {effect}. Stats - Attack: {characterStats.attackDamage}, Block: {characterStats.block}");
    }

    public void RemoveStatusEffect(string effect, int stacks)
    {
        Debug.Log($"{characterStats.name} loses {stacks} stacks of {effect}.");
    }

    private void ApplyBurnEffect(int stacks)
    {
        int damage = 2 * stacks;
        characterStats.TakeDamage(damage);
        characterStats.RemoveStatusEffect("Burn", 1);
        Debug.Log($"Burn deals {damage} damage to {characterStats.name}. Remaining Burn stacks: {characterStats.GetStatusEffectStacks("Burn")}");
    }
}
