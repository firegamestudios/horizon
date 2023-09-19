using System;
using System.Collections;
using UnityEngine;
using DiceMaster;
using TMPro;
using UnityEngine.Events;

public class RollAgility : BaseRoll
{
    [SerializeField] private int difficulty;
   

    private bool rolling = false;
    private int DC;

   
    
    [Serializable] public class MyEvent : UnityEvent { } // UnityEvent with no arguments
    [Header("Roll Events")]
    public MyEvent OnSuccessEvent; // Event for success
    public MyEvent OnFailEvent;    // Event for failure

   

    public void OnAgilityRolled(GameObject pc)
    {
        difficulty = saveLoadManager.playerData.attributes[1];
        DC = 20 - difficulty;
        rolling = true;
        diceManager.Roll(pc.transform);
    }

    private void Update()
    {
        if (rolling && diceManager.total != 0)
        {
            rolling = false;

            if (diceManager.total >= DC)
            {
                HandleTestResult("SUCCESS!");
                OnSuccessEvent?.Invoke(); // Invoke the OnSuccess event
            }
            else
            {
                HandleTestResult("FAILED...");
                OnFailEvent?.Invoke(); // Invoke the OnFail event
            }
        }
    }

    private void HandleTestResult(string resultText)
    {
        string results = $"Test Agility: {DC}\nRolled: {diceManager.total}\n{resultText}";
        uiManager.diceResultText.text = results;
        uiManager.resultPanel.enabled = true;
        StartCoroutine(ClearResults());
    }

    private IEnumerator ClearResults()
    {
        yield return new WaitForSeconds(10f);
        uiManager.diceResultText.text = "";
        diceManager.total = 0;
        uiManager.resultPanel.enabled = false;
        myOnFocused.SetActive(false);
    }


}
