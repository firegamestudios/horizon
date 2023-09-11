using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;

    public float amount = 1;

    void Start()
    {
       for (int i = 0; i < amount; i++)
        {
            Instantiate(prefabToSpawn, transform.position, transform.rotation); ;
        }
       
    }

  
}
