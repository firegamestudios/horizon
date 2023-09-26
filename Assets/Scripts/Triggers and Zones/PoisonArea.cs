using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PC>() != null)
        {
            PC pc = other.GetComponent<PC>();
            pc.OnPoisoned(1f, 60f);
        }
    }
}
