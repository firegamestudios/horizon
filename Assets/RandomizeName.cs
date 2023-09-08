using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeName : MonoBehaviour
{
    [SerializeField] private string namePrefix = "Object_"; // Prefix for the name
    [SerializeField] private int minRandomNumber = 1000;   // Minimum random number
    [SerializeField] private int maxRandomNumber = 9999;   // Maximum random number

    private void Start()
    {
        // Generate a random number within the specified range
        int randomNum = Random.Range(minRandomNumber, maxRandomNumber + 1);

        // Combine the prefix and the random number to create a unique name
        string uniqueName = namePrefix + randomNum;

        // Set the unique name to the GameObject
        gameObject.name = uniqueName;
    }
}
