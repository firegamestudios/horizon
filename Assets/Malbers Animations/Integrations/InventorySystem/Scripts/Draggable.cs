using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MalbersAnimations.InventorySystem
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public bool canDrag;
        public Transform parentAfterDrag;
        public Image image;
        public InventorySlot parentInventorySlot;

        private void Start()
        {
            //If we dont have a parent slot set, then we set one.
            if (parentInventorySlot == null)
            {
                parentInventorySlot = transform.parent.GetComponent<InventorySlot>();
            }
        }
        //Called if Player Begins to Drag - gets amended via the individual Inventory Slots
        public void OnBeginDrag(PointerEventData eventData)
        {
            parentAfterDrag = transform.parent; //Remembering the parent (Slot) so we can reset AFTER we drag if needed
            if (canDrag)
            {

                transform.SetParent(parentAfterDrag.parent); //Setting the parent to the Slots parent so that when dragging, it appears on top of everything
                transform.SetAsLastSibling(); //Setting as last sibling to show on top
                image.raycastTarget = false;
            }
        }

        //Called if Player is dragging slot
        public void OnDrag(PointerEventData eventData)
        {
            if (canDrag)
            {
                transform.position = eventData.position;
            }
        }

        //Calls when player finishes dragging
        public void OnEndDrag(PointerEventData eventData)
        {
            //Adds check to make sure we are actually moving an item rather than nothing.
            //Debug.Log("Test");
            //Debug.Log(eventData.ToString());

            if (eventData.pointerEnter == null || eventData.pointerEnter.CompareTag("Inventory") || eventData.pointerEnter.CompareTag("Untagged"))
            {
                transform.SetParent(parentAfterDrag);
                transform.SetAsFirstSibling();
                image.raycastTarget = true;
                if (parentInventorySlot.item == null)
                {
                    canDrag = false;
                }

                parentAfterDrag = null;
                return;
            }


            if (eventData.pointerEnter.GetComponent<Draggable>())
            {
                return;
            }


        }

        public void OnPointerClick(PointerEventData eventData)
        {
            FocusOnItem();
        }

        public void FocusOnItem()
        {
            //Check to see if we have an item in the slot. If not, we Unfocus instead. If we do, we continue...
            if (parentInventorySlot.item == null)
            {

                UnfocusOnItem(parentInventorySlot);


                return;
            }


            foreach (InventorySlot slot in parentInventorySlot.inventory.slotList)
            {
                //Check all slots to see if it's the selected slot.
                if (slot.selectedSlot == true)
                {
                    //any that are marked as active, we set to false
                    slot.selectedSlotImage.enabled = false;
                    slot.selectedSlot = false;
                }
            }

            //Set inventory master selected slot to null
            parentInventorySlot.inventory.inventoryMaster.currentSelectedSlot = null;

            //then set this slot to the active slot
            parentInventorySlot.selectedSlot = true;
            parentInventorySlot.selectedSlotImage.enabled = true;
            //Set the inventory masters selected slot also
            parentInventorySlot.inventory.inventoryMaster.currentSelectedSlot = parentInventorySlot;

            //Set Up Available Buttons - we make each one enabled based on the item. Some may be equippable and others arent therefore we dont need
            //certain buttons.
            parentInventorySlot.inventory.inventoryMaster.removeButton.gameObject.SetActive(parentInventorySlot.item.Discardable);
            parentInventorySlot.inventory.inventoryMaster.dropButton.gameObject.SetActive(parentInventorySlot.item.Droppable);
            parentInventorySlot.inventory.inventoryMaster.useButton.gameObject.SetActive(parentInventorySlot.item.Usable);

            if (parentInventorySlot.equippedSlot == true)
            {
                //If it's currently the equipped weapon, we set the unequip button, otherwise we set the equip button
                parentInventorySlot.inventory.inventoryMaster.unequipButton.gameObject.SetActive(parentInventorySlot.item.Equippable);
                parentInventorySlot.inventory.inventoryMaster.equipButton.gameObject.SetActive(false);
            }
            else
            {
                parentInventorySlot.inventory.inventoryMaster.equipButton.gameObject.SetActive(parentInventorySlot.item.Equippable);
                parentInventorySlot.inventory.inventoryMaster.unequipButton.gameObject.SetActive(false);
            }

            removeAllButtonListenersFromParentSlot();
            addButtonListenersFromParentSlot();

        }

        public void addButtonListenersFromParentSlot()
        {
            parentInventorySlot.inventory.inventoryMaster.useButton.onClick.AddListener
               (parentInventorySlot.inventory.inventoryMaster.useButton.GetComponent<ItemButtonReaction>().ButtonReactionFunction);
            //Debug.Log("Added Use Listener");

            parentInventorySlot.inventory.inventoryMaster.removeButton.onClick.AddListener
                (parentInventorySlot.inventory.inventoryMaster.removeButton.GetComponent<ItemButtonReaction>().ButtonReactionFunction);

            parentInventorySlot.inventory.inventoryMaster.dropButton.onClick.AddListener
                (parentInventorySlot.inventory.inventoryMaster.dropButton.GetComponent<ItemButtonReaction>().ButtonReactionFunction);

            parentInventorySlot.inventory.inventoryMaster.unequipButton.onClick.AddListener
                (parentInventorySlot.inventory.inventoryMaster.unequipButton.GetComponent<ItemButtonReaction>().ButtonReactionFunction);

            parentInventorySlot.inventory.inventoryMaster.equipButton.onClick.AddListener
                (parentInventorySlot.inventory.inventoryMaster.equipButton.GetComponent<ItemButtonReaction>().ButtonReactionFunction);
        }

        public void removeAllButtonListenersFromParentSlot()
        {
            //Clear all listeners First
            //Now we clear the events if any exist
            parentInventorySlot.inventory.inventoryMaster.removeButton.onClick.RemoveAllListeners();
            parentInventorySlot.inventory.inventoryMaster.dropButton.onClick.RemoveAllListeners();
            //Debug.Log("Removed Use Listener");
            parentInventorySlot.inventory.inventoryMaster.useButton.onClick.RemoveAllListeners();
            parentInventorySlot.inventory.inventoryMaster.equipButton.onClick.RemoveAllListeners();
            parentInventorySlot.inventory.inventoryMaster.unequipButton.onClick.RemoveAllListeners();
        }
        public void UnfocusOnItem(InventorySlot slot)
        {
            slot.selectedSlot = false;
            slot.selectedSlotImage.enabled = false;
            slot.inventory.inventoryMaster.currentSelectedSlot = null;
            //If we click on a slot that doesnt contain an item, then we remove the buttons
            slot.inventory.inventoryMaster.removeButton.gameObject.SetActive(false);
            slot.inventory.inventoryMaster.dropButton.gameObject.SetActive(false);
            slot.inventory.inventoryMaster.useButton.gameObject.SetActive(false);
            slot.inventory.inventoryMaster.equipButton.gameObject.SetActive(false);
            slot.inventory.inventoryMaster.unequipButton.gameObject.SetActive(false);
            //Now we clear the events if any exist
            slot.inventory.inventoryMaster.removeButton.onClick.RemoveAllListeners();
            slot.inventory.inventoryMaster.dropButton.onClick.RemoveAllListeners();
            slot.inventory.inventoryMaster.useButton.onClick.RemoveAllListeners();
            slot.inventory.inventoryMaster.equipButton.onClick.RemoveAllListeners();
            slot.inventory.inventoryMaster.unequipButton.onClick.RemoveAllListeners();
        }
    }
}
