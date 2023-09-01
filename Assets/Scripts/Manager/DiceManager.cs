using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceMaster;
using UnityEngine.Events;

public class DiceManager : MonoBehaviour
{

    public Dice[] dice;
    public int total = 0;
    Dice currentDice;

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
        total = 0;
        if(currentDice == null)
        {
            Dice newDice = Instantiate(dice[0], roller.position + diceOffset, roller.rotation);
            currentDice = newDice;
            currentDice.onShowNumber.AddListener(RegisterNumber);
        }
        else
        {

        }
       
        PC pc = roller.GetComponent<PC>();
        
    }

    public void RegisterNumber(int number)
    {
        Debug.Log("Got " + number);

        total = number;
      
    }
}
