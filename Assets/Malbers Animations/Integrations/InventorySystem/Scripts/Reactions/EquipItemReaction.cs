using MalbersAnimations.Weapons;
using UnityEngine;

namespace MalbersAnimations.InventorySystem
{
    [System.Serializable]
    [AddTypeMenu("Malbers/Inventory/Equip Item")]
    public class EquipItemReaction : InventoryMasterReaction
    {
        protected override bool _TryReact(Component component)
        {
            //Get Main Character and Weapon Manager Reference.
            InventoryMaster invMaster = component as InventoryMaster;
            if (invMaster == null) return false;

            MWeaponManager weaponManager = invMaster.character.Value.GetComponent<MWeaponManager>();

            //Check to see if there is already a weapon attached. If so, unequip it to prepare for new equip.
            if (weaponManager.Weapon != null)
            {
                invMaster.currentSelectedSlot.item.OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject); //Item Event
                invMaster.currentSelectedSlot.inventory.OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject); //Inventory Event
                //OnItemUnEquipped.Invoke(weaponManager.Weapon.gameObject);
                weaponManager.UnEquip_Fast();
                GameObject.Destroy(invMaster.currentSelectedSlot.inventory.currentEquippedWeapon.gameObject);
                invMaster.currentSelectedSlot.inventory.currentEquippedWeapon = null;
            }

            //Instantiate the new weapon and make it equal to the Weapon variable. Then equip it.
            weaponManager.Weapon = GameObject.Instantiate(invMaster.currentSelectedSlot.item.inWorldPrefab, invMaster.transform.position, Quaternion.identity).GetComponent<MWeapon>();
            invMaster.currentSelectedSlot.inventory.currentEquippedWeapon = weaponManager.Weapon;

            //If using Holsters:

            if (weaponManager.UseHolsters)
            {
                //Store Weapon into active holster
                //weaponManager.Store_Weapon(weaponManager.Weapon.Holster, true);
                //Set Weapon on active holster
                weaponManager.Holster_SetWeapon(weaponManager.Weapon);

                foreach (InventorySlot slot in invMaster.currentSelectedSlot.inventory.slotList)
                {
                    slot.equippedSlot = false; //Make sure we set all other slots to false
                    slot.EquippedText.gameObject.SetActive(false);
                }
                invMaster.currentSelectedSlot.equippedSlot = true;
                invMaster.currentSelectedSlot.EquippedText.gameObject.SetActive(true);
                invMaster.currentSelectedSlot.draggable.FocusOnItem();
                //OnItemEquipped.Invoke(null);
                invMaster.currentSelectedSlot.item.OnItemEquipped.Invoke(null); //Item Event
                invMaster.currentSelectedSlot.inventory.OnItemEquipped.Invoke(null); //Inventory Event
            }
            else //Not using Holsters
            {
                weaponManager.Equip_Fast();

                foreach (InventorySlot slot in invMaster.currentSelectedSlot.inventory.slotList)
                {
                    slot.equippedSlot = false;
                    slot.EquippedText.gameObject.SetActive(false);
                }
                invMaster.currentSelectedSlot.equippedSlot = true;
                invMaster.currentSelectedSlot.EquippedText.gameObject.SetActive(true);
                invMaster.currentSelectedSlot.draggable.FocusOnItem();
                //OnItemEquipped.Invoke(null);
                invMaster.currentSelectedSlot.item.OnItemEquipped.Invoke(null); //Item Event
                invMaster.currentSelectedSlot.inventory.OnItemEquipped.Invoke(null); //Inventory Event
            }
            return true;
        }
    }
}
