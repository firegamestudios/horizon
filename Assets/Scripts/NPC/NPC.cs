using MalbersAnimations;
using MalbersAnimations.Controller;
using MalbersAnimations.Reactions;
using MalbersAnimations.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //Components
    MAnimal animal;
   
    MDamageable damageable;
    Reaction customReaction;
    Animator anim;



    /* -> Weapon that will deal the damage.
     * Recreate and make it in a way that detects the player's current weapon. !!
     */
    private GameObject weapon;

    private float regularAnimSpeed;

    // POISON VARIABLES.
    [Tooltip("Smaller numbers, stronger poison")]
    float poisonPotency;
    float poisonReset;
    float poisonDuration;
    float poisonDurationReset;
    
    [SerializeField] private bool poisoned;
    StatModifier poisonDamage;

    // PARALYZE VARIABLES.
    private float paralyzedTimer;
    [SerializeField] private bool paralyzed;

    // BURNED VARIABLES.
    private float burnedTimer;
    [SerializeField] private bool burned;

    // FROZEN VARIABLES.
    private float frozenTimer;
    [SerializeField] private bool frozen;

    private void Start()
    {
        damageable = GetComponent<MDamageable>();
        anim = GetComponent<Animator>();

        regularAnimSpeed = anim.speed;
    }
    public void OnCauseDamage(StatModifier _poisonDamage, StatElement statElement, float _poisonPotency, float _poisonDuration)
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
            animal.Sleep = true; // Não está funcionando.
            paralyzedTimer -= Time.deltaTime;

            if (paralyzedTimer <= 0)
            {
                paralyzed = false;
                paralyzedTimer = Random.Range(1, 5);
            }
        }
    }

    void Frozen()
    {
        if (frozen)
        {
            anim.speed = 0;
            frozenTimer -= Time.deltaTime;

            if (frozenTimer <= 0)
            {
                frozen = false;
                anim.speed = regularAnimSpeed;
                frozenTimer = Random.Range(1, 5);
            }
        }
    }

    #endregion

    #region OnHit and OnDeath
    public void OnHit()
    {

    }
    #endregion

    #region Effect of Elements

  

  

    #endregion
}