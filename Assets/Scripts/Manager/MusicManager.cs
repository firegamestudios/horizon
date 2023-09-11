using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;
using MalbersAnimations.Controller;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    public float combatDistance;

    public GameObject[] animalObjs;
    public List<MAnimal> enemies = new();
    PC pc;

    public Tag mTag;

    float timer = 5;
    float reset;

    AudioSource mAudioSource;
    public AudioClip mAudioClip;
    public List<AudioClip> battleClips;

    float fadeDuration = 3f;
    public float currentFadeTime;
    public bool isFadingOut;
    float startVolume = 1;


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

        EndBattle();
    }



    public void SetupBattle()
    {
        if (mAudioSource.clip == mAudioClip)
        {
            FadeOutMusic(); // Start fading out the current music
            StartCoroutine(ChangeMusicClip()); // Wait for the fade-out, then change the clip and fade in
        }
    }

    private IEnumerator ChangeMusicClip()
    {
        yield return new WaitForSeconds(fadeDuration); // Wait for the fade-out to complete
        int dice = Random.Range(0, battleClips.Count);
        mAudioSource.clip = battleClips[dice];
        mAudioSource.Play();
        FadeInMusic(); // Start fading in the new music
    }

    public void EndBattle()
    {
        if (mAudioSource.clip != mAudioClip)
        {
            FadeOutMusic(); // Start fading out the current music
            StartCoroutine(ResetMusicClip()); // Wait for the fade-out, then reset the clip and fade in
        }
    }

    private IEnumerator ResetMusicClip()
    {
        yield return new WaitForSeconds(fadeDuration); // Wait for the fade-out to complete
        mAudioSource.clip = mAudioClip;
        mAudioSource.Play();
        FadeInMusic(); // Start fading in the original music
    }

    // Fade out the current music
    private void FadeOutMusic()
    {
        isFadingOut = true;
        currentFadeTime = 0.0f;
    }

    // Fade in the new music
    private void FadeInMusic()
    {
        isFadingOut = false;
        currentFadeTime = 0.0f;
    }

    private void FixedUpdate()
    {
        // Apply audio fading if necessary
        if (isFadingOut && currentFadeTime <= fadeDuration)
        {
            // Calculate the fade factor
            float fadeFactor = currentFadeTime / fadeDuration;

            // Apply the fading effect
            mAudioSource.volume = Mathf.Lerp(startVolume, 0.0f, fadeFactor);

            // Increment the fade time
            currentFadeTime += Time.fixedDeltaTime;
        }
        else if (!isFadingOut && currentFadeTime <= fadeDuration)
        {
            // Calculate the fade factor
            float fadeFactor = currentFadeTime / fadeDuration;

            // Apply the fading effect
            mAudioSource.volume = Mathf.Lerp(0.0f, startVolume, fadeFactor);

            // Increment the fade time
            currentFadeTime += Time.fixedDeltaTime;
        }
    }

}
