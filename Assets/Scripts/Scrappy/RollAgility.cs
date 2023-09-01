using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceMaster;
using TMPro;
using MalbersAnimations.UI;
using System.Xml.Schema;
using UnityEngine.UI;

public class RollAgility : MonoBehaviour
{
    DiceManager diceManager;

    public int DC;
    int agility;

    SaveLoadManager saveLoadManager;

    bool rolling = false;

    Transform diceResultObj;
    TMP_Text diceResultText;
    UIFollowTransform uiFollowTransform;
    Image resultPanel;

    private void Awake()
    {
        diceManager = FindAnyObjectByType<DiceManager>();
        saveLoadManager = FindAnyObjectByType<SaveLoadManager>();
        diceResultObj = GameObject.Find("Dice Result").transform;
        diceResultText = diceResultObj.GetComponentInChildren<TMP_Text>();
        resultPanel = diceResultObj.GetComponent<Image>();
        uiFollowTransform = diceResultObj.GetComponent<UIFollowTransform>();
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
                    UpdateUI("SUCCESS!");
                }
                else
                {
                    print("FAILED AGILITY TEST");
                    UpdateUI("FAILED...");
                }

                
            }
        }
    }

    void UpdateUI(string r)
    {
        string results = "Test Agility: " + DC.ToString() + "\n" + "Rolled: " + diceManager.total.ToString() + "\n" + r;
        diceResultText.text = results;
        uiFollowTransform.WorldTransform = transform;
        resultPanel.enabled = true;
        StartCoroutine("ClearResults");
    }

    IEnumerator ClearResults()
    {
        yield return new WaitForSeconds(10f);
        diceResultText.text = "";
        diceManager.total = 0;
        resultPanel.enabled = false;
    }


}
