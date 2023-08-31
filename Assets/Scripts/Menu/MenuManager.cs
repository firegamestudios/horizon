using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void OnResumeClicked()
    {

    }
    public void OnNewGameClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuitGameClicked()
    {

    }
}
