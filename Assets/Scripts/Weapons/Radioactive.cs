using MalbersAnimations;
using MalbersAnimations.Controller;
using MalbersAnimations.Reactions;
using MalbersAnimations.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radioactive : MonoBehaviour
{
    public List<Tag> targetTags;

    public StatModifier damage;

    Reaction customReaction;

    public StatElement statElement;

    public float timer = 1f;

    float originalMin;
    float originalMax;

    MAnimal animal;

    public float potency;
    public float duration;

    private void Start()
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
            npc.OnCauseDamage(damage.MinValue.Value, damage.MaxValue.Value, statElement, potency, duration);
        }

    }

    /// <summary>
    /// OnTriggerStay will cause radioactive damage over time
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            if (other.GetComponent<NPC>() != null)
            {
                NPC npc = other.GetComponent<NPC>();
                damage.MinValue.Value = originalMin;
                damage.MaxValue.Value = originalMax;
                npc.OnCauseDamage(damage.MinValue.Value, damage.MaxValue.Value, statElement, potency, duration);
            }
            else if (other.GetComponent<PC>() != null)
            {
                MDamageable damageable = other.GetComponent<MDamageable>();
                damage.MinValue.Value = originalMin;
                damage.MaxValue.Value = originalMax;
                damageable.ReceiveDamage(Vector3.forward, gameObject, damage, false, true, customReaction, false, statElement);
                print("Current minValue: " + damage.MinValue.Value + " and MaxValue: " + damage.MaxValue.Value);
            }
            else
            {
                //do nothing
            }
            timer = Random.Range(1,2);
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.name);
    }
}
