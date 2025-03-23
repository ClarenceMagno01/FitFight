using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;
    public string description;
    public bool isUpgraded = false;

    public abstract void PlayCard(PlayerStats player, EnemyStats target);
    public abstract void UpgradeCard();
}
