using Droidzone.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSlot : MonoBehaviour
{
    public KeyCode myInput;

    private void Update()
    {
        if (Input.GetKeyDown(myInput))
        {
           GameManager.Pc.transform.GetComponent<FeatsSystem>().UseFeat(gameObject.name);
        }
    }
}
