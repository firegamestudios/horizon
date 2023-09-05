using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    Animator anim;

    public List<GameObject> toDeactivate;
    public List<GameObject> toActivate;

    AudioSource source;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }
    public void OpenDoor()
    {
        anim.SetBool("isOpen", true);
    }
    public void BreakDoor()
    {
        anim.SetBool("isOpen", true);
        for (int i = 0; i < toDeactivate.Count; i++)
        {
            toDeactivate[i].SetActive(false);
        }
        for (int i = 0; i < toActivate.Count; i++)
        {
            toActivate[i].SetActive(true);
        }

        source.Play();
    }
}
