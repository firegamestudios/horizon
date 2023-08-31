using MalbersAnimations.Weapons;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    PlayerData playerData;

    MMelee melee;

    public bool equipped = false;

    TMP_Text weaponDamageText;

    private void Awake()
    {
        melee = GetComponent<MMelee>(); 
        weaponDamageText = GameObject.Find("Weapon Damage Text").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if(equipped)
        {
            weaponDamageText.text = "Weapon Damage\n Min: " + melee.statModifier.MinValue.Value.ToString() + "\n Max: " + melee.statModifier.MaxValue.Value.ToString();
        }
    }

    #region Equip and Unequip

    public void OnEquipped()
    {
        playerData = FindAnyObjectByType<PC>().PlayerData;

        float currentMin = melee.statModifier.MinValue;
        float currentMax = melee.statModifier.MaxValue;
        melee.statModifier.MinValue = currentMin + playerData.MeleeDamage + playerData.MeleeBonus;
        melee.statModifier.MaxValue = currentMax + playerData.MeleeDamage + playerData.MeleeBonus + playerData.Leadership;
        equipped = true;
        print("Equipped");
    }

    public void OnUnequipped()
    {
       equipped= false;
    }
    #endregion
}
