using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;
using TMPro;
using MalbersAnimations.Controller;

using Inworld;
using Inworld.Runtime;
using Inworld.Util;
using MalbersAnimations.Reactions;
using Cinemachine;
using MalbersAnimations.InventorySystem;

public class PC : MonoBehaviour
{
    //Conditions
    [SerializeField] bool isPoisoned;
    float poisonTimer;
    float poisonTimerReset;
    float poisonDuration;

    [SerializeField] bool isFast;
    float speedTimer;
    float speedTimerReset;
    float speedDuration;
    float speedPower;

    public int nextLevelXP;

    SaveLoadManager saveLoadManager;
    PlayerData playerData;
    BulletTimeManager bulletTimeManager;

    //Malbers
    MAnimal animal;
    Stats stats;
    MDamageable damageable;
    MalbersInput malbersInput;
    InventoryMaster inv;
   
    //Damage
    private GameObject weapon;
    Reaction customReaction;

    public MAttackTrigger attackTriggerKick;
    public MAttackTrigger attackTriggerLeftHand;
    public MAttackTrigger attackTriggerRightHand;

    //UI 
    UIManager uiManager;

    //material
    public Material matHead;
    public Material matBody;

    public Texture poisonTex;
    Texture bloodTex;

   
    //Elements
    //0 = poison
    public List<StatElement> statElements;

    //Camera
    CinemachineVirtualCamera virtualCamera;
    private void Awake()
    {
        virtualCamera = transform.Find("Internal Components").Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        saveLoadManager = FindAnyObjectByType<SaveLoadManager>();
        bulletTimeManager = FindAnyObjectByType<BulletTimeManager>();
        stats = GetComponent<Stats>();
        animal = GetComponent<MAnimal>();
        uiManager = FindAnyObjectByType<UIManager>();
        inv = GameObject.Find("Inventory Master").GetComponent<InventoryMaster>();
        inv.character = transform;
        inv.enabled = true;
        poisonTex = Resources.Load<Texture>("Textures/poison");
        damageable = GetComponent<MDamageable>();
        malbersInput = GetComponent<MalbersInput>();
       
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
        print("Name: " + playerData.playerName);
        InworldAI.User.Name = playerData.playerName;
       
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

        nextLevelXP = (int)playerData.XP + 150 * (playerData.Level * playerData.Level);

        uiManager.SetupPlayerName(playerData.playerName, playerData.classe, playerData.race);
        uiManager.SetupAttributes(playerData);

       
    }

    #endregion

    #region Update
    private void Update()
    {
        //Poison
        poisonDuration -= Time.deltaTime;
        if(poisonDuration <= 0)
        {
           EndPoison();
        }

        if (isPoisoned)
        {
            poisonTimer -= Time.deltaTime;
            if(poisonTimer < 0)
            {
                poisonTimer = poisonTimerReset;
                PoisonDamage();
            }
        }
        //Speed
        speedDuration -= Time.deltaTime;
        if (speedDuration <= 0)
        {
            EndSpeed();
        }

        if (isFast)
        {
            speedTimer -= Time.deltaTime;
            if (speedTimer < 0)
            {
                speedTimer = speedTimerReset;
                //do nothing
            }
        }

        if (uiManager != null)
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
        print("Freeze Player()");
        if(animal.State_Get(0).Active == false)
        animal.State_Force(0);
        malbersInput.enabled = false;
    }
    public void UnfreezePlayer()
    {
        malbersInput.enabled = true;
       
    }
    public void FreezeCamera(bool isOrbitInputActive)
    {
       
        if (virtualCamera != null)
        {
            if (isOrbitInputActive)
            {
                virtualCamera.Priority = -100;
            }
            else
            {
                virtualCamera.Priority = 100;
            }
        }
    }
    #endregion

    #region Conditions
    public void ConditionControl()
    {
        if(damageable.LastDamage.Element.element == statElements[0])
        {
            print("Last Element: " + damageable.LastDamage.Element.element.ToString());
            OnPoisoned(1f, 10f);
        }
    }
    public void OnPoisoned(float _poisonTimer, float _poisonDuration)
    {
        poisonTimer = _poisonTimer;
        poisonTimerReset = _poisonTimer;
        poisonDuration = _poisonDuration;
        isPoisoned = true;

        //trocar material
        matHead.SetFloat("_BloodIntensity", 1f);
        matBody.SetFloat("_BloodIntensity", 1f);
        matHead.SetTexture("_BloodMap", poisonTex);
        matBody.SetTexture("_BloodMap", poisonTex);
    }
    public void OnSpeed(float _speedTimer, float _speedDuration)
    {
        speedTimer = _speedTimer;
        speedTimerReset = _speedTimer;
        speedDuration = _speedDuration;
        isFast = true;

        //VFX
        transform.Find("Internal Components").Find("Effects").Find("Speed").gameObject.SetActive(true);
    }
    void EndSpeed()
    {
        isFast = false;
        animal.AnimatorSpeed = 1f;
        transform.Find("Internal Components").Find("Effects").Find("Speed").GetComponent<ParticleSystem>().Stop();
       // print("SPEED INSCREASE ENDED");
    }
    void PoisonDamage()
    {
        //Droids don't take poison damage
        if (playerData.race == "Droid") return;
       
        float poisonDamage = Random.Range(1 * playerData.Level, 2 * playerData.Level);
        
        print("Poison Damage: " + poisonDamage.ToString());
      
        damageable.ReceiveDamage(stats.stats[0].ID, poisonDamage);
    }

    void EndPoison()
    {
        isPoisoned = false;
        matHead.SetFloat("_BloodIntensity", 0f);
        matBody.SetFloat("_BloodIntensity", 0f);
    }
    #endregion

    #region Effects
    public void Heal(float amount)
    {
        stats.stats[0].Value += amount;
    }
    public void Speed(float amount, float timer)
    {
        isFast = true;
        speedPower = amount;
        animal.AnimatorSpeed = speedPower;
        transform.Find("Internal Components").Find("Effects").Find("Speed").GetComponent<ParticleSystem>().Play();
        print("SPEED INCREASED");
        OnSpeed(speedPower, timer);
    }
    #endregion

    #region OnHurt and OnDeath
    public void OnHurt()
    {

    }

    public void OnDeath()
    {
        
    }
    #endregion

}
