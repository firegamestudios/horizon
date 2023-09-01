using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceMaster;

public class RollAgility : MonoBehaviour
{
    DiceManager diceManager;

    public int DC;

    SaveLoadManager saveLoadManager;

    private void Awake()
    {
        diceManager = FindAnyObjectByType<DiceManager>();
        saveLoadManager = FindAnyObjectByType<SaveLoadManager>();

        Dice d = diceManager.dice[0];
        d.onShowNumber.AddListener(RegisterNumber);
    }

    public void OnAgilityRolled(GameObject pc)
    {
        print(pc.name + " interacted with this");

        int agility = saveLoadManager.playerData.attributes[1];

        diceManager.Roll(pc.transform);
    }

    public void RegisterNumber(int number)
    {
        Debug.Log("Got " + number);

    }
}
