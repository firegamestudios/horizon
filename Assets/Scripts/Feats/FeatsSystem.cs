using MalbersAnimations.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class FeatsSystem : BaseRoll
{
    //rolling variables
    int myAttribute;
    private bool rolling = false;
    private int DC;
    [Serializable] public class MyEvent : UnityEvent { } // UnityEvent with no arguments
    [Header("Roll Events")]
    public MyEvent OnSuccessEvent; // Event for success
    public MyEvent OnFailEvent;    // Event for failure

    string currentRolledFeat;

    //prefabs
    GameObject acidVialPrefab;
    GameObject bacteriaVialPrefab;

    private void Start()
    {
        acidVialPrefab = Resources.Load<GameObject>("Weapons/Acid Vial");
        bacteriaVialPrefab = Resources.Load<GameObject>("Weapons/Bacterial Fever");
    }

    public void UseFeat(string _feat)
    {
        //print("UseFeat(): " + _feat);

        currentRolledFeat = _feat;

        switch (_feat)
        {
            case "Acid Vial":
                OnCraftingRolled(gameObject, _feat);
                break;
            case "Acidthrower":

                break;
            case "Autoturret":

                break;
            case "Bacterial Fever":
                OnCraftingRolled(gameObject, _feat);
                break;
            case "Build Drone":

                break;
            case "Disable Drone":

                break;
            case "Disable System":

                break;
            case "Electrical Mine":
                break;
            case "EMP Attack":

                break;
            case "Eye Bacteria":

                break;
            case "Fire Ammo":

                break;
            case "Flaming Arrow":

                break;
            case "Glide":

                break;
            case "Grease Trap":

                break;
            case "Hack Lock":

                break;
            case "Hack Turret":

                break;
            case "Holosphere":

                break;
            case "Insulation":

                break;
            case "Nanocontrol":

                break;
            case "Nitrotrap":

                break;
            case "Organic Transmutation":

                break;
            case "Poison Ammo":

                break;
            case "Poison Arrow":

                break;
            case "Poison Bottle":

                break;
            case "Poison Shot":

                break;
            case "Regeneration":

                break;
            case "Repair System":

                break;
            case "Reprogram Droid":

                break;
            case "Tripwire":

                break;
            case "Viral Charge":

                break;
            case "Wolf Trap":

                break;


        }
    }
   


    //Roll ability
    public void OnCraftingRolled(GameObject pc, string _feat)
    {
        print("OnCraftingRolled: " +  _feat);
        myAttribute = saveLoadManager.playerData.attributes[3];
        DC = 20 - myAttribute;
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
        string results = $"Test Crafting: {DC}\nRolled: {diceManager.total}\n{resultText}";
        uiManager.diceResultText.text = results;
        uiManager.resultPanel.enabled = true;

        //Here we give the players the result of the roll, successful or failed
        if(resultText == "SUCCESS!")
        {
            switch(currentRolledFeat)
            {
                case "Acid Vial":
                    uiManager.AutoMessage("You successfully crafted some acid vials!");
                    GameObject acidVialInstance = Instantiate(acidVialPrefab, transform.position + new Vector3(0,1f,-1f), Quaternion.identity);
                    acidVialInstance.GetComponent<Pickable>().enabled = true;
                    acidVialInstance.GetComponent<BoxCollider>().enabled = true;
                    break;
                case "Bacterial Fever":
                    uiManager.AutoMessage("You successfully crafted some vials with bacteria!");
                    GameObject bacteriaVialInstance = Instantiate(bacteriaVialPrefab, transform.position + new Vector3(0, 1f, -1f), Quaternion.identity);
                    bacteriaVialInstance.GetComponent<Pickable>().enabled = true;
                    bacteriaVialInstance.GetComponent<BoxCollider>().enabled = true;
                    break;
            }
        }
        else
        {
            switch (currentRolledFeat)
            {
                case "Acid Vial":
                    uiManager.AutoMessage("You failed to craft some acid vials...");
                    break;
                case "Bacterial Fever":
                    uiManager.AutoMessage("You failed to craft bacteria...");
                    break;
            }
        }

        StartCoroutine(ClearResults());
    }

    private IEnumerator ClearResults()
    {
        yield return new WaitForSeconds(10f);
        uiManager.diceResultText.text = "";
        diceManager.total = 0;
        uiManager.resultPanel.enabled = false;
        if(!isFeatsSystem)
        {
            myOnFocused.SetActive(false);
        }
        
    }

}
