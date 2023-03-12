using System;
using System.Collections;
using UnityEngine;

public class CandyGenerator : MonoBehaviour
{
    /*
     * Maximum Position Candies can Spawn from [x axis].
     */
    [SerializeField]
    float MaxSpawnPos;

    /*
     * The Speed at which Candies will Spawn (Difference of Time Between spawning 2 separate candies)
     */
    [SerializeField]
    float SpawnSpeed;

    /*
     * Cantainer for all Different Candies.
     * This is used to Spawn Candies by Randomizing the Index which is referenced.
     */
    public GameObject[] Candies;

    /*
     * Static Instance for Other Classes to access and Change Data that are static or other relevant Workflow.
     */
    public static CandyGenerator candyGenerator;

    /*
     * Called right when the Game Object is initialised on screen.
     * It is called before anything else happens at the Top Most.
     */
    private void Awake()
    {
        //Initialising the static Instance here, if its not already having a value.
        if (candyGenerator == null)
        {
            candyGenerator = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartSpawningCandies(); //Calling function to start the spawning of candies.
    }

    // Update is called once per frame
    void Update()
    {
        //Nothing here.
    }


    /*
     * This Function Spawns Random Candies, At Random Position within the Range of X - Axis Spawnable region.
     */
    private void SpawnCandy()
    {
        
        int CandyIndex = UnityEngine.Random.Range(0, Candies.Length); //Getting a random Integer with the range of total number of candies and storing in CandyIndex.

        //Creating a Vector3 variable for a random position on x axis within a range specified by the MaxSpawnPos variable [y and z axis remains same.]
        Vector3 Location = new Vector3(UnityEngine.Random.Range(-MaxSpawnPos, MaxSpawnPos + 1),transform.position.y,transform.position.z); 

        //Creating the Game Object in the Scene.
        Instantiate(Candies[CandyIndex], Location, transform.rotation);
    }

    /*
     * This Function starts the CoRoutine.
     */
    public void StartSpawningCandies() 
    {
        StartCoroutine("CandySpawner"); //Starting the CoRoutine CandySpawner.
    }

    /*
     * This function is a CoRoutine, which runs Parallaly with the Update Function:
     * Calls SpawnCandy Function at a particular Interval, Infinitely to SPawn Candies.
     */
    IEnumerator CandySpawner()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            SpawnCandy();

            yield return new WaitForSeconds(SpawnSpeed);
        }
    }


    /*
     * This Function Stops the CoRoutine.
     */
    public void StopSpawningCandies() 
    {
        StopCoroutine("CandySpawner"); // Stopping the CandySpawner CoRoutine.
    }

    
}
