using Droidzone.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holosign : MonoBehaviour
{
    UIManager uiManager;

    [TextArea(3,3)]
    public string myText;

    private void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
    }

    public void Read()
    {
        uiManager.Holosign(myText);
        GameManager.Pc.FreezePlayer();
        GameManager.Pc.FreezeCamera();
        uiManager.EnableCrosshair(false);
    }
}
