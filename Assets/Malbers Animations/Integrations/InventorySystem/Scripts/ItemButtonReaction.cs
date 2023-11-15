using UnityEngine;

namespace MalbersAnimations.InventorySystem
{
    [AddComponentMenu("Malbers/Inventory/Item Button Reaction")]

    public class ItemButtonReaction : MonoBehaviour
    {
        public string ButtonName;
        public InventoryMaster inventoryMaster;

        public void ButtonReactionFunction()
        {
            //var character = inventoryMaster.character.Value; //Find the Inventory Character

            switch (ButtonName)
            {
                case "Use":
                    inventoryMaster.currentSelectedSlot.item.UseReaction.reaction.TryReact(inventoryMaster); //React on the Inventory
                    break;
                case "Equip":
                    inventoryMaster.currentSelectedSlot.item.EquipReaction.reaction.TryReact(inventoryMaster);
                    break;
                case "Unequip":
                    inventoryMaster.currentSelectedSlot.item.UnequipReaction.reaction.TryReact(inventoryMaster);
                    break;
                case "Drop":
                    inventoryMaster.currentSelectedSlot.item.DropReaction.reaction.TryReact(inventoryMaster);
                    break;
                case "Remove":
                    inventoryMaster.currentSelectedSlot.item.RemoveReaction.reaction.TryReact(inventoryMaster);
                    break;
                default:
                    break;
            }
        }

    }
}