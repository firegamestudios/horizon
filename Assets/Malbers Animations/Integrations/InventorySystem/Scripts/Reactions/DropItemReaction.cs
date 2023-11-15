using MalbersAnimations.Controller;
using UnityEngine;

namespace MalbersAnimations.InventorySystem
{
    [System.Serializable]
    [AddTypeMenu("Malbers/Inventory/Drop Item")]
    public class DropItemReaction : InventoryMasterReaction
    {
        protected override bool _TryReact(Component component)
        {
            var invMaster = component as InventoryMaster;
            if (invMaster == null) return false;

            var weaponManager = invMaster.character.Value.GetComponent<MWeaponManager>();    //Get the Weapon Manager
            var animal = invMaster.character.Value.GetComponent<MAnimal>();                 //Get the character

            if (invMaster.currentSelectedSlot.item.type.ID != 2)
            {
                //OnItemDropped.Invoke(null);
                invMaster.currentSelectedSlot.item.OnItemDropped.Invoke(null); //Item Event
                invMaster.currentSelectedSlot.inventory.OnItemDropped.Invoke(null); //Inventory Event
                invMaster.currentSelectedSlot.inventory.DropItem(invMaster.currentSelectedSlot.item, 1, invMaster.currentSelectedSlot.SlotID);
                //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
                return true;
            }

            //Otherwise if it IS a weapon, we check the weapon manager to see if we are using holsters or not...
            if (weaponManager.UseHolsters)
            {
                Holster holster = weaponManager.ActiveHolster;

                //First we see if we have a weapon equipped 
                if (weaponManager.Weapon)
                {
                    //Next we check if the Weapon IS the same one as item in the slot...
                    if (weaponManager.Weapon.GetComponent<InventoryItem>().inventoryItem == invMaster.currentSelectedSlot.item)
                    {
                        //OnItemDropped.Invoke(weaponManager.Weapon.gameObject);
                        invMaster.currentSelectedSlot.item.OnItemDropped.Invoke(weaponManager.Weapon.gameObject); //Item Event
                        invMaster.currentSelectedSlot.inventory.OnItemDropped.Invoke(weaponManager.Weapon.gameObject); //Inventory Event
                        weaponManager.HolsterClearAll();
                        if (animal.Sleep == true)
                        {
                            //hack to reset stance as it wont reset whilst lock input/sleep is on so we turn off temporarily.
                            animal.Sleep = false;
                            animal.Stance_Reset();
                            animal.Stance_RestoreDefault();
                            animal.Sleep = true;
                        }
                        else
                        {
                            animal.Stance_Reset();
                            animal.Stance_RestoreDefault();
                        }

                        weaponManager.Weapon = null;     //IMPORTANT
                        invMaster.currentSelectedSlot.inventory.DropItem(invMaster.currentSelectedSlot.item, 1, invMaster.currentSelectedSlot.SlotID);
                        return true;
                    }
                    else //if the item selected IS a weapon BUT isn't the one equipped then we drop the item but not the one equipped...
                    {
                        //OnItemDropped.Invoke(null);
                        invMaster.currentSelectedSlot.item.OnItemDropped.Invoke(null); //Item Event
                        invMaster.currentSelectedSlot.inventory.OnItemDropped.Invoke(null); //Inventory Event
                        invMaster.currentSelectedSlot.inventory.DropItem(invMaster.currentSelectedSlot.item, 1, invMaster.currentSelectedSlot.SlotID);
                        //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
                        return true;
                    }
                }

                //If it's not equipped currently we then check if it's in the holster.
                if (holster.Weapon != null) //we have a weapon in a holster
                {

                    if (holster.Weapon.GetComponent<InventoryItem>().inventoryItem != invMaster.currentSelectedSlot.item) //check to see if the item we are dropping is the same as what is in the holster
                    {
                        //Destroy(holster.Weapon.gameObject);
                        //OnItemDropped.Invoke(null);
                        invMaster.currentSelectedSlot.item.OnItemDropped.Invoke(null); //Item Event
                        invMaster.currentSelectedSlot.inventory.OnItemDropped.Invoke(null); //Inventory Event
                        invMaster.currentSelectedSlot.inventory.DropItem(invMaster.currentSelectedSlot.item, 1, invMaster.currentSelectedSlot.SlotID);
                        //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
                    }
                    else// it is the same item as what's in the holster
                    {
                        //OnItemDropped.Invoke(null);
                        if (invMaster.currentSelectedSlot.equippedSlot) //if we've chosen the equipped slot and it's in the holster, then destroy the holster GO
                        {

                            invMaster.currentSelectedSlot.inventory.DestroyGameObject(holster.Weapon.gameObject);
                        }

                        invMaster.currentSelectedSlot.inventory.DropItem(invMaster.currentSelectedSlot.item, 1, invMaster.currentSelectedSlot.SlotID);
                        //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
                    }

                }
                else //For every other scenario
                {
                    //OnItemDropped.Invoke(null);
                    invMaster.currentSelectedSlot.item.OnItemDropped.Invoke(null); //Item Event
                    invMaster.currentSelectedSlot.inventory.OnItemDropped.Invoke(null); //Inventory Event
                    invMaster.currentSelectedSlot.inventory.DropItem(invMaster.currentSelectedSlot.item, 1, invMaster.currentSelectedSlot.SlotID);
                    //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
                }

            }
            else //Use External //Need to finish this
            {
                //OnItemDropped.Invoke(null);
                invMaster.currentSelectedSlot.item.OnItemDropped.Invoke(null); //Item Event
                invMaster.currentSelectedSlot.inventory.OnItemDropped.Invoke(null); //Inventory Event

                //Check for weapon
                //If Weapon Exists then check if current equipped weapon
                //if yes, then unequip


                //We have Weapon Equipped
                if (weaponManager.Weapon != null)
                {
                    //We check if the current selected weapon in the slot we are dropping matches the equipped weapon
                    if (weaponManager.Weapon.gameObject.GetComponent<InventoryItem>().inventoryItem == invMaster.currentSelectedSlot.item)
                    {
                        //If true, we unequip, if false, we skip this and just drop whatever we selected
                        weaponManager.UnEquip_Fast();
                    }
                    //invMaster.currentSelectedSlot.inventory.DestroyGameObject(weaponManager.Weapon.gameObject);
                }

                invMaster.currentSelectedSlot.inventory.DropItem(invMaster.currentSelectedSlot.item, 1, invMaster.currentSelectedSlot.SlotID);
                //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
            }

            return true;
        }

    }

}
