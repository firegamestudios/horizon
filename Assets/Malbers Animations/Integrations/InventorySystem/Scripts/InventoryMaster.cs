using MalbersAnimations.Scriptables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MalbersAnimations.InventorySystem
{
    [AddComponentMenu("Malbers/Inventory/Inventory Master")]
    public class InventoryMaster : MonoBehaviour
    {
        /// <summary> List of all the Inventories </summary>
        public List<Inventory> ListOfInventories;

        //List of Registered Items
        public List<Item> RegisteredItems;

        public InventorySlot currentSelectedSlot;
        public bool isOpen;

        /// <summary> Should we lock the character input when the inventory is open? </summary>
        public bool lockCharacter = false;

        [FormerlySerializedAs("characterToLockInputFor")]
        [Tooltip("Who is using the inventory")]
        public TransformReference character = new();

        /// <summary>  The Controller that can be set to Lock Movement and Inputs (Logic will still works) </summary>
        ILockCharacter controller;

      //  public bool usingMalbersInventory = true;

        [Tooltip("Use the Notification System when adding Items to the Inventory")]
        public bool notifications = true;

        //Buttons for Inventory Slots

        
        [Tooltip("Inventory UI Use Button Reference")]
        public Button useButton;
        [Tooltip("Inventory UI Drop Button Reference")]
        public Button dropButton;
        [Tooltip("Inventory UI Equip Button Reference")]
        public Button equipButton;
        [Tooltip("Inventory UI Remove Button Reference")]
        public Button removeButton;
        [Tooltip("Inventory UI Unequip Button Reference")]
        public Button unequipButton;


        // The inventoryPanel is the parent object
        public GameObject inventoryPanel;
        //The ButtonsPanel
        public GameObject buttonsPanel;

        //The Info panel
        public GameObject infoPanel;
     

        [Tooltip( "Enables the  save system using encryption to the save the current Inventory state")]
        public bool EncryptionEnabled;

        #region HiddenVariables
        //Hidden Variables
        [HideInInspector, SerializeField] private int Editor_Tabs1;
        private IDataService DataService = new JSONDataService();
        #endregion

        private void Start()
        {
            currentSelectedSlot = null;
            SetUpSlots();
          //  SetUpButtons();

            controller = character.Value.GetComponent<ILockCharacter>(); //Store the character Locking methods

        }

        //public void SetUpButtons()
        //{
        //    List<Button> buttons = new()
        //    {
        //        useButton,
        //        removeButton,
        //        dropButton,
        //        equipButton,
        //        unequipButton
        //    };

        //    foreach (Button button in buttons)
        //    {
        //        if (button.TryGetComponent<ItemButtonReaction>(out var tempbutton))
        //        {
        //            if (tempbutton.inventoryMaster == null)
        //            {
        //                tempbutton.inventoryMaster = this;
        //            }
        //        }
        //    }
        //}


        public void SetUpSlots()
        {
            foreach (Inventory inv in ListOfInventories)
            {
                foreach (Transform child in inv.transform)
                {
                    if (child.TryGetComponent<InventorySlot>(out var slot))
                    {
                        // Add the InventorySlot component to the SlotList
                        inv.slotList.Add(slot);
                        slot.inventory = inv;
                    }
                }

                for (int i = 0; i < inv.slotList.Count; i++)
                {
                    //Then we set their SlotID to the number... IMPORTANT
                    inv.slotList[i].SlotID = i;
                    //Then we add a value in the item list for the slot to NULL
                    inv.inventoryData.itemList.Add(null);
                    //Then we add a value in the quantity list for the slot to 0
                    inv.inventoryData.quantityList.Add(0);

                    foreach (InventorySlot slot in inv.slotList)
                    {
                        slot.quantity.enabled = false;
                    }
                }
            }


            //Set the Inventory Master on each inventory Set
            foreach (Inventory inventory in ListOfInventories)
            {
                if (inventory.inventoryMaster == null)
                {
                    inventory.inventoryMaster = this;
                }
            }
        }


        /// <summary> Open Close the Inventory given a bool value (Malbers) </summary>
        public virtual void OpenClose(bool open)
        {
            if (open) OpenInventory(); else CloseInventory();  
        }


        public void OpenInventory()
        {
            inventoryPanel.SetActive(true);
            buttonsPanel.SetActive(true);
            isOpen = true;

            //Debug.Log("Button Hit");

            if (lockCharacter)
            {
                controller?.Lock(true);
            }
        }

        public void CloseInventory()
        {
            inventoryPanel.SetActive(false);
            buttonsPanel.SetActive(false);
            infoPanel.SetActive(false);
            isOpen = false;


            if (lockCharacter)
            {
                
                controller?.Lock(false);
            }
        }


        public void AddItemToInventory(Item item, int qty = 1, int slotNumber = -1, bool usingSaveLoadSystem = false)
        {
            //Find which inventory to add it to.
            //First get the itemtype and store in a temp variable.
            var itemType = item.type;

            ////Next find the inventory that has that itemtype: (New CODE)
            var Inventory_By_ID = ListOfInventories.Find(x => x.itemType == item.type); //Find the Inventory by Item Type

            if (Inventory_By_ID != null)
            {
                print("Inventory_By_ID: " + Inventory_By_ID.inventoryName);
                Inventory_By_ID?.AddItem(item, qty, slotNumber, usingSaveLoadSystem);
            }
            else
            {
                Debug.LogError("Inventory_By_ID is NULL");
            }


            #region Old Code (Edited by Malbers)
            //foreach (Inventory inventory in ListOfInventories)
            //{
            //    if (inventory.itemType == tempItem)
            //    {
            //        if (usingSaveLoadSystem == true)
            //        {
            //            inventory.AddItem(item, qty, slotNumber, true);
            //        }
            //        else
            //        {
            //            inventory.AddItem(item, qty);
            //            return;
            //        }
            //    }
            //}
            #endregion
        }

        public void RemoveItemFromInventory(Item item, int qty = 1)
        {
            var itemType = item.type;

            //Next find the inventory that has that itemtype: (New CODE)
            var Inventory_By_ID = ListOfInventories.Find(x => x.itemType == itemType); //Find the Inventory by Item Type
            Inventory_By_ID?.RemoveItem(item, qty);

            //Old Code
            //foreach (Inventory inventory in ListOfInventories)
            //{
            //    if (inventory.itemType == tempItem)
            //    {
            //        inventory.RemoveItem(item, qty);
            //    }
            //}
        }

        public void SerializeJSON()
        {
            if (DataService.SaveData("/playerInventories.json", ListOfInventories, EncryptionEnabled))
            {
                Debug.Log("Saved Data Correctly");
            }
            else
            {
                Debug.LogError("Could not save file!");
            }
        }

        public void DeserializeJSON()
        {
            List<Inventory> inventories = DataService.LoadData<List<Inventory>>("/playerInventories.json", EncryptionEnabled);

            //ListOfInventories = inventories;

            foreach (Inventory inv in inventories)
            {
                //Check Registered Item List against inv.inventorydata.itemlist.ID
                //If null return
                //Check Quantity of item

                //If Valid, AddItemToInventory(Item, Qty);

                for (int i = 0; i < inv.inventoryData.itemList.Count; i++)
                {
                    Item registeredItem = null;
                    int quantityOfItem = 0;
                    int slotPositionToAdd = 0;

                    foreach (Item item in RegisteredItems)
                    {
                        if (inv.inventoryData.itemList[i] != null)
                        {
                            if (item.ID == inv.inventoryData.itemList[i].ID)
                            {
                                registeredItem = item;
                                quantityOfItem = inv.inventoryData.quantityList[i];
                                slotPositionToAdd = i;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (registeredItem != null)
                    {
                        AddItemToInventory(registeredItem, quantityOfItem, slotPositionToAdd, true);
                    }
                }
            }
        }
    }
}