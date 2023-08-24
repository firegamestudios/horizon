using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This script goes in the loading scene
/// </summary>

public class LoadingScreenController : MonoBehaviour
{
    public Image loadingBackground;
    public Image progressBar;
    public TMP_Text tipText;

    Transform wp;

    public List<string> tips;

    public List<Sprite> bgs;

    // Static variable to store the name of the scene to load
    public static string sceneToLoad;


    private void Awake()
    {
        wp = GameObject.Find("WP").transform;
    }
    void Start()
    {
        SetupTooltip();
        // Start loading the scene
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    void SetupTooltip()
    {
        int dice = Random.Range(0, wp.childCount);
        tipText.text = tips[dice];
        wp.GetChild(dice).gameObject.SetActive(true);


    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Start loading the scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // While the scene is still loading:
        while (!asyncOperation.isDone)
        {
            // Update the progress bar
            progressBar.fillAmount = asyncOperation.progress;

            // Yield control to the runtime system for one frame
            yield return null;
        }
    }
}
