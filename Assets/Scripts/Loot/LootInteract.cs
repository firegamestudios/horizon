using MoreMountains.InventoryEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInteract : MonoBehaviour
{
    GameObject myLoot;
    InventoryDisplay invDisplay;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        myLoot = transform.GetChild(0).gameObject;
        invDisplay = GameObject.Find("LootInventoryDisplay").GetComponent<InventoryDisplay>();
        canvasGroup = GameObject.Find("LootCanvasGroup").GetComponent<CanvasGroup>();
    }

    public void OpenMyLoot()
    {
        string lootName = myLoot.name;
        invDisplay.TargetInventoryName = myLoot.name;
        invDisplay.TargetInventory = transform.GetChild(0).GetComponent<Inventory>();
        invDisplay.SetupInventoryDisplay();
        invDisplay.AllowMovingObjectsToThisInventory = true;
        invDisplay.RedrawInventoryDisplay();
        canvasGroup.alpha = 1;
        
        print("OpenMyLoot()");
    }

    public void CloseMyLoot()
    {
        canvasGroup.alpha = 0;
    }

   
}
