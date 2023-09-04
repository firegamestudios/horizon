using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void OpenDoor()
    {
        anim.SetBool("isOpen", true);
    }
}
