using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public List<GameObject> objectsToSpawn; // List of prefabs to spawn
    public float spawnInterval = 1f; // Interval between spawns
    public Vector3 spawnPosition = Vector3.zero; // Fixed spawn position

    private GameObject lastSpawnedObject = null; // Keep track of the last spawned object

    private void Start()
    {
        // Start spawning objects at regular intervals
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            // Destroy the last spawned object (if any) before proceeding
            if (lastSpawnedObject != null)
            {
                Destroy(lastSpawnedObject);
                lastSpawnedObject = null;
            }

            // Countdown before next spawn
            for (int i = 3; i > 0; i--)
            {
                Debug.Log("Next spawn in: " + i + " seconds");
                yield return new WaitForSeconds(1f);
            }

            // Spawn a random object
            SpawnRandomObject();

            // Wait for the next spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnRandomObject()
    {
        if (objectsToSpawn.Count == 0)
        {
            Debug.LogWarning("No objects to spawn. Please assign objects in the objectsToSpawn list.");
            return;
        }

        GameObject objectToSpawn;

        // Ensure no back-to-back repeats
        List<GameObject> availableObjects = new List<GameObject>(objectsToSpawn);
        if (lastSpawnedObject != null)
        {
            availableObjects.Remove(lastSpawnedObject);
        }

        if (availableObjects.Count == 0)
        {
            Debug.LogWarning("Only one object in the list. Cannot avoid repeats.");
            return;
        }

        objectToSpawn = availableObjects[Random.Range(0, availableObjects.Count)];

        // Spawn the object at the fixed position
        lastSpawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        // Log the name of the spawned object
        Debug.Log("Spawned: " + objectToSpawn.name);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the spawn point in the editor for visualization
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(spawnPosition, 0.5f);
    }
}