using UnityEngine; 

namespace MalbersAnimations.InventorySystem
{
    [System.Serializable]
    [AddTypeMenu("Malbers/Inventory/Use Item")]
    public class UseItemReaction : InventoryMasterReaction
    { 
        protected override bool _TryReact(Component component)
        {
            InventoryMaster invMaster = component as InventoryMaster;
            if (invMaster == null) return false;

            var character = invMaster.character.Value;

            if (invMaster.currentSelectedSlot.item != null)
            {
                invMaster.currentSelectedSlot.item.OnItemUsed.Invoke(character.gameObject);
                invMaster.currentSelectedSlot.inventory.OnItemUsed.Invoke(character.gameObject);
                invMaster.currentSelectedSlot.inventory.RemoveItem(invMaster.currentSelectedSlot.item, 1, invMaster.currentSelectedSlot.SlotID);
                Debug.Log("Used 1 Item");
                #region oldCode
                // Use the item by calling the function of that specific item
                //invMaster.currentSelectedSlot.item.OnItemUsed.Invoke(null); //Item Event
                //invMaster.currentSelectedSlot.item.Use(invMaster.currentSelectedSlot.SlotID);
                //invMaster.currentSelectedSlot.OnItemUsed.Invoke(null); //Slot Event
                //Inventory.instance.OnItemUsed.Invoke(null); //Inventory Event
                #endregion
            }

            return true;
        }
    }
}
