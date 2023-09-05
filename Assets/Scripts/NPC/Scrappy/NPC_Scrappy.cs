using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using MalbersAnimations.Controller.AI;

public class NPC_Scrappy : NPC
{
    MAnimalBrain brain;
    MAnimalAIControl control;
    public List<MAIState> states;

    public override void Initialize()
    {
        base.Initialize();
        //Find
        brain = GetComponentInChildren<MAnimalBrain>();
        control = GetComponentInChildren<MAnimalAIControl>();
        print("ScrappySaved: " + DialogueLua.GetVariable("ScrappySaved").asString);
        DialogueManager.instance.StartConversation("Scrappy", transform, pc.transform);
    }

    #region Ally Commands
    public void StayHere()
    {
        brain.currentState = states[0];
        control.Target = null;
    }
    #endregion
}
