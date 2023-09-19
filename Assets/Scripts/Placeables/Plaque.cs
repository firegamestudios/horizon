using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plaque : MonoBehaviour
{
    [TextArea(3,3)]
    public string message;

    UIManager manager;
    AudioSource source;

    private void Awake()
    {
        manager = FindAnyObjectByType<UIManager>();
        source = GetComponent<AudioSource>();
    }

    public void ReadPlaque()
    {
        manager.TransformMessage(message, transform);
        source.Play();
    }
}
