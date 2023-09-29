using Droidzone.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FeatButton : MonoBehaviour
{
    public KeyCode holdInput;
    public KeyCode myInput;

   

    // Update is called once per frame
    void Update()
    {
        //natural inputs
        if(holdInput == KeyCode.None)
        {
            if (Input.GetKeyDown(myInput))
            {
                GameManager.Pc.GetComponent<FeatsSystem>().UseFeat(gameObject.name);
            }
        }
        //ctrl inputs
        else
        {
            if(Input.GetKey(holdInput) && Input.GetKeyDown(myInput))
            {
                GameManager.Pc.GetComponent<FeatsSystem>().UseFeat(gameObject.name);
            }
        }
    }

  
}
