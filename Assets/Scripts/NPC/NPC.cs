using Cinemachine;
using MalbersAnimations;
using MalbersAnimations.Controller;
using MalbersAnimations.Controller.AI;
using MalbersAnimations.Reactions;
using MalbersAnimations.Weapons;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Droidzone.Core;
public class NPC : MonoBehaviour
{
    //My Components
    protected MAnimal animal;
    MDamageable damageable;
    Rigidbody rb;
    Stats stats;
    
    Animator anim;
    Blood blood;

    //The player
    private PC pc;

    /* -> Weapon that will deal the damage.
     * Recreate and make it in a way that detects the player's current weapon. !!
     */
    private GameObject weapon;
    Reaction customReaction;
    StatElement damagedByElement;

    private float regularAnimSpeed;

    //Effects variables
    float elementDuration;
    float elementPotency;
    float elementReset;

    // POISON VARIABLES.
    [Tooltip("Smaller numbers, stronger poison")]
    float poisonPotency;
    float poisonReset;
    float poisonDuration;
    float poisonDurationReset;

    [SerializeField] bool poisoned;

    float minPoisonDamage;
    float maxPoisonDamage;

    //ACID VARIABLES.
    [Tooltip("Smaller number, stronger acid")]
    float acidPotency;
    float acidReset;
    float acidDuration;
    float acidDurationReset;

    [SerializeField] bool acidified;

    float minAcidDamage;
    float maxAcidDamage;

    //BACTERIA VARIABLES
    [Tooltip("Smaller number, stronger acid")]
    float feverPotency;
    float feverReset;
    float feverDuration;
    float feverDurationReset;

    [SerializeField] bool fever;

    float minFeverDamage;
    float maxFeverDamage;

    // PARALYZE VARIABLES.
    private float paralyzedTimer;
    [SerializeField] private bool paralyzed;
   

    // PETRIFIED VARIABLES.
    [SerializeField] private bool petrified;

    // BURNED VARIABLES.
    private float burnedTimer;
    [SerializeField] private bool burned;

    // FROZEN VARIABLES.
    private float frozenTimer;
    [SerializeField] private bool frozen;

    // Electrified VARIABLES.
    private float electrifiedTimer;
    [SerializeField] private bool electrified;

    [Header("NPC Control")]

    public bool stayFrozenAfterDialogue = false;

   GameManager manager;

    //AI
   
   MProjectileThrower projThrow;

    //material
   
    public Material matBody;

    Texture poisonTex;
    Texture bloodTex;

    protected PC Pc { get => pc ??= FindAnyObjectByType<PC>(); set => pc = value; }

    private void Awake()
    {
        animal = GetComponent<MAnimal>();
        stats = GetComponent<Stats>();
        damageable = GetComponent<MDamageable>();
        anim = GetComponent<Animator>();
        manager = FindAnyObjectByType<GameManager>();
        blood = GetComponentInChildren<Blood>();
        projThrow = GetComponentInChildren<MProjectileThrower>();
        rb = GetComponent<Rigidbody>();
        poisonTex = Resources.Load<Texture>("Textures/poison");
    }

    private void Start()
    {
       
        //Setup the regular animation speed
        regularAnimSpeed = anim.speed;

        //Initialization for custom children classes
        Initialize();
    }

    public virtual void Initialize()
    {
        //do the override in the custom child class
    }

    private void OnEnable()
    {
        DialogueManager.instance.conversationStarted += OnConversationStarted;
        DialogueManager.instance.conversationEnded += OnConversationEnded;
    }

