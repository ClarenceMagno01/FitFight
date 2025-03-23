using UnityEngine;

public class PlayerStats : CharacterStats
{
    public int maxMP = 50;
    public int currentMP;
    public int gold = 100;

    protected override void Start()
    {
        base.Start();
        currentMP = maxMP;
    }

    public void GainGold(int amount)
    {
        gold += amount;
        Debug.Log($"Player gained {amount} gold. Total: {gold}");
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log($"Player spent {amount} gold. Remaining: {gold}");
            return true;
        }
        Debug.Log("Not enough gold!");
        return false;
    }

    protected override void Die()
    {
        Debug.Log("Player has died! Game Over.");
    }
}
