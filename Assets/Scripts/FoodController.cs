using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public float foodSpawnInterval;
    public float foodLifeTime;
    public int minSnakeSize; 
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
            //Checking if snake size is below the minimum threshold
            if(snakeController.GetSnakeSize() < minSnakeSize)
            {
                //Spawn only Growth Food
                SpawnFood(growthFoodPrefab);
            }
            else
            {
                //Randomly select a food type
                GameObject foodPrefab = Random.value < 0.5f ? growthFoodPrefab : decreaseFoodPrefab;

                //Spawn selected food type
                SpawnFood(foodPrefab);
            }
            
            // Wait for the next food spawn interval
            yield return new WaitForSeconds(foodSpawnInterval);

        }
    }

    private void SpawnFood(GameObject foodPrefab)
    {
        // To just adjust spawn position range
        Vector2 spawnPosition = new Vector2(Random.Range(xFoodSpawnRangeNegative, xFoodSpawnRangePositive), Random.Range(yFoodSpawnRangeNegative, yFoodSpawnRangePositive));

        GameObject food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);

        // Set the lifetime of the food item
        Destroy(food, foodLifeTime);
    }
}