    #region Called from Battle
    public void OnCauseDamage(float minDamage, float maxDamage, StatElement statElement, float _potency, float _duration)
    {
        string elementName = statElement.DisplayName;
        print("OnCauseDamage(): " + statElement.DisplayName);
        damagedByElement = statElement;

        switch (elementName)
        {
            case "Poison":
                poisoned = true;
                poisonPotency = _potency;
                poisonReset = _potency;
                poisonDuration = _duration;
                poisonDurationReset = _duration;
                minPoisonDamage = minDamage;
                maxPoisonDamage = maxDamage;
                weapon = damageable.LastDamage.Damager;
                //trocar material

                matBody.SetFloat("_BloodIntensity", 1f);
               
                matBody.SetTexture("_BloodMap", poisonTex);
                break;
            case "Acid":
                acidified = true; 
                acidPotency = _potency;
                acidReset = _potency;
                acidDuration = _duration;
                acidDurationReset = _duration;
                minAcidDamage = minDamage;
                maxAcidDamage = maxDamage;
                weapon = damageable.LastDamage.Damager;
                //trocar material
                print("OnCaused Acid Damage");
                matBody.SetFloat("_BloodIntensity", 1f);
                matBody.SetTexture("_BloodMap", poisonTex);
                break;
            case "Bacteria":
                fever = true;
                feverPotency = _potency;
                feverReset = _potency;
                feverDuration = _duration;
                feverDurationReset = _duration;
                minFeverDamage = minDamage;
                maxFeverDamage = maxDamage;
                weapon = damageable.LastDamage.Damager;
                transform.Find("Internal Components").Find("Effects").Find("Fever").gameObject.SetActive(true);
                break;
        }

       
    }

    #endregion

    #region Update Methods

    private void Update()
    {
        Poisoned();
        Acidified();
        Paralyzed();
        Burned();
        Frozen();
        Fever();

        //To be called in children classes
        UpdateNPC();
    }

    public virtual void UpdateNPC()
    {

    }
    void Poisoned()
    {
       
        if (poisoned)
        {
            poisonDuration -= Time.deltaTime;

            if (poisonDuration < 0)
            {
                poisoned = false;
            }

            poisonPotency -= Time.deltaTime;

            if (poisonPotency <= 0)
            {
                StatModifier poisonDamage = new StatModifier();
                poisonDamage.MinValue.Value = minPoisonDamage;
                poisonDamage.MaxValue.Value = maxPoisonDamage;
                damageable.ReceiveDamage(Vector3.forward, weapon, poisonDamage, false, true, customReaction, false);
                poisonPotency = poisonReset;
            }
        }
    }
    void Acidified()
    {
       
        if (acidified)
        {
            if (stats.stats[0].Value <= 0)
            {
                EndAcid();
            }

            acidDuration -= Time.deltaTime;

            if (acidDuration < 0)
            {
                EndAcid();
            }

            acidPotency -= Time.deltaTime;

            if (acidPotency <= 0)
            {
                StatModifier acidDamage = new StatModifier();
                acidDamage.MinValue.Value = minAcidDamage;
                acidDamage.MaxValue.Value = maxAcidDamage;
                //change ID to stamina
                acidDamage.ID = stats.stats[1].ID;
                print("StatID to take acid damage: " + acidDamage.ID.DisplayName);
                customReaction = damageable.reaction;
                             
                print("acidDamage.Value " + acidDamage.Value);
               
                //and cause damage again
               
                damageable.ReceiveDamage(Vector3.forward, weapon, acidDamage, false, true, customReaction, false, damagedByElement);
                acidPotency = acidReset;
            }
        }
    }
    void EndAcid()
    {
        acidified = false;
       
        matBody.SetFloat("_BloodIntensity", 0f);
    }

    void Fever()
    {

        if (fever)
        {
           feverDuration -= Time.deltaTime;

            if (feverDuration < 0)
            {
                EndFever();
            }

            feverPotency -= Time.deltaTime;

            if (feverPotency <= 0)
            {
                StatModifier feverDamage = new StatModifier();
                feverDamage.MinValue.Value = minFeverDamage;
                feverDamage.MaxValue.Value = maxFeverDamage;
                feverDamage.ID = stats.stats[1].ID;
                damageable.ReceiveDamage(Vector3.forward, weapon, feverDamage, false, true, customReaction, false, damagedByElement);
                animal.AnimatorSpeed = 0.5f;
                feverPotency = feverReset;
            }
        }
    }
    void EndFever()
    {
        fever = false;
        animal.AnimatorSpeed = 1f;
        transform.Find("Internal Components").Find("Effects").Find("Fever").gameObject.SetActive(false);
    }

