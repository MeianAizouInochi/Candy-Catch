using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    private float TimeToWait;

    // Start is called before the first frame update
    void Start()
    {
        StartSpawningCandy();
    }

    IEnumerator SpawnCandies() 
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            SpawnCandy();

            yield return new WaitForSeconds(TimeToWait);
        }
    }

    private void SpawnCandy()
    {
        //Procedure to SpawnCandy.
    }

    public void StartSpawningCandy()
    {
        StartCoroutine("SpawnCandies");
    }
}
