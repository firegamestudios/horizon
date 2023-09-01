using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceMaster;
using UnityEngine.Events;

public class DiceManager : MonoBehaviour
{

    public Dice[] dice;
    private int total = 0;

    int currentDC;
    int currentAtt;

    public Vector3 diceOffset = new Vector3 (0, 1f, 0);

    void Start()
    {
        foreach (var d in dice)
            d.onShowNumber.AddListener(RegisterNumber);
    }

    public void Roll(Transform roller)
    {
        Instantiate(dice[0], roller.position + diceOffset, roller.rotation);
        PC pc = roller.GetComponent<PC>();
        
    }

    public void RegisterNumber(int number)
    {
       // Debug.Log("Got " + number);

        total += number;
       // Debug.Log("Total: " + total);

       
    }
}
