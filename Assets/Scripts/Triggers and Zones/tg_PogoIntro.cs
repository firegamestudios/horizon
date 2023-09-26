using Inworld;
using MalbersAnimations;
using MalbersAnimations.Controller.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tg_PogoIntro : MonoBehaviour
{

    public InworldCharacter inworldCharacter;
    public GameObject aiCanvas;
    public Tag playerTag;

    public MAIState aiState;
    public MAnimalBrain animalBrain;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Pogo Intro") == 1)
        {
            animalBrain.currentState = aiState;
            //Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Tags>() != null)
        {

            if (other.GetComponent<Tags>().tags[0] == playerTag)
            {
                print("ENTERED POGO TRIGGER");
                inworldCharacter.SendTrigger("intro");
                aiCanvas.SetActive(true);
                animalBrain.currentState = aiState;
                PlayerPrefs.SetInt("Pogo Intro", 1);
                Destroy(gameObject);
            }
        }

    }

}
