using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletTimeManager : MonoBehaviour
{
    
    public float bulletTimeDuration;

    private bool isBulletTimeActive = false;

    AudioSource source;
    public List<AudioClip> hitClips;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

  

    public void BulletTime(int index)
    {
        int d3;
        switch (index)
        {
            //Kick
            case 0:
                d3 = Random.Range(0, 3);
                source.clip = hitClips[d3];
                source.Play();

                if (!isBulletTimeActive)
                {
                    StartCoroutine(ActivateBulletTime());
                }
                break;
                //Punch
            case 1:
                d3 = Random.Range(0, 3);
                source.clip = hitClips[d3];
                source.Play();
                break;
            default:
                break;
        }
       
    }

    private IEnumerator ActivateBulletTime()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        isBulletTimeActive = true;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(bulletTimeDuration);

        Time.timeScale = 1.0f; // Reset time scale
        isBulletTimeActive = false;
    }
}
