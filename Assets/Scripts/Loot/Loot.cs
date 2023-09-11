using MoreMountains.InventoryEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
   public List<InventoryItem> Items = new List<InventoryItem>();

    Inventory inv;

    public int creditsMin;
    public int creditsMax;

    //Player
    PC pc;
    Inventory pcInv;
    
    //UI
    UIManager uiManager;

    AudioSource source;
    private void Awake()
    {
        inv = GetComponent<Inventory>();
        pc = FindAnyObjectByType<PC>();
        source = transform.parent.GetComponent<AudioSource>();
        uiManager = FindAnyObjectByType<UIManager>();
    }
    void Start()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].ItemName == "Credit")
            {
                int amount = Random.Range(creditsMin, creditsMax);
                GiveCreditsAmount(amount, i);
            }
            else
            {
                inv.AddItem(Items[i], 1);
            }
            
        }

        pcInv = pc.inv;
    }

    void GiveCreditsAmount(int amount, int index)
    {
        for (int i = 0; i < amount; i++)
        {
            inv.AddItem(Items[index], amount);
        }
    }

    public void TakeMyLoot()
    {
        for (int i = 0; i < inv.Content.Length; i++)
        {
            if (inv.Content[i] != null)
            {
                pcInv.AddItem(inv.Content[i], inv.Content[i].Quantity);
                inv.RemoveItemByID(inv.Content[i].ItemID, inv.Content[i].Quantity);
            }
          
        }
        source.Play();
        uiManager.UpdateInventoryDisplay();
        print("Take All loot ()");
    }

 
}
