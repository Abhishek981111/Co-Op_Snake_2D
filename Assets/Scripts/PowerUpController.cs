using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public GameObject[] powerUps; // To assign power up prefab in inspector
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 20f;

    private void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            // Calculate a random interval for the next power-up spawn
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);

            // Randomly select a power-up from the array
            int randomIndex = Random.Range(0, powerUps.Length);
            GameObject powerUpPrefab = powerUps[randomIndex];

            // Spawn the selected power-up at a random position
            Vector2 spawnPosition = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        }
        
    }
}
