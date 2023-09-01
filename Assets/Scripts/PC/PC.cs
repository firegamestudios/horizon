using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;
using TMPro;
using MalbersAnimations.Controller;

public class PC : MonoBehaviour
{
   
    SaveLoadManager saveLoadManager;
    PlayerData playerData;
    BulletTimeManager bulletTimeManager;

    //Malbers
    MAnimal animal;
    Stats stats;

    public MAttackTrigger attackTriggerKick;
    public MAttackTrigger attackTriggerLeftHand;
    public MAttackTrigger attackTriggerRightHand;

   //UI 
   UIManager uiManager;
    private void Awake()
    {
        saveLoadManager = FindAnyObjectByType<SaveLoadManager>();
        bulletTimeManager = FindAnyObjectByType<BulletTimeManager>();
        stats = GetComponent<Stats>();
        animal = GetComponent<MAnimal>();
        uiManager = FindAnyObjectByType<UIManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadCharacter();
    }

    #region Save Load

    void LoadCharacter()
    {
        saveLoadManager.LoadPlayerData();
        playerData = saveLoadManager.playerData;
        SetupStats();
    }

    #endregion

    #region Setup Character

    void SetupStats()
    {
        stats.stats[0].Value = playerData.Health;
        stats.stats[0].MaxValue = playerData.Health;
        stats.stats[1].Value = playerData.Energy;
        stats.stats[1].MaxValue = playerData.Energy;
        stats.stats[2].Value = playerData.Resistance;
        stats.stats[2].MaxValue = playerData.Resistance;

        //Melee Damage
        attackTriggerKick.statModifier.MinValue = playerData.MeleeDamage + playerData.MeleeBonus;
        attackTriggerKick.statModifier.MaxValue = playerData.MeleeDamage + playerData.Leadership + playerData.MeleeBonus;

    }

    #endregion

    #region Update
    private void Update()
    {
        if(uiManager != null)
        {
            uiManager.healthText.text = stats.stats[0].Value.ToString();
            uiManager.energyText.text = stats.stats[1].Value.ToString();
            uiManager.resistanceText.text = stats.stats[2].Value.ToString();
        }
      
    }
    #endregion

    #region Bullet Time

    public void BulletTime(int index)
    {
        bulletTimeManager.BulletTime(index);
    }

    #endregion

    #region My PlayerData
    public PlayerData PlayerData { get { return playerData; } }
    #endregion

    #region Player Control
    public void FreezePlayer()
    {
        animal.State_Force(0);
        animal.Sleep = true;
    }
    public void UnfreezePlayer()
    {
        animal.Sleep = false;
    }
    #endregion

}
