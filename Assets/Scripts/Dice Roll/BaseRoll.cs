using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoll : MonoBehaviour
{

    protected DiceManager diceManager;
    protected SaveLoadManager saveLoadManager;
    protected UIManager uiManager;
    protected GameObject myOnFocused;
    protected Transform resulsWP;

    public bool isFeatsSystem;

    private void Awake()
    {
        // Find DiceManager GameObject using tag
        diceManager = GameObject.FindGameObjectWithTag("DiceManager").GetComponent<DiceManager>();

        // Find SaveLoadManager GameObject using tag
        saveLoadManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SaveLoadManager>();

        //Find UI Manager
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        //OnFocused
        if(!isFeatsSystem)
        myOnFocused = transform.Find("OnFocused").gameObject;

        resulsWP = transform.Find("Results WP");
    }

}
