using MalbersAnimations.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
  
    MusicManager musicManager;

    public float detectionRadius;
    public LayerMask enemyLayer;

    //Important objects
    PC pc;

    private void Awake()
    {
        pc = FindAnyObjectByType<PC>();
        musicManager = FindAnyObjectByType<MusicManager>();
    }

   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(3);
        }

      
    }
    // Call this method to switch to the exploration state
  
   

}
