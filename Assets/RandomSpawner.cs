using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;

    public float amount = 10000;

    void Start()
    {
        int xPos = 0; 
        int zPos = 0;

        while(xPos < amount)
        {
            Instantiate(prefabToSpawn, new Vector3(xPos, 0.5f, zPos), Quaternion.identity);
            xPos++;
            zPos++;
        }
    }

  
}
