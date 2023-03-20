using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    /*
     * Time difference between spawn of 2 candies.
     */
    [SerializeField]
    private float TimeToWait;

    /*
     * Maximum position on X axis for spawning of Candy.
     */
    [SerializeField]
    float MaxCandySpawnPosX;

    /*
     * Array for Loading candies.
     * Need to change this before deployment.
     */
    [SerializeField]
    public GameObject[] Candies;

    /*
     * Static reference to the instance of this class.
     * To keep it alive throughout the LifeCycle of the game.
     */
    public static CandySpawner candySpawner;
    
    /*
     * THis variable is used to introduce new candies in game at runtime w.r.t score.
     */
    public int MaxExposedCandyIndex;


    public float InitialCandySpawnTime = 2.0f; // Initial First Candy Spawn Time taken.

    private void Awake()
    {
        candySpawner = this;

        MaxExposedCandyIndex = 5;

        InitialCandySpawnTime = 2.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartSpawningCandy();
    }
    
    /*
     * Coroutine to spawn candies continously.
     */
    IEnumerator SpawnCandies() 
    {
        yield return new WaitForSeconds(InitialCandySpawnTime);

        while (true)
        {
            SpawnCandy(); 

            yield return new WaitForSeconds(TimeToWait);
        }
    }

    /*
     * Function to start the above coroutine
     */
    public void StartSpawningCandy()
    {
        StartCoroutine("SpawnCandies");
    }

    /*
     * Function to stop the coroutine.
     */
    public void StopSpawningCandy()
    {
        StopCoroutine("SpawnCandies");
    }

    /*
     * Function that Instantiates the candy at random location on x axis.
     */
    private void SpawnCandy()
    {
        //Procedure to SpawnCandy.

        float RandomCandySpawnPosX = UnityEngine.Random.Range(-MaxCandySpawnPosX,MaxCandySpawnPosX);

        int RandomCandyArrayIndex = UnityEngine.Random.Range(0,MaxExposedCandyIndex);

        Instantiate(Candies[RandomCandyArrayIndex], new Vector3(RandomCandySpawnPosX, transform.position.y, transform.position.z), transform.rotation);


    }
}
