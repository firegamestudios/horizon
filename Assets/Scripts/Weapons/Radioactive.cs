using MalbersAnimations;
using MalbersAnimations.Reactions;
using MalbersAnimations.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void Start()
    {
        originalMin = damage.MinValue.Value;
        originalMax = damage.MaxValue.Value;
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
            //if other has tags
            if (other.GetComponent<Tags>() != null)
            {
                Tags otherTags = other.GetComponent<Tags>();

                for (int i = 0; i < targetTags.Count; i++)
                {
                    if (otherTags.tags[0].DisplayName == targetTags[i].DisplayName)
                    {
                        if (other.GetComponent<MDamageable>() != null)
                        {
                            MDamageable damageable = other.GetComponent<MDamageable>();
                            damage.MinValue.Value = originalMin;
                            damage.MaxValue.Value = originalMax;
                            damageable.ReceiveDamage(Vector3.forward, gameObject, damage, false, true, customReaction, false, statElement);
                            print("Current minValue: " + damage.MinValue.Value + " and MaxValue: " + damage.MaxValue.Value);
                        }
                    }
                }
            }
            timer = Random.Range(1,2);
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.name);
    }
}
