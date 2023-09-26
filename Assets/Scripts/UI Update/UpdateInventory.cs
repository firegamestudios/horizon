using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.InventoryEngine;

public class UpdateInventory : MonoBehaviour
{
    InventoryDisplay invDisplay;

    private void Awake()
    {
        invDisplay = GameObject.Find("RogueMainInventoryDisplay").GetComponent<InventoryDisplay>();
    }
    public void UpdateInventoryDisplay()
    {
      invDisplay.RedrawInventoryDisplay();
    }
}
