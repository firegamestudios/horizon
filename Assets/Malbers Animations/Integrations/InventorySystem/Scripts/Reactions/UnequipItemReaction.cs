using MalbersAnimations.Controller;
using UnityEngine;

namespace MalbersAnimations.InventorySystem
{
    [System.Serializable]
    [AddTypeMenu("Malbers/Inventory/Unequip Item")]
    public class UnequipItemReaction : InventoryMasterReaction
    { 
        protected override bool _TryReact(Component component)
        {
            var invMaster = component as InventoryMaster;
            if (invMaster == null) return false;

            var weaponManager = invMaster.character.Value.GetComponent<MWeaponManager>();    //Get the Weapon Manager
           // var animal = invMaster.character.Value.GetComponent<MAnimal>();                 //Get the character

            if (!invMaster.currentSelectedSlot.equippedSlot)
            {
                return true; // it means we aren't selecting the current equipped slot so therefore we cannot proceed...
            }

            if (invMaster.currentSelectedSlot.inventory.currentEquippedWeapon == null)
            {
                return true; //it means we have nothing currently as our equipped weapon...
            }

            //check to see if using external or holsters
            if (weaponManager.UseHolsters)
            {
                //if using holsters, we check if it's In the holster or not
                if (weaponManager.Weapon != null) //If there is a weapon, that means we have it in our hands
                {
                    //OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject);
                    invMaster.currentSelectedSlot.item.OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject); //Item Event
                    invMaster.currentSelectedSlot.inventory.OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject); //Inventory Event

                    ////Hack to change back to default stance.
                    //if (animal.Sleep)
                    //{
                    //    animal.Sleep = false;
                    //    weaponManager.UnEquip_Fast(); //Unequip it
                    //    animal.Sleep = true;
                    //}
                    //else
                    {
                        weaponManager.UnEquip_Fast(); //Unequip it
                    }

                    GameObject.Destroy(invMaster.currentSelectedSlot.inventory.currentEquippedWeapon.gameObject);
                    invMaster.currentSelectedSlot.inventory.currentEquippedWeapon = null;
                    weaponManager.ActiveHolster.Weapon = null;
                    invMaster.currentSelectedSlot.equippedSlot = false;
                    invMaster.currentSelectedSlot.EquippedText.gameObject.SetActive(false);
                    invMaster.currentSelectedSlot.draggable.FocusOnItem();
                }
                else //it means it's stored in a holster, remove from the holster, then unequip and destroy the prefab
                {
                    //OnItemUnEquipped.Invoke(weaponManager.ActiveHolster.Weapon.gameObject);
                    invMaster.currentSelectedSlot.item.OnItemUnEquipped.Invoke(weaponManager.ActiveHolster.Weapon.gameObject); //Item Event
                    invMaster.currentSelectedSlot.inventory.OnItemUnEquipped.Invoke(weaponManager.ActiveHolster.Weapon.gameObject); //Inventory Event
                    weaponManager.UnEquip_Fast(); //Unequip it
                    GameObject.Destroy(weaponManager.ActiveHolster.Weapon.gameObject);
                    invMaster.currentSelectedSlot.inventory.currentEquippedWeapon = null;
                    weaponManager.ActiveHolster.Weapon = null;
                    invMaster.currentSelectedSlot.equippedSlot = false;
                    invMaster.currentSelectedSlot.EquippedText.gameObject.SetActive(false);
                    invMaster.currentSelectedSlot.draggable.FocusOnItem();
                }




            }
            else //if using external, just fast unequip and destroy the prefab
            {
                //OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject);
                invMaster.currentSelectedSlot.item.OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject); //Item Event
                invMaster.currentSelectedSlot.inventory.OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject); //Inventory Event
                weaponManager.UnEquip_Fast(); //Unequip it
                GameObject.Destroy(invMaster.currentSelectedSlot.inventory.currentEquippedWeapon.gameObject);
                invMaster.currentSelectedSlot.inventory.currentEquippedWeapon = null;
                invMaster.currentSelectedSlot.equippedSlot = false;
                invMaster.currentSelectedSlot.EquippedText.gameObject.SetActive(false);
                invMaster.currentSelectedSlot.draggable.FocusOnItem();

            }
            return true;
        }
    }
}
