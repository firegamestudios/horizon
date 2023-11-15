using MalbersAnimations.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// IN THIS SCRIPT: Inventory Slot Handler that shows the player one item and it's quantity based on the Inventory Script
namespace MalbersAnimations.InventorySystem
{
    [AddComponentMenu("Malbers/Inventory/Inventory Slot")]
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        public Inventory inventory;
        //The ID Number of the slot. Starts at 0. Will be automatically be numbered.

        public int SlotID;

        //The item on the slot, if it's null the slot is considered empty

        public Item item;
        public bool selectedSlot;

        // Each slots shows the icon and quantity of that item, the following are the references to those on the UI
        public Image itemImage;

        public Text quantity;
        public Image hoverOverImage;
        public Image selectedSlotImage;
        public Text EquippedText;
        public bool equippedSlot;

        //Draggable Component
        public Draggable draggable;

        //Events
        public GameObjectEvent OnItemUsed =         new();
        public GameObjectEvent OnItemEquipped =     new();
        public GameObjectEvent OnItemUnEquipped =   new();
        public GameObjectEvent OnItemDropped =      new();
        public GameObjectEvent OnItemRemoved =      new();

        #region HiddenVariables
        //Hidden Variables
        [HideInInspector, SerializeField] private int Editor_Tabs1;
        #endregion
        // The following function is called everytime an item is added or removed from the inventory
        public void UpdateSlot(Item itemInSlot, int quantityInSlot)
        {
            item = itemInSlot;

            // If the item is null or the quantity 0 the slot is considered empty

            if (itemInSlot != null && quantityInSlot != 0)
            {
                // Slot has item: Enable the icon and Remove Button
                draggable.canDrag = true;
                //removeButton.gameObject.SetActive(true);
                //dropButton.gameObject.SetActive(true);
                itemImage.enabled = true;
                itemImage.sprite = itemInSlot.icon;

                if (selectedSlot)
                {
                    draggable.FocusOnItem();
                }
                else
                {
                    draggable.UnfocusOnItem(this);
                }


                //If the quantity on the slot is equal to one there is no necessity of enabling the quantity UI text
                if (quantityInSlot >= 1)
                {

                    quantity.enabled = true;
                    quantity.text = quantityInSlot.ToString();
                }
                else
                {
                    quantity.enabled = false;
                }

                if (equippedSlot)
                {
                    EquippedText.gameObject.SetActive(true);
                }
                else
                {
                    EquippedText.gameObject.SetActive(false);
                }

            }
            else
            {
                // Slot Empty: Disable the Icon, quantity and Remove Buttons
                draggable.canDrag = false;
                draggable.UnfocusOnItem(this);
                inventory.inventoryMaster.removeButton.gameObject.SetActive(false);
                inventory.inventoryMaster.dropButton.gameObject.SetActive(false);
                inventory.inventoryMaster.useButton.gameObject.SetActive(false);
                inventory.inventoryMaster.equipButton.gameObject.SetActive(false);
                itemImage.sprite = null;
                //itemImage.enabled = false; //This
                quantity.enabled = false;
                equippedSlot = false;
                EquippedText.gameObject.SetActive(false);
            }
        }

        // Called if the player mouses over this slot
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Tells the UI that shows the information of an item to appear and show it
            GetComponentInParent<ItemInfoUpdate>().UpdateInfoPanel(item);
            hoverOverImage.enabled = true;
        }

        // Called if the player take the mouse out of the slot borders
        public void OnPointerExit(PointerEventData eventData)
        {
            // Calls the function that sets the panel inactive
            GetComponentInParent<ItemInfoUpdate>().ClosePanel();
            hoverOverImage.enabled = false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            //getting the dropped gameobject
            GameObject dropped = eventData.pointerDrag;

            Draggable draggable = dropped.GetComponent<Draggable>();
            //Check to see if we are moving nothing. If so, do nothing.
            if (draggable.parentInventorySlot.item == null)
            {
                return;
            }
            //setting the draggable item varable
            Draggable draggableItem = dropped.GetComponent<Draggable>();
            //getting the parent slot in case we need to swap to another slot
            InventorySlot parentSlot = draggableItem.parentInventorySlot;
            //Calling Swap Items in case we need to swap and checking if this isn't the items slot
            if (draggableItem.parentInventorySlot != this)
            {
                SwapItemsInSlots(parentSlot, this);
            }

            draggable.transform.SetParent(draggable.parentAfterDrag);
            draggable.transform.SetAsFirstSibling();
            draggable.image.raycastTarget = true;
            //draggable.canDrag = false;
            //draggable.parentAfterDrag = null;
            //draggableItem.parentAfterDrag = transform;
        }

        public void SwapItemsInSlots(InventorySlot slot1, InventorySlot slot2)
        {
            Item item1 = slot1.item;
            int qty1 = item1 != null ? inventory.GetItemQuantity(slot1.SlotID) : 0;
            Item item2 = slot2.item;
            int qty2 = item2 != null ? inventory.GetItemQuantity(slot2.SlotID) : 0;

            if (slot1.selectedSlot)
            {
                slot2.selectedSlot = true;
                slot1.selectedSlot = false;
            }
            else if (slot2.selectedSlot)
            {
                slot2.selectedSlot = false;
                slot1.selectedSlot = true;
            }
            if (slot1.equippedSlot)
            {
                slot2.equippedSlot = true;
                slot1.equippedSlot = false;
            }
            else if (slot2.equippedSlot)
            {
                slot1.equippedSlot = true;
                slot2.equippedSlot = false;
            }

            Item tmpItem1, tmpItem2;
            int tmpQuantity1 = 0, tmpQuantity2 = 0;

            tmpItem1 = item1;
            tmpItem2 = item2;

            tmpQuantity1 = qty1;
            tmpQuantity2 = qty2;

            //Need to swap positions in the itemList and quantity List
            inventory.inventoryData.itemList[slot1.SlotID] = tmpItem2;
            inventory.inventoryData.itemList[slot2.SlotID] = tmpItem1;

            inventory.inventoryData.quantityList[slot1.SlotID] = tmpQuantity2;
            inventory.inventoryData.quantityList[slot2.SlotID] = tmpQuantity1;

            slot1.UpdateSlot(item2, qty2);
            slot2.UpdateSlot(item1, qty1);
        }
    }
}