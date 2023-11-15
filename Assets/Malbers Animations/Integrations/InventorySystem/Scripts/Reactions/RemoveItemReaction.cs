using MalbersAnimations.Controller;
using UnityEngine;

namespace MalbersAnimations.InventorySystem
{
    [System.Serializable]
    [AddTypeMenu("Malbers/Inventory/Remove Item")]
    public class RemoveItemReaction : InventoryMasterReaction
    { 
        protected override bool _TryReact(Component component)
        {
            var invMaster = component as InventoryMaster;
            if (invMaster == null) return false;
            var weaponManager = invMaster.character.Value.GetComponent<MWeaponManager>();    //Get the Weapon Manager
            var animal = invMaster.character.Value.GetComponent<MAnimal>();                 //Get the character


            // Removes item from the Inventory Completely - it's a delete function.
            if (invMaster.currentSelectedSlot.item != null)
            {
                if (invMaster.currentSelectedSlot.item.type.ID == 2)
                {
                    if (weaponManager.UseHolsters)
                    {
                        Holster holster = weaponManager.ActiveHolster;

                        //First we see if we have a weapon equipped 
                        if (weaponManager.Weapon)
                        {
                            //Next we check if the Weapon IS the same one as item in the slot...
                            if (weaponManager.Weapon.GetComponent<InventoryItem>().inventoryItem == invMaster.currentSelectedSlot.item)
                            {
                                //OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject);
                                //OnItemRemoved.Invoke(weaponManager.Weapon.gameObject);
                                invMaster.currentSelectedSlot.item.OnItemUnEquipped.Invoke(null); //Item Event
                                invMaster.currentSelectedSlot.item.OnItemRemoved.Invoke(null); //Item Event
                                invMaster.currentSelectedSlot.inventory.OnItemUnEquipped.Invoke(null); //Inventory Event
                                invMaster.currentSelectedSlot.inventory.OnItemRemoved.Invoke(null); //Inventory Event
                                invMaster.currentSelectedSlot.inventory.DestroyGameObject(weaponManager.Weapon.gameObject);
                                invMaster.currentSelectedSlot.inventory.inventoryData.itemList[invMaster.currentSelectedSlot.SlotID] = null;
                                invMaster.currentSelectedSlot.inventory.inventoryData.quantityList[invMaster.currentSelectedSlot.SlotID] = 0;
                                if (animal.Sleep == true)
                                {
                                    //hack to reset stance as it wont reset whilst lock input/sleep is on so we turn off temporarily.
                                    animal.Sleep = false;
                                    weaponManager.HolsterClearAll();
                                    animal.Stance_Reset();
                                    animal.Stance_RestoreDefault();
                                    animal.Sleep = true;
                                }
                                else
                                {
                                    weaponManager.HolsterClearAll();
                                    animal.Stance_Reset();
                                    animal.Stance_RestoreDefault();
                                }
                                //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
                                invMaster.currentSelectedSlot.UpdateSlot(null, 0);
                                return true;
                            }
                            else //if the item selected IS a weapon BUT isn't the one equipped then we drop the item but not the one equipped...
                            {
                                invMaster.currentSelectedSlot.inventory.inventoryData.itemList[invMaster.currentSelectedSlot.SlotID] = null;
                                invMaster.currentSelectedSlot.inventory.inventoryData.quantityList[invMaster.currentSelectedSlot.SlotID] = 0;

                                //OnItemRemoved.Invoke(null);
                                invMaster.currentSelectedSlot.item.OnItemRemoved.Invoke(null); //Item Event
                                invMaster.currentSelectedSlot.inventory.OnItemRemoved.Invoke(null); //Inventory Event
                                //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
                                invMaster.currentSelectedSlot.UpdateSlot(null, 0);

                                return true;
                            }
                        }

                        //If it's not equipped currently we then check if it's in the holster.
                        if (holster.Weapon != null) //we have a weapon in a holster
                        {

                            if (holster.Weapon.GetComponent<InventoryItem>().inventoryItem != invMaster.currentSelectedSlot.item) //check to see if the item we are dropping is the same as what is in the holster
                            {
                                //Destroy(holster.Weapon.gameObject);
                                invMaster.currentSelectedSlot.inventory.inventoryData.itemList[invMaster.currentSelectedSlot.SlotID] = null;
                                invMaster.currentSelectedSlot.inventory.inventoryData.quantityList[invMaster.currentSelectedSlot.SlotID] = 0;

                                //OnItemRemoved.Invoke(null);
                                invMaster.currentSelectedSlot.item.OnItemRemoved.Invoke(null); //Item Event
                                invMaster.currentSelectedSlot.inventory.OnItemRemoved.Invoke(null); //Inventory Event
                                //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
                                invMaster.currentSelectedSlot.UpdateSlot(null, 0);


                            }
                            else// it is the same item as what's in the holster
                            {
                                if (invMaster.currentSelectedSlot.equippedSlot) //if we've chosen the equipped slot and it's in the holster, then destroy the holster GO
                                {
                                    //OnItemUnEquipped.Invoke(holster.Weapon.gameObject);
                                    //OnItemRemoved.Invoke(holster.Weapon.gameObject);
                                    invMaster.currentSelectedSlot.item.OnItemUnEquipped.Invoke(holster.Weapon.gameObject); //Item Event
                                    invMaster.currentSelectedSlot.item.OnItemRemoved.Invoke(holster.Weapon.gameObject); //Item Event
                                    invMaster.currentSelectedSlot.inventory.OnItemUnEquipped.Invoke(holster.Weapon.gameObject); //Inventory Event
                                    invMaster.currentSelectedSlot.inventory.OnItemRemoved.Invoke(holster.Weapon.gameObject); //Inventory Event
                                    invMaster.currentSelectedSlot.inventory.DestroyGameObject(holster.Weapon.gameObject);
                                }

                                invMaster.currentSelectedSlot.inventory.inventoryData.itemList[invMaster.currentSelectedSlot.SlotID] = null;
                                invMaster.currentSelectedSlot.inventory.inventoryData.quantityList[invMaster.currentSelectedSlot.SlotID] = 0;

                                //OnItemRemoved.Invoke(null);
                                invMaster.currentSelectedSlot.item.OnItemRemoved.Invoke(null); //Item Event
                                invMaster.currentSelectedSlot.inventory.OnItemRemoved.Invoke(null); //Inventory Event
                                //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
                                invMaster.currentSelectedSlot.UpdateSlot(null, 0);

                            }

                        }
                        else //For every other scenario
                        {
                            invMaster.currentSelectedSlot.inventory.inventoryData.itemList[invMaster.currentSelectedSlot.SlotID] = null;
                            invMaster.currentSelectedSlot.inventory.inventoryData.quantityList[invMaster.currentSelectedSlot.SlotID] = 0;
                            //OnItemRemoved.Invoke(null);
                            invMaster.currentSelectedSlot.item.OnItemRemoved.Invoke(null); //Item Event
                            invMaster.currentSelectedSlot.inventory.OnItemRemoved.Invoke(null); //Inventory Event
                            //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
                            invMaster.currentSelectedSlot.UpdateSlot(null, 0);
                        }

                    }
                    else //Use External
                    {
                        if (weaponManager.Weapon)
                        {
                            //OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject);
                            //OnItemRemoved.Invoke(weaponManager.Weapon.gameObject);
                            invMaster.currentSelectedSlot.item.OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject); //Item Event
                            invMaster.currentSelectedSlot.item.OnItemRemoved.Invoke(weaponManager.Weapon.gameObject); //Item Event
                            invMaster.currentSelectedSlot.inventory.OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject); //Inventory Event
                            invMaster.currentSelectedSlot.inventory.OnItemRemoved.Invoke(weaponManager.Weapon.gameObject); //Inventory Event
                            weaponManager.UnEquip_Fast(); //Unequip it
                            invMaster.currentSelectedSlot.inventory.DestroyGameObject(invMaster.currentSelectedSlot.inventory.currentEquippedWeapon.gameObject);
                        }
                        invMaster.currentSelectedSlot.inventory.currentEquippedWeapon = null;
                        invMaster.currentSelectedSlot.inventory.inventoryData.itemList[invMaster.currentSelectedSlot.SlotID] = null;
                        invMaster.currentSelectedSlot.inventory.inventoryData.quantityList[invMaster.currentSelectedSlot.SlotID] = 0;

                        //OnItemRemoved.Invoke(null);
                        invMaster.currentSelectedSlot.item.OnItemRemoved.Invoke(null); //Item Event
                        invMaster.currentSelectedSlot.inventory.OnItemRemoved.Invoke(null); //Inventory Event
                        invMaster.currentSelectedSlot.UpdateSlot(null, 0);
                        //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);

                    }
                }
                else //item isn't a weapon so we just set everything back to null and update the slot.
                {
                    invMaster.currentSelectedSlot.item.OnItemRemoved.Invoke(null); //Item Event
                    invMaster.currentSelectedSlot.inventory.OnItemRemoved.Invoke(null); //Inventory Event
                    //OnItemRemoved.Invoke(null);
                    invMaster.currentSelectedSlot.inventory.inventoryData.itemList[invMaster.currentSelectedSlot.SlotID] = null;
                    invMaster.currentSelectedSlot.inventory.inventoryData.quantityList[invMaster.currentSelectedSlot.SlotID] = 0;
                    //invMaster.currentSelectedSlot.draggable.UnfocusOnItem(invMaster.currentSelectedSlot);
                    invMaster.currentSelectedSlot.UpdateSlot(null, 0);
                }

            }

            return true;
        }
    }
}
