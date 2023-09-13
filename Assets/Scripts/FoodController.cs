using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public float foodSpawnInterval;
    public float foodLifeTime = 10f;
    public GameObject growthFoodPrefab;
    public GameObject decreaseFoodPrefab;

    public float xFoodSpawnRangeNegative;
    public float xFoodSpawnRangePositive;
    public float yFoodSpawnRangeNegative;
    public float yFoodSpawnRangePositive;

    public SnakeController snakeController;

    private void Start()
    {
        // Food spawning start
        StartCoroutine(SpawnFoodRoutine());
    }

    private IEnumerator SpawnFoodRoutine()
    {
        while (true)
        {
            // To just adjust spawn position range
            Vector2 spawnPosition = new Vector2(Random.Range(xFoodSpawnRangeNegative, xFoodSpawnRangePositive), Random.Range(yFoodSpawnRangeNegative, yFoodSpawnRangePositive));
            GameObject foodPrefab = Random.value < 0.5f ? growthFoodPrefab : decreaseFoodPrefab;

            GameObject food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);

            // Set the lifetime of the food item
            Destroy(food, foodLifeTime);

            // Wait for the next food spawn interval
            yield return new WaitForSeconds(foodSpawnInterval);
        }
    }
}
