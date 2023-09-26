using MoreMountains.InventoryEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FeatsSystem : BaseRoll
{
    //PC
    Inventory inv;

    Vector3 offset = new Vector3 (0, 1.2f, 0);

    GameObject dronePrefab;

    public List<GameObject> droneInstances = new();

    //Rolling system
    [Serializable] public class MyEvent : UnityEvent { } // UnityEvent with no arguments
    [Header("Roll Events")]
    public MyEvent OnSuccessEvent; // Event for success
    public MyEvent OnFailEvent;    // Event for failure
    private bool rolling = false;
    string currentAttributeRolled;
    string currentFeatTested;
    private int DC;
    private int difficulty; //don't forget to set the difficulty
    

    private void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        dronePrefab = Resources.Load<GameObject>("Crafts/Drone");
       
    }


    private void Update()
    {
        if (rolling && diceManager.total != 0)
        {
            rolling = false;

            if (diceManager.total >= DC)
            {
                HandleTestResult("SUCCESS!", currentAttributeRolled, currentFeatTested);
                OnSuccessEvent?.Invoke(); // Invoke the OnSuccess event
            }
            else
            {
                HandleTestResult("FAILED...", currentAttributeRolled, currentFeatTested);
                OnFailEvent?.Invoke(); // Invoke the OnFail event
            }
        }
    }
    public void UseFeat(string _feat)
    {
        

        switch (_feat)
        {
            case "Build Drone":
                BuildDrone();
                break;
            default:
                break;
        }
    }

    //Feats methods
    void BuildDrone()
    {
        //first check for items (scrap and electronic parts)
        inv = GetComponent<PC>().inv;
        saveLoadManager = FindAnyObjectByType<SaveLoadManager>();   
        difficulty = 4;

        if(inv.GetQuantity("Scraps")  > 0 && inv.GetQuantity("Electronic Parts") > 0)
        {
            uiManager.AutoMessage("Building a drone...");
            currentAttributeRolled = "Crafting";
            currentFeatTested = "Build Drone";
            OnCraftingRolled(gameObject);
        }
        else
        {
            uiManager.AutoMessage("We need both Scraps and Electronic Parts to build a drone...");
        }
    }

    public void OnCraftingRolled(GameObject pc)
    {
        difficulty = saveLoadManager.playerData.attributes[3];
        DC = 20 - difficulty;
        rolling = true;
        diceManager.Roll(pc.transform);
    }

    void OnBuildDrone()
    {
        droneInstances.Add(Instantiate(dronePrefab, transform.position, transform.rotation));
        inv.RemoveItemByID("Scraps", 1);
        inv.RemoveItemByID("Electronic Parts", 1);
    }

    private void HandleTestResult(string resultText, string attributeName, string currentFeat)
    {
        string results = $"Test {attributeName}: {DC}\nRolled: {diceManager.total}\n{resultText}";
        uiManager.diceResultText.text = results;

        //Check effects on success
        if(resultText == "SUCCESS!")
        {
            if (currentAttributeRolled == "Crafting" && currentFeatTested == "Build Drone")
            {
                OnBuildDrone();
            }
        }
      

        uiManager.resultPanel.enabled = true;
        StartCoroutine(ClearResults());
    }

    private IEnumerator ClearResults()
    {
        yield return new WaitForSeconds(10f);
        uiManager.diceResultText.text = "";
        diceManager.total = 0;
        uiManager.resultPanel.enabled = false;
        if(!isFeatsSystem)
        myOnFocused.SetActive(false);
    }
}
