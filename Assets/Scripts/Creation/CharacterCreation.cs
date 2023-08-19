using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour
{
    GameObject background;
    public GameObject creationScene;
    public GameObject creationPanel;
    public GameObject ConnectButton;

    private void Awake()
    {
        background = GameObject.Find("Background");
        creationScene = GameObject.Find("Main Camera").transform.Find("Creation Scene").gameObject;
    }

    public void OnCreationEntered()
    {
        background.SetActive(false);
        creationScene.SetActive(true);
        creationPanel.SetActive(true);
    }

    public void BaseSelection(int index)
    {
        PlayerPrefs.SetInt("Base", index);
        ConnectButton.SetActive(true);
        creationScene.SetActive(false);
        creationPanel.SetActive(false);
        background.SetActive(true);
    }
}
