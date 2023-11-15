using MalbersAnimations.Events;
using MalbersAnimations.Weapons;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.InventorySystem
{
    /// <summary>  Inventory sets  </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [AddComponentMenu("Malbers/Inventory/Inventory Set")]
    public class Inventory : MonoBehaviour
    {
        public string inventoryName;
        public InventoryMaster inventoryMaster;

        public MWeapon currentEquippedWeapon;

        public ItemType itemType;
        [JsonProperty]
        public InventoryData inventoryData = new();
        // The items on the inventory
        //[JsonProperty]
        //public List<Item> itemList = new List<Item>();

        //// The correponding quantities of each item
        //[JsonProperty]
        //public List<int> quantityList = new List<int>();

        // The slotList is the list of slots on the inventory, you can turn this List public and place the slots manually inside of it
        // Currently it's making the list based on the inventoryPanel children objects on GatherSlots() in line 86

        public List<InventorySlot> slotList = new List<InventorySlot>();

        //Offset when dropping item - leave at 0 for no offset.

        public Vector3 itemDroppingOffset = new Vector3(0, 0, 0);

        //Should we use Holsters for the inventory or not?
        #region HiddenVariables
        //Hidden Variables

        [HideInInspector, SerializeField] private int Editor_Tabs1;
        #endregion

        #region Events
        [Tooltip("Invoked when the item is used E.g. Consumable")]
        public GameObjectEvent OnItemUsed = new();

        [Tooltip("Invoked when the item (Weapon) is equipped")]
        public GameObjectEvent OnItemEquipped = new();

        [Tooltip("Invoked when the item (Weapon) is unequipped")]
        public GameObjectEvent OnItemUnEquipped = new();

        [Tooltip("Invoked when the item is dropped from the slot")]
        public GameObjectEvent OnItemDropped = new();

        [Tooltip("Invoked when the item is removed from the slot")]
        public GameObjectEvent OnItemRemoved = new();
        #endregion    



        //public static Inventory instance;

        //void Awake()
        //{
        //    instance = this;
        //}


        public void Start()
        {
            // Add the slots of the Inventory Panel to the list
            #region oldCode
            //foreach (GameObject tab in TabList)
            //{
            //    foreach (Transform child in tab.transform)
            //    {
            //        // Check if the child has a component called "InventorySlot"
            //        InventorySlot slot = child.GetComponent<InventorySlot>();
            //        if (slot != null)
            //        {
            //            // Add the InventorySlot component to the SlotList
            //            slotList.Add(slot);
            //        }
            //    }
            //}

            //foreach(Transform child in this.transform)
            //{
            //    InventorySlot slot = child.GetComponent<InventorySlot>();
            //    if (slot != null)
            //    {
            //        // Add the InventorySlot component to the SlotList
            //        slotList.Add(slot);
            //    }
            //}

            //Quick check to see the status at the beginning and if it's open then we disable the animal character
            //if (inventoryPanel.activeInHierarchy == true)
            //{
            //    OpenInventory();
            //}



            //UpdateInventoryUI();

            //SetUpSlots();
            #endregion
            inventoryData.inventoryName = inventoryName;
        }

        public void SetUpSlots()
        {
            #region oldCode
            //Here we set up the slots. We do that by grabbing how many slots are in the slot list that we generate in the Start Function...
            //for (int i = 0; i < slotList.Count; i++)
            //{
            //    //Then we set their SlotID to the number... IMPORTANT
            //    slotList[i].SlotID = i;
            //    //Then we add a value in the item list for the slot to NULL
            //    itemList.Add(null);
            //    //Then we add a value in the quantity list for the slot to 0
            //    quantityList.Add(0);

            //    foreach(InventorySlot slot in slotList)
            //    {
            //        slot.quantity.enabled = false;
            //    }
            //}
            #endregion

        }

        public int randomItemIDGenerator()
        {
            int number = Random.Range(1, 999);
            return number;
        }

        public bool checkIfInventoryIsFull()
        {
            if (!inventoryData.itemList.Contains(null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if  We Are Exceeding Max Allowed Number Of Item In Stack </summary>
        /// <param name="itemToCheck"></param>
        /// <param name="Qty"></param>
        /// <returns>Returns true if  We Are Exceeding Max Allowed Number Of Item In Stack</returns>
        public bool CheckIfWeAreExceedingMaxAllowedNumberOfItemInStack(Item itemToCheck, int Qty)
        {
            if (inventoryData.itemList.Contains(itemToCheck))
            {
                int currentNumber = inventoryData.quantityList[inventoryData.itemList.IndexOf(itemToCheck)];

                // if adding the item means we go over the limit
                return currentNumber + Qty > itemToCheck.maxStacks;

                //OLD code (Edited by Malbers)

                //if (currentNumber + Qty > itemToCheck.maxStacks) // if adding the item means we go over the limit
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }

            return false;
        }

        // AddItem() can be called in other scripts with the following line:
        //Inventory.instance.Add(ItemYouWantToGiveHere , quantityOfThatItem);
        // Currently it's being called by the AddItemToInventory Script on the Add Items Buttons 
        /// <summary>
		/// Tries to add an item of the specified type
		/// </summary>
		/// <param name="SlotNumber">SlotNumber to Add to. If -1 we check for the first empty slot in the list.</param>
        public void AddItem(Item itemAdded, int quantityAdded, int SlotNumber = -1, bool usingSaveLoadSystem = false)
        {
          //  Debug.Log($"AddItem: usingSaveLoadSystem {usingSaveLoadSystem}");

            if (usingSaveLoadSystem == true)
            {
                inventoryData.itemList[SlotNumber] = itemAdded;
                inventoryData.quantityList[SlotNumber] = quantityAdded;
                slotList[SlotNumber].UpdateSlot(inventoryData.itemList[SlotNumber], inventoryData.quantityList[SlotNumber]);
                return;
            }
            //Check Item is Stackable
            if (itemAdded.Stackable)
            {
               // Debug.Log("ITEM STACkable");

                //check if item list contains the item already
                if (inventoryData.itemList.Contains(itemAdded))
                {
                    //Check if we are exceeding allowed number of items in stack
                    if (CheckIfWeAreExceedingMaxAllowedNumberOfItemInStack(itemAdded, quantityAdded) == true)
                    {
                        //if true - we then check if Inventory is full
                        if (checkIfInventoryIsFull() == true)
                        {
                            DropItem(itemAdded, 1, -1, false);
                            //Notify Player somehow
                            NotificationManager.Instance.OpenNotification("CANNOT ADD ITEM", "Maximum Number Exceeded");
                            return;
                        }
                        else
                        {
                            //If Inventory isn't full, we need to add to new slot

                            //Get the position of the null value
                            for (int y = 0; y < inventoryData.itemList.Count; y++)
                            {
                                if (inventoryData.itemList[y] == null) //if we find the first null value this means it's the first space available
                                {
                                    inventoryData.itemList[y] = itemAdded; //add the item to the list at the first null space
                                    inventoryData.quantityList[y] = quantityAdded; //add the quantity at that space too.
                                    slotList[y].UpdateSlot(inventoryData.itemList[y], inventoryData.quantityList[y]); //Update the slot where we added
                                    if (inventoryMaster.notifications)
                                    {
                                        NotificationManager.Instance.OpenNotification("ITEM ADDED", "Added: " + quantityAdded.ToString() + " x " + itemAdded.itemName);
                                    }
                                    return;
                                }
                            }
                        }

                    }

                    //update the quantity list of that item
                    inventoryData.quantityList[inventoryData.itemList.IndexOf(itemAdded)] = inventoryData.quantityList[inventoryData.itemList.IndexOf(itemAdded)] + quantityAdded;
                    //Update the Slot
                    slotList[inventoryData.itemList.IndexOf(itemAdded)].UpdateSlot(inventoryData.itemList[inventoryData.itemList.IndexOf(itemAdded)], inventoryData.quantityList[inventoryData.itemList.IndexOf(itemAdded)]);
                    if (inventoryMaster.notifications)
                    {
                        NotificationManager.Instance.OpenNotification("ITEM ADDED", "Added: " + quantityAdded.ToString() + " x " + itemAdded.itemName);
                    }

                }
                else // It doesn't contain the item
                {
                    if (SlotNumber == -1)
                    {
                        //Check to see if there is space by checking for a null value!
                        if (inventoryData.itemList.Contains(null))
                        {
                            //Get the position of the null value
                            for (int y = 0; y < inventoryData.itemList.Count; y++)
                            {
                                if (inventoryData.itemList[y] == null) //if we find the first null value this means it's the first space available
                                {

                                    inventoryData.itemList[y] = itemAdded; //add the item to the list at the first null space
                                    inventoryData.quantityList[y] = quantityAdded; //add the quantity at that space too.
                                    slotList[y].UpdateSlot(inventoryData.itemList[y], inventoryData.quantityList[y]); //Update the slot where we added
                                    if (inventoryMaster.notifications)
                                    {
                                        NotificationManager.Instance.OpenNotification
                                            ("ITEM ADDED", "Added: " + quantityAdded.ToString() + " x " + itemAdded.itemName);
                                    }
                                    return;
                                    #region oldCode
                                    ////ADD IN CHECK FOR ITEMTYPE VS SLOTTYPE
                                    //if(itemTypeVsSlotType(itemAdded, slotList[y]) != "MISMATCH")
                                    //{
                                    //    switch (itemTypeVsSlotType(itemAdded, slotList[y]))
                                    //    {
                                    //        case "General":
                                    //            itemList[y] = itemAdded; //add the item to the list at the first null space
                                    //            quantityList[y] = quantityAdded; //add the quantity at that space too.
                                    //            slotList[y].UpdateSlot(itemList[y], quantityList[y]); //Update the slot where we added
                                    //            if (usingNotificationManager)
                                    //            {
                                    //                NotificationManager.notificationManager.OpenNotification("ITEM ADDED", "Added: " + itemAdded.itemName);
                                    //            }
                                    //            return;
                                    //        case "Weapon":
                                    //            itemList[y] = itemAdded; //add the item to the list at the first null space
                                    //            quantityList[y] = quantityAdded; //add the quantity at that space too.
                                    //            slotList[y].UpdateSlot(itemList[y], quantityList[y]); //Update the slot where we added
                                    //            if (usingNotificationManager)
                                    //            {
                                    //                NotificationManager.notificationManager.OpenNotification("ITEM ADDED", "Added: " + itemAdded.itemName);
                                    //            }
                                    //            return;
                                    //        case "Armour":
                                    //            itemList[y] = itemAdded; //add the item to the list at the first null space
                                    //            quantityList[y] = quantityAdded; //add the quantity at that space too.
                                    //            slotList[y].UpdateSlot(itemList[y], quantityList[y]); //Update the slot where we added
                                    //            if (usingNotificationManager)
                                    //            {
                                    //                NotificationManager.notificationManager.OpenNotification("ITEM ADDED", "Added: " + itemAdded.itemName);
                                    //            }
                                    //            return;
                                    //        case "Resources":
                                    //            itemList[y] = itemAdded; //add the item to the list at the first null space
                                    //            quantityList[y] = quantityAdded; //add the quantity at that space too.
                                    //            slotList[y].UpdateSlot(itemList[y], quantityList[y]); //Update the slot where we added
                                    //            if (usingNotificationManager)
                                    //            {
                                    //                NotificationManager.notificationManager.OpenNotification("ITEM ADDED", "Added: " + itemAdded.itemName);
                                    //            }
                                    //            return;
                                    //        case "KeyItem":
                                    //            itemList[y] = itemAdded; //add the item to the list at the first null space
                                    //            quantityList[y] = quantityAdded; //add the quantity at that space too.
                                    //            slotList[y].UpdateSlot(itemList[y], quantityList[y]); //Update the slot where we added
                                    //            if (usingNotificationManager)
                                    //            {
                                    //                NotificationManager.notificationManager.OpenNotification("ITEM ADDED", "Added: " + itemAdded.itemName);
                                    //            }
                                    //            return;
                                    //        default:
                                    //            break;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    break;
                                    //}
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            //Notify the player somehow there isnt space
                            Debug.Log("There is currently no space to add this item");
                        }
                    }
                    else //if we are adding to specific slot
                    {
                        inventoryData.itemList[SlotNumber] = itemAdded;
                        inventoryData.quantityList[SlotNumber] = quantityAdded;
                        slotList[SlotNumber].UpdateSlot(inventoryData.itemList[SlotNumber], inventoryData.quantityList[SlotNumber]);
                        if (inventoryMaster.notifications)
                        {
                            NotificationManager.Instance.OpenNotification("ITEM ADDED", "Added: " + quantityAdded.ToString() + " x " + itemAdded.itemName);
                        }
                        return;
                    }
                }
            }
            else //Not stackable - If Not stackable, only add 1 per item...
            {
                if (SlotNumber == -1)
                {
                    //Check to see if there is a place for the item
                    if (inventoryData.itemList.Contains(null))
                    {
                        //Get the position of the null value
                        for (int y = 0; y < inventoryData.itemList.Count; y++)
                        {
                            if (inventoryData.itemList[y] == null) //if we find the first null value this means it's the first space available
                            {
                                inventoryData.itemList[y] = itemAdded; //add the item to the list at the first null space
                                inventoryData.quantityList[y] = quantityAdded; //add the quantity at that space too.
                                slotList[y].UpdateSlot(inventoryData.itemList[y], inventoryData.quantityList[y]); //Update the slot where we added
                                if (inventoryMaster.notifications)
                                {
                                    NotificationManager.Instance.OpenNotification("ITEM ADDED", "Added: " + quantityAdded.ToString() + " x " + itemAdded.itemName);
                                }
                                return;
                            }
                        }
                    }
                    else
                    {
                        //Notify the player somehow there isnt space
                        Debug.Log("There is currently no space to add this item");
                    }
                }
                else //There is a slot number specified to add to
                {
                    inventoryData.itemList[SlotNumber] = itemAdded;
                    inventoryData.quantityList[SlotNumber] = 1;
                    slotList[SlotNumber].UpdateSlot(inventoryData.itemList[SlotNumber], inventoryData.quantityList[SlotNumber]);
                    if (inventoryMaster.notifications)
                    {
                        NotificationManager.Instance.OpenNotification("ITEM ADDED", "Added: " + quantityAdded.ToString() + " x " + itemAdded.itemName);
                    }
                    return;
                } 
            }

            #region old code v2
            ////If the Item is Stackable it checks if there is already that item in the inventory and only adds the quantity

            ////Check if item is stackable or not
            //if (itemAdded.Stackable)
            //{
            //    //Then we check if the item list contains the item already
            //    if(itemList.Contains(itemAdded))
            //    {
            //        quantityList[itemList.IndexOf(itemAdded)] = quantityList[itemList.IndexOf(itemAdded)] + quantityAdded;
            //        //Call Update Slot
            //        slotList[itemList.IndexOf(itemAdded)].UpdateSlot(itemList[itemList.IndexOf(itemAdded)], quantityList[itemList.IndexOf(itemAdded)]);
            //    }
            //    else //if it doesn't contain the item already - we need to add a new item
            //    {
            //        //First we check to see if there is a space in the itemList for the new item
            //        if (itemList.Count < slotList.Count)

            //        {
            //            //Next we check for the first valid NULL spaces in the inventory if there are any.

            //            //Checking for first Valid Null space
            //            for (int y = 0; y < itemList.Count; y++)
            //            {
            //                if (itemList[y] == null) //if we find the first null value this means it's the first space available
            //                {
            //                    itemList[y] = itemAdded; //add the item to the list at the first null space
            //                    quantityList[y] = quantityAdded; //add the quantity at that space too.
            //                    //Call Update Slot here.
            //                    slotList[y].UpdateSlot(itemList[y], quantityList[y]);
            //                    return; //Once added we return out as we don't need this anymore.
            //                }
            //                else
            //                {
            //                    y++; //if we dont find one yet, increase the y value to go to next spot in the itemList.
            //                }
            //            }

            //            //If we finish the for loop without finding the null value, then we know that we can add to the next available non-null space
            //            itemList.Add(itemAdded);
            //            quantityList.Add(quantityAdded);
            //            //Call Update Slot here
            //            slotList[itemList.IndexOf(itemAdded)].UpdateSlot(itemList[itemList.IndexOf(itemAdded)], quantityList[itemList.IndexOf(itemAdded)]);

            //       }
            //        else
            //        {
            //            //Do something with Notification Manager here saying no space available.
            //        }
            //    }
            //}
            //else //If not stackable - so like a weapon or something similar...
            //{
            //    for (int i = 0; i < quantityAdded; i++)
            //    {
            //        if (itemList.Count < slotList.Count)
            //        {
            //            bool foundNullSlot = false; // Flag to track if a null slot is found

            //            for (int y = 0; y < itemList.Count; y++)
            //            {
            //                if (itemList[y] == null)
            //                {
            //                    itemList[y] = itemAdded;
            //                    quantityList[y] = 1; // Set quantity to 1 for non-stackable item
            //                    foundNullSlot = true; // Set the flag to true
            //                    //Call Update Slot here
            //                    slotList[y].UpdateSlot(itemList[y], quantityList[y]);
            //                    break; // Exit the inner loop
            //                }
            //            }

            //            if (!foundNullSlot) // No null slot found, add to the next available non-null slot
            //            {
            //                itemList.Add(itemAdded);
            //                quantityList.Add(1); // Set quantity to 1 for non-stackable item
            //                // Call Update Slot here
            //                slotList[itemList.IndexOf(itemAdded)].UpdateSlot(itemList[itemList.IndexOf(itemAdded)], quantityList[itemList.IndexOf(itemAdded)]);
            //            }


            //        }
            //        else
            //        {
            //            // Do something with Notification Manager here saying no space available
            //            break; // Exit the outer loop since there's no space available
            //        }
            //    }

            //}
            #endregion
            #region v1 attempt - old code
            //if (itemAdded.Stackable)
            //{
            //    if (itemList.Contains(itemAdded))
            //    {
            //        quantityList[itemList.IndexOf(itemAdded)] = quantityList[itemList.IndexOf(itemAdded)] + quantityAdded;
            //    }
            //    else
            //    {

            //        if (itemList.Count < slotList.Count)
            //        {
            //            if(itemList.Count != 0)
            //            {
            //                for (int y = 0; y < itemList.Count; y++)
            //                {
            //                    if (itemList[y] == null)
            //                    {
            //                        itemList[y] = itemAdded;
            //                        quantityList[y] = quantityAdded;
            //                        UpdateInventoryUI();
            //                        return; //THIS IS THE PROBLEM WHEN ADDING ITEM, THEN DROPPING, THEN PICKING BACK UP
            //                    }
            //                    else
            //                    {
            //                        y++;
            //                    }
            //                }
            //                itemList.Add(itemAdded);
            //                quantityList.Add(quantityAdded);
            //            }      
            //            else //there are no nulls and therefore we just add into first slot available
            //            {
            //                itemList.Add(itemAdded);
            //                quantityList.Add(quantityAdded);
            //            }

            //        }
            //        else { }

            //    }

            //}
            //else
            //{
            //    for (int i = 0; i < quantityAdded; i++) //For each item that is being added...
            //    {
            //        if (itemList.Count < slotList.Count) //Check that the item list has space
            //        {
            //            if(itemList.Count != 0) // if the item list doesn't equal 0 (which means something has been there before)...
            //            {

            //                for (int y = 0; y < itemList.Count; y++) //go through each item...
            //                {
            //                    if (itemList[y] == null) //check if there is a null value aka a space somewhere
            //                    {
            //                        itemList[y] = itemAdded; //add the item into that space
            //                        quantityList[y] = 1; //update the quantity of that space to 1
            //                        UpdateInventoryUI(); //Update the inventory UI
            //                        return; //return out of adding item as we've already done it.
            //                    }
            //                    else
            //                    {
            //                        y++;
            //                    }
            //                }

            //                itemList.Add(itemAdded);
            //                quantityList.Add(1);

            //            }
            //            else
            //            {
            //                itemList.Add(itemAdded);
            //                quantityList.Add(1);
            //            }
            //        }
            //        else { }

            //    }

            //}

            //// Update Inventory everytime an item is added
            //UpdateInventoryUI();
            #endregion
        }

        public int GetItemQuantity(int SlotPositionInInventory)
        {
            int qtyToReturn = 0; //initialize int

            qtyToReturn = inventoryData.quantityList[SlotPositionInInventory];

            //if (inventoryData.itemList.Contains(item))
            //{
            //    qtyToReturn = inventoryData.quantityList[inventoryData.itemList.IndexOf(item)];
            //}

            return qtyToReturn;
        }

        public void RemoveItem(Item itemRemoved, int quantityRemoved, int SlotNumber = -1)
        {
            // If the item is stackable it removes the quantity and if it's 0 or less it removes the item completely from the itemList
            if (itemRemoved.Stackable)
            {
                if (SlotNumber == -1)
                {
                    if (inventoryData.itemList.Contains(itemRemoved)) //if the ItemList contains the removed item
                    {
                        //we remove the quantity from the list
                        inventoryData.quantityList[inventoryData.itemList.IndexOf(itemRemoved)] = inventoryData.quantityList[inventoryData.itemList.IndexOf(itemRemoved)] - quantityRemoved;

                        //if the quantity is less than or equal to 0
                        if (inventoryData.quantityList[inventoryData.itemList.IndexOf(itemRemoved)] <= 0)
                        {
                            //Set the item list, quantity list to null/0. It updates the slot first so that it doesn't return null.
                            slotList[inventoryData.itemList.IndexOf(itemRemoved)].UpdateSlot(null, 0);
                            inventoryData.quantityList[inventoryData.itemList.IndexOf(itemRemoved)] = 0;
                            inventoryData.itemList[inventoryData.itemList.IndexOf(itemRemoved)] = null;
                            return;
                        }
                        else
                        {
                            slotList[inventoryData.itemList.IndexOf(itemRemoved)].UpdateSlot(itemRemoved, inventoryData.quantityList[inventoryData.itemList.IndexOf(itemRemoved)]);
                        }
                    }
                }
                else //we have specified a slot number to use
                {
                    if (inventoryData.itemList.Contains(itemRemoved)) //check that the item list actually contains the item
                    {
                        inventoryData.quantityList[SlotNumber] = inventoryData.quantityList[SlotNumber] - quantityRemoved;

                        if (inventoryData.quantityList[SlotNumber] <= 0)
                        {
                            slotList[SlotNumber].UpdateSlot(null, 0);
                            inventoryData.quantityList[SlotNumber] = 0;
                            inventoryData.itemList[SlotNumber] = null;
                            return;
                        }
                        else
                        {
                            slotList[SlotNumber].UpdateSlot(inventoryData.itemList[SlotNumber], inventoryData.quantityList[SlotNumber]);
                        }
                    }
                }
            }
            else //If not stackable.
            {
                if (SlotNumber == -1) //Means no Slot Number specified - will find first item in list and remove
                {
                    slotList[inventoryData.itemList.IndexOf(itemRemoved)].UpdateSlot(null, 0);
                    inventoryData.quantityList[inventoryData.itemList.IndexOf(itemRemoved)] = 0;
                    inventoryData.itemList[inventoryData.itemList.IndexOf(itemRemoved)] = null;
                }
                else // We have specified a specific slot to remove the item
                {
                    slotList[SlotNumber].UpdateSlot(null, 0);
                    inventoryData.quantityList[SlotNumber] = 0;
                    inventoryData.itemList[SlotNumber] = null;
                }
            }
            // Update Inventory everytime an item is removed
            //UpdateInventoryUI();
            //Find and update only the slot that the item was removed from...
        }

        public void DropItem(Item itemDropped, int quantityRemoved, int SlotNumber = -1, bool removeItem = true)
        {
            if (removeItem)
            {
                RemoveItem(itemDropped, quantityRemoved, SlotNumber);
            }

            var character = inventoryMaster.character.Value;
            Vector3 offset = character.TransformPoint(itemDroppingOffset);  //This is a faster way for doing the lines below ;) (Edited by malbers)

            //Vector3 offset = new(character.position.x + itemDroppingOffset.x,
            //    character.position.y + itemDroppingOffset.y,
            //   character.position.z + itemDroppingOffset.z);

            if (itemDropped.inWorldPrefab != null)
            {
                //GameObject droppedItem = 
                 Instantiate(itemDropped.inWorldPrefab, offset, Quaternion.identity);
                //droppedItem.GetComponent<InventoryItem>().itemQtyToGive = 1;

            }
            else
            {
                Debug.Log("No Prefab is associated on the item. Add one to drop the item successfully");
            }
            //itemDropped.DropItem(offset);
        }

        public void PickItem(Item itemToAdd, int quantityToAdd)
        {
            AddItem(itemToAdd, quantityToAdd);
        }

        public void DestroyGameObject(GameObject gameObject, float delay = 0f)
        {
            Destroy(gameObject, delay);
        }

        // --------------------------------------------------UI------------------------------------------------
        #region oldCodev0.1
        // Everytime an item is Added or Removed from the Inventory, the UpdateInventoryUI runs
        public void UpdateInventoryUI()
        {
            // This int is to count how many slots are full
            int ind = 0;

            //foreach(InventorySlot slot1 in slotList)
            //{
            //    itemList.Add(null);
            //    quantityList.Add(0);
            //}

            // For each slot in the list it's attributed an Item from the itemList and the corresponding quantity
            foreach (InventorySlot slot in slotList)
            {

                if (inventoryData.itemList.Count != 0)
                {
                    // If the ind is greater than the item quantity, the rest is considered empty slot

                    if (ind < inventoryData.itemList.Count)
                    {
                        if (inventoryData.itemList[ind] == null)
                        {
                            slot.UpdateSlot(null, 0);
                            ind++;
                        }
                        else
                        {
                            slot.UpdateSlot(inventoryData.itemList[ind], inventoryData.quantityList[ind]);
                            ind++;
                        }
                        // Calls the UpdateSlot() function on the respective slot and attributes the item and quantity of their unique index in the itemList

                    }
                    else
                    {
                        //Update Empty Slot
                        slot.UpdateSlot(null, 0);
                    }
                }
                else
                {
                    //Update Empty Slot
                    slot.UpdateSlot(null, 0);
                }

            }
        }
        #endregion
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class InventoryData
    {
        [JsonProperty]
        public string inventoryName;
        [JsonProperty]
        public List<Item> itemList = new();

        /// <summary> The correponding quantities of each item </summary>
        [JsonProperty]
        public List<int> quantityList =new();
    }
}
