using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;
using MalbersAnimations.Controller;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    public GameObject[] animalObjs;
    public List<MAnimal> enemies = new();
    PC pc;

    public Tag mTag;

    float timer = 14;
    float reset;

    AudioSource mAudioSource;
    public AudioClip mAudioClip;
    public List<AudioClip> battleClips;

    private void Awake()
    {
        pc = FindAnyObjectByType<PC>();
        mAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        reset = timer;

        animalObjs =  GameObject.FindGameObjectsWithTag("Animal");

        for (int i = 0; i < animalObjs.Length; i++)
        {
            if (animalObjs[i].GetComponent<Tags>() != null)
            {
                if (animalObjs[i].GetComponent<Tags>().HasTag(mTag))
                {
                    enemies.Add(animalObjs[i].GetComponent<MAnimal>());
                }
                
            }
           
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if( timer < 0)
        {
            CheckIfBattle();
            timer = reset;
        }
    }

    void CheckIfBattle()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            float distance = Vector3.Distance(pc.transform.position, enemies[i].transform.position);

            if(distance < 20f)
            {
                if(mAudioSource.clip != mAudioClip) { return; }

                int dice = Random.Range(0, battleClips.Count);
                mAudioSource.clip = battleClips[dice];
                mAudioSource.Play();
                print("ENEMY CLOSE: " + distance + " meters");
            }
        }
    }

}
