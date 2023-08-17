using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public float foodSpawnInterval;
    public GameObject growthFoodPrefab;
    public GameObject decreaseFoodPrefab;

    public float xFoodSpawnRangeNegative;
    public float xFoodSpawnRangePositive;
    public float yFoodSpawnRangeNegative;
    public float yFoodSpawnRangePositive;

    public SnakeController snakeController;
    void Start()
    {
        //Start spawning food
        InvokeRepeating("SpawnFood", foodSpawnInterval, foodSpawnInterval);
    }

    private void SpawnFood()
    {
        //Adjust spwan position range
        Vector2 spawnPosition = new Vector2(Random.Range(xFoodSpawnRangeNegative, xFoodSpawnRangePositive), Random.Range(yFoodSpawnRangeNegative, yFoodSpawnRangePositive));
        GameObject foodPrefab = Random.value < 0.5f ? growthFoodPrefab : decreaseFoodPrefab;

        // if(foodPrefab = decreaseFoodPrefab)
        // {
        //     foodPrefab = growthFoodPrefab;  //Ensure that snake length can't go too low
        // }

        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }
}
