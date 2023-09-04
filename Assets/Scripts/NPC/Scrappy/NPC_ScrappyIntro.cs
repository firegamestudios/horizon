using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using PixelCrushers.DialogueSystem;

public class NPC_ScrappyIntro : NPC
{
    public AudioSource screamSound;
    public GameObject scrappy;
    public Transform wpIncinerator;

    public Animator incineratorDoorAnimator;
    public AudioSource doorAudio;

    public override void Initialize()
    {
        base.Initialize();
        Freeze();
    }

    //First encounter
    public void SavesFromIncinerator()
    {
        DialogueLua.SetVariable("ScrappySaved", "true");
        //print(DialogueLua.GetVariable("ScrappySaved").asString);
        gameObject.SetActive(false);
        scrappy.SetActive(true);
    }

    public void FallsInIncinerator()
    {
        DialogueLua.SetVariable("ScrappyFell", "true");
        print("ScrappyIntro: " + DialogueLua.GetVariable("ScrappyIntro").asString);
        print("ScrappyFell: " + DialogueLua.GetVariable("ScrappyFell").asString);
        screamSound.Play();
        Unfreeze();
    }

    #region Update
    public override void UpdateNPC()
    {
        base.UpdateNPC();
        if(DialogueLua.GetVariable("ScrappyFell").asString ==  "true")
        {
            if(incineratorDoorAnimator.GetBool("isOpen") == true)
            {
                scrappy.transform.position = wpIncinerator.position;
                scrappy.SetActive (true);
                doorAudio.Play();
                gameObject.SetActive(false);
            }
        }
    }
    #endregion
}
