using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceMaster;

public class RollAgility : MonoBehaviour
{
    DiceManager diceManager;

    public int DC;
    int agility;

    SaveLoadManager saveLoadManager;

    bool rolling = false;

    private void Awake()
    {
        diceManager = FindAnyObjectByType<DiceManager>();
        saveLoadManager = FindAnyObjectByType<SaveLoadManager>();

    }

    public void OnAgilityRolled(GameObject pc)
    {
        //print(pc.name + " interacted with this");

        agility = saveLoadManager.playerData.attributes[1];
        //Start the roll and listen (in Update)
        rolling = true;
        DC = 20 - agility;
        diceManager.Roll(pc.transform);
    }

    private void Update()
    {
        if (rolling)
        {
            if(diceManager.total != 0)
            {
               // print("Reading results here");
                rolling = false;
                
                if(diceManager.total >= DC)
                {
                    print("SUCESS AGILITY TEST");
                }
                else
                {
                    print("FAILED AGILITY TEST");
                }
            }
        }
    }


}
