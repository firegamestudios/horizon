using MalbersAnimations.Weapons;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    PlayerData playerData;

    MMelee melee;
    MShootable shootable;
    MBow bow;

    public bool equipped = false;

    TMP_Text weaponDamageText;

    private void Awake()
    {
        if(GetComponent<MMelee>() != null)
            melee = GetComponent<MMelee>();

        if (GetComponent<MShootable>() != null)
            shootable = GetComponent<MShootable>();

        if (GetComponent<MBow>() != null)
            bow = GetComponent<MBow>();

        weaponDamageText = GameObject.Find("Weapon Damage Text").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if(equipped)
        {
            if(melee)
                weaponDamageText.text = "Weapon Damage\n Min: " + melee.statModifier.MinValue.Value.ToString() + "\n Max: " + melee.statModifier.MaxValue.Value.ToString();
            if (shootable)
                weaponDamageText.text = "Weapon Damage\n Min: " + shootable.statModifier.MinValue.Value.ToString() + "\n Max: " + shootable.statModifier.MaxValue.Value.ToString();
            if (bow)
                weaponDamageText.text = "Weapon Damage\n Min: " + bow.statModifier.MinValue.Value.ToString() + "\n Max: " + bow.statModifier.MaxValue.Value.ToString();
        }
    }

    #region Equip and Unequip

    public void OnEquipped()
    {
        playerData = FindAnyObjectByType<PC>().PlayerData;

        if (melee)
        {
            float currentMin = melee.statModifier.MinValue;
            float currentMax = melee.statModifier.MaxValue;
            melee.statModifier.MinValue = currentMin + playerData.MeleeDamage + playerData.MeleeBonus;
            melee.statModifier.MaxValue = currentMax + playerData.MeleeDamage + playerData.MeleeBonus + playerData.Leadership;
        }
        if (shootable)
        {
            float currentMin = shootable.statModifier.MinValue;
            float currentMax = shootable.statModifier.MaxValue;
            shootable.statModifier.MinValue = currentMin + playerData.RangedDamage + playerData.RangedBonus;
            shootable.statModifier.MaxValue = currentMax + playerData.RangedDamage + playerData.RangedBonus + playerData.Leadership;
        }
        if (bow)
        {
            float currentMin = bow.statModifier.MinValue;
            float currentMax = bow.statModifier.MaxValue;
            bow.statModifier.MinValue = currentMin + playerData.RangedDamage + playerData.RangedBonus;
            bow.statModifier.MaxValue = currentMax + playerData.RangedDamage + playerData.RangedBonus + playerData.Leadership;
        }
        equipped = true;
        print("Equipped");
    }

    public void OnUnequipped()
    {
       equipped= false;
    }
    #endregion
}
