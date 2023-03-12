using System;
using System.Collections;
using UnityEngine;

public class CandyGenerator : MonoBehaviour
{
    [SerializeField]
    float MaxSpawnPos;

    [SerializeField]
    float SpawnSpeed;

    public GameObject[] Candies;



    // Start is called before the first frame update
    void Start()
    {
        StartSpawningCandies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /*
     * This Function Spawns Random Candies, At Random Position within the Range of X - Axis Spawnable region.
     */
    private void SpawnCandy()
    {

        int CandyIndex = UnityEngine.Random.Range(0, Candies.Length);

        Vector3 Location = new Vector3(UnityEngine.Random.Range(-MaxSpawnPos, MaxSpawnPos + 1),transform.position.y,transform.position.z);

        Instantiate(Candies[CandyIndex], Location, transform.rotation);
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
     * This Function starts the CoRoutine.
     */
    public void StartSpawningCandies() 
    {
        StartCoroutine("CandySpawner");
    }


    /*
     * This Function Stops the CoRoutine.
     */
    public void StopSpawningCandies() 
    {
        StopCoroutine("CandySpawner");
    }

    
}
