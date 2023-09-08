using MoreMountains.InventoryEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
   public List<InventoryItem> Items = new List<InventoryItem>();

    Inventory inv;

    private void Awake()
    {
        inv = GetComponent<Inventory>();
    }
    void Start()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            inv.AddItem(Items[i], 1);
        }
    }

 
}