    void Burned()
    {
        //if (burned)
        //{
        //    damageable.ReceiveDamage(Vector3.forward, weapon, damage, false, true, customReaction, false);

        //    burnedTimer -= Time.deltaTime;

        //    if (burnedTimer <= 0)
        //    {
        //        burned = false;
        //        burnedTimer = Random.Range(1, 3);
        //    }
        //}
    }

    void Paralyzed()
    {
        if (paralyzed)
        {
            animal.Sleep = true; 
            elementDuration -= Time.deltaTime;

            if (elementDuration < 0)
            {
                paralyzed = false;
            }

            elementPotency -= Time.deltaTime;

            if (elementPotency <= 0)
            {
                animal.Sleep = false;
                elementPotency = elementReset;
            }
        }
    }

    void Petrified()
    {
        if (petrified)
        {
            animal.State_Force(0);
            animal.Sleep = true;
        }
    }

    public void Unpetrify()
    {
        animal.Sleep = false;
    }

    void Frozen()
    {
        //if (frozen)
        //{
        //    anim.speed = 0;
        //    frozenTimer -= Time.deltaTime;

        //    if (frozenTimer <= 0)
        //    {
        //        frozen = false;
        //        anim.speed = regularAnimSpeed;
        //        frozenTimer = Random.Range(1, 5);
        //    }
        //}
    }

    #endregion

    #region Main events
    /// <summary>
    /// Methods called when taking damage and on death
    /// </summary>
    public void OnHit()
    {
        //print("OnHit() called!");
       
        blood.ActivateBlood();
    }
    public void OnDeath()
    {
        if (animal.State_Get(0).IsActiveState)
        {
            animal.State_Force(10);
        }
        

    }

    public void OnPlayerPerceived()
    {
      
    }

    #endregion

    #region OnConversation
    /// <summary>
    /// Methods called on conversation and for player control during them. These events are called when conversation starts or ends.
    /// </summary>

  
    private void OnConversationStarted(Transform t)
    {
        //This is not a NPC with conversation
        if (GetComponent<DialogueSystemTrigger>() == null) return;

        //This is not the NPC in the active conversation
        if(DialogueManager.instance.activeConversation.conversationTitle != GetComponent<DialogueSystemTrigger>().conversation)
        {
            return;
        }
        print("Conversation started with " + gameObject.name);
        GameManager.Pc.FreezePlayer();
        Freeze();
        CinemachineVirtualCamera MyVirtualCam = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        MyVirtualCam.Priority = 100;
    }

    private void OnConversationEnded(Transform t)
    {
        //This is not a NPC with conversation
        if (GetComponent<DialogueSystemTrigger>() == null) return;

        if (DialogueManager.instance.activeConversation.conversationTitle != GetComponent<DialogueSystemTrigger>().conversation)
        {
            return;
        }
        print("Conversation ended with " + gameObject.name);
        CinemachineVirtualCamera MyVirtualCam = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        MyVirtualCam.Priority = -50;
        GameManager.Pc.UnfreezePlayer();
        if(stayFrozenAfterDialogue == false)
        {
            Unfreeze();
        }
        
    }

    #endregion

    #region NPC Control
    public void Freeze()
    {
        print(gameObject.name + " is frozen");
        animal.State_Force(0);
        //LockInput LockMovement
        animal.LockInput = true;
        animal.LockMovement = true;
        print("Current Gravity Power: " + animal.GravityPower);
       
    }
    public void Unfreeze()
    {
        animal.LockInput = false;
        animal.LockMovement = false;
       
    }

    public void StartConversation(string title)
    {
        DialogueManager.instance.StartConversation(title, Pc.transform, transform);
    }
    #endregion

    #region Special Attacks
    public void ShootPoison()
    {
        //print("SHOOTS POISON");
        projThrow.Fire();
    }
    #endregion

}