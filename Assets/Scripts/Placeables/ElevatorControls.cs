using MalbersAnimations.Scriptables;
using MalbersAnimations.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorControls : MonoBehaviour
{
    MSimpleTranslator translator;

    public Vector3Reference TopPosition;
    public Vector3Reference BottomPosition;

    private void Awake()
    {
        translator = GetComponentInParent<MSimpleTranslator>();
    }
    public void ActivateElevator()
    {
        if(translator.start.Value.y == 0)
        {
            translator.start = BottomPosition;
            translator.end = TopPosition;
            translator.enabled = false;
            translator.enabled = true;
        }
        else
        {
            translator.start = TopPosition;
            translator.end = BottomPosition;
            translator.enabled = false;
            translator.enabled = true;
        }
    }
}
