using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoll : MonoBehaviour
{

    protected DiceManager diceManager;
    protected SaveLoadManager saveLoadManager;
    protected UIManager uiManager;
    protected GameObject myOnFocused;

    private void Awake()
    {
        // Find DiceManager GameObject using tag
        diceManager = GameObject.FindGameObjectWithTag("DiceManager").GetComponent<DiceManager>();

        // Find SaveLoadManager GameObject using tag
        saveLoadManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SaveLoadManager>();

        //Find UI Manager
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        //OnFocused
        myOnFocused = transform.Find("OnFocused").gameObject;
    }

}
