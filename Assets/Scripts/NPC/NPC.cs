using Cinemachine;
using MalbersAnimations;
using MalbersAnimations.Controller;
using MalbersAnimations.Reactions;
using MalbersAnimations.Weapons;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //My Components
    protected MAnimal animal;
    MDamageable damageable;
    Reaction customReaction;
    Animator anim;
   
    //The player
    protected PC pc;
   

    /* -> Weapon that will deal the damage.
     * Recreate and make it in a way that detects the player's current weapon. !!
     */
    private GameObject weapon;

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

    bool poisoned;
    
    float minPoisonDamage;
    float maxPoisonDamage;

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

    
    private void Awake()
    {
        animal = GetComponent<MAnimal>();
        pc = FindAnyObjectByType<PC>();
        damageable = GetComponent<MDamageable>();
        anim = GetComponent<Animator>();
       
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


    public void OnCauseDamage(float minDamage, float maxDamage, StatElement statElement, float _poisonPotency, float _poisonDuration)
    {
        string elementName = statElement.DisplayName;

        poisonPotency = _poisonPotency;
        poisonReset = _poisonPotency;
        poisonDuration = _poisonDuration;
        poisonDurationReset = _poisonDuration;
        
        switch (elementName)
        {
            case "Poison":
                poisoned = true;
                break;
        }
    }

    #region Update Methods

    private void Update()
    {
        Poisoned();
        Paralyzed();
        Burned();
        Frozen();

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

    #region OnHit and OnDeath
    /// <summary>
    /// Methods called when taking damage and on death
    /// </summary>
    public void OnHit()
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
        pc.FreezePlayer();
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
        pc.UnfreezePlayer();
        Unfreeze();
    }

    #endregion

    #region NPC Control
    public void Freeze()
    {
        animal.State_Force(0);
        animal.Sleep = true;
    }
    public void Unfreeze()
    {
        animal.Sleep = false;
    }

    public void StartConversation(string title)
    {
        DialogueManager.instance.StartConversation(title, pc.transform, transform);
    }
    #endregion

}