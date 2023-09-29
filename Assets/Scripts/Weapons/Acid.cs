using MalbersAnimations;
using MalbersAnimations.Controller;
using MalbersAnimations.Reactions;
using MalbersAnimations.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    public List<Tag> targetTags;

    public StatModifier damage;

    public Reaction customReaction;

    public StatElement statElement;

    public float timer = 1f;

    float originalMin;
    float originalMax;

    [Tooltip("At what rate it causes damage?")]
    public float acidPotency;
    [Tooltip("How long will this acid last?")]
    public float acidDuration;

    MAnimal animal;
    void Start()
    {
        originalMin = damage.MinValue.Value;
        originalMax = damage.MaxValue.Value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NPC>() != null)
        {
            NPC npc = other.GetComponent<NPC>();
            damage.MinValue.Value = originalMin;
            damage.MaxValue.Value = originalMax;
            npc.OnCauseDamage(damage.MinValue.Value, damage.MaxValue.Value, statElement, acidPotency, acidDuration);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            if (other.GetComponent<MDamageable>() != null)
            {
                if (other.GetComponent<NPC>() != null)
                {
                    NPC npc = other.GetComponent<NPC>();
                    damage.MinValue.Value = originalMin;
                    damage.MaxValue.Value = originalMax;
                    
                    npc.OnCauseDamage(damage.MinValue.Value, damage.MaxValue.Value, statElement, acidPotency, acidDuration);
                }
                else if(other.GetComponent<PC>() != null)
                {
                    MDamageable damageable = other.GetComponent<MDamageable>();
                    damage.MinValue.Value = originalMin;
                    damage.MaxValue.Value = originalMax;
                    //damageable.ReceiveDamage(Vector3.forward, gameObject, damage, false, false, customReaction, true);
                    damageable.ReceiveDamage(Vector3.forward, gameObject, damage, false, true, customReaction, false, statElement);
                    print("Current minValue: " + damage.MinValue.Value + " and MaxValue: " + damage.MaxValue.Value);
                }
                else
                {
                    //do nothing
                }

            }
            timer = Random.Range(1, 2);
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.name);
    }
}
