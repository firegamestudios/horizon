using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceMaster;

public class DiceManager : MonoBehaviour
{

    public Dice[] dice;
    private int total = 0;

    void Start()
    {
        foreach (var d in dice)
            d.onShowNumber.AddListener(RegisterNumber);
    }

    public void RegisterNumber(int number)
    {
        Debug.Log("Got " + number);

        total += number;
        Debug.Log("Total: " + total);
    }
}
