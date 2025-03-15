using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public PlayerStats player;
    public List<EnemyStats> enemies; // List of all active enemies
    private int currentEnemyIndex = 0;
    private bool isPlayerTurn = true;

    void Start()
    {
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        isPlayerTurn = true;
        Debug.Log("Player's Turn!");
    }

    public void EndPlayerTurn()
    {
        isPlayerTurn = false;
        StartCoroutine(EnemyTurn());
    }

    public void RemoveEnemy(EnemyStats enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }

        if (enemies.Count == 0)
        {
            Debug.Log("All enemies defeated! Player wins!");
        }
    }


    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn Start!");

        for (currentEnemyIndex = 0; currentEnemyIndex < enemies.Count; currentEnemyIndex++)
        {
            if (enemies[currentEnemyIndex] != null)
            {
                yield return new WaitForSeconds(1f); // Delay for action
                enemies[currentEnemyIndex].StartTurn();
                yield return new WaitForSeconds(1f); // Delay before next enemy
            }
        }

        Debug.Log("Enemy Turn End!");
        StartPlayerTurn();
    }
}
