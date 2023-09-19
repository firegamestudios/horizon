using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MalbersAnimations.UI;
using UnityEngine.UI;
using MoreMountains.InventoryEngine;
using MalbersAnimations;
using Cinemachine;
using Inworld;

public class UIManager : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text energyText;
    public TMP_Text resistanceText;

    public GameObject diceResultObj;
    public TMP_Text diceResultText;
    public UIFollowTransform uiFollowTransform;
    public Image resultPanel;

    public TMP_Text systemMessageText;
    public Animator sysMessageAnim;

    public TMP_Text transformMessageText;
    public Animator transMessageAnim;

    public TMP_Text playerNameText;

    //Ai Inworld
    public GameObject globalChatCanvas;
    public AudioCapture audioCapture;
    public GameObject aiCanvas;
    float aiCanvasTimer = 5f;
    float aiCanvasTimerReset = 5f;

    [HideInInspector]
    public InventoryDisplay inventoryDisplay;
    public InventoryInputManager inventoryInputManager;

    public PC pc;
    MalbersInput malbersInput;
    public CinemachineBrain brain;

    private void Awake()
    {
       malbersInput = pc.GetComponent<MalbersInput>();
    }

    private void Update()
    {
       //Inventory priority
        if (inventoryDisplay.IsOpen)
        {
            InputAndCamera(false, false);
        }
        else
        {
            //if Inventory Closed, Check Dialogue Box
            if(globalChatCanvas.activeInHierarchy == true)
            {
                InputAndCamera(false, false);
                inventoryInputManager.enabled = false;

                if(aiCanvas.activeInHierarchy == false)
                {
                    aiCanvas.SetActive(true);
                }
                
            }
            else
            {
                InputAndCamera(true, true);
                inventoryInputManager.enabled = true;
                aiCanvasTimer -= Time.deltaTime;
                if(aiCanvasTimer < 0)
                {
                    aiCanvasTimer = aiCanvasTimerReset;
                    aiCanvas.SetActive(false);
                }
            }

        }

        //AudioCapture push to Talk
        if(Input.GetKey(KeyCode.Y))
        {
            audioCapture.enabled = true;
        }
        else
        {
            if(audioCapture.enabled == true)
            {
                audioCapture.enabled = false;
            }
           
        }
    }

    #region SetupPlayer
    public void SetupPlayerName(string myName, string myClass, string myRace)
    {
        playerNameText.text = myName + ", " + myRace + " " + myClass;
    }
    #endregion

    public void InputAndCamera(bool _input, bool _camera)
    {
        malbersInput.enabled = _input;
        brain.enabled = _camera;
    }

    public void AutoMessage(string message)
    {
        systemMessageText.text = message;
        sysMessageAnim.SetBool("on", true);
        float timer = (float)message.Length/2;
        StartCoroutine(ClearAutoMessage(timer));
    }

    IEnumerator ClearAutoMessage(float timer)
    {
        yield return new WaitForSeconds(timer);
        sysMessageAnim.SetBool("on", false);
    }

    public void TransformMessage(string message, Transform trans)
    {
        transformMessageText.text = message;
        transMessageAnim.SetBool("on", true);
        transMessageAnim.transform.GetComponent<TransformMessage>().transformInWorld = trans;
        float timer = (float)message.Length / 3;
        StartCoroutine(ClearTransMessage(timer));
    }

    public void ClearTransformMessage()
    {
        transMessageAnim.SetBool("on", false);
        transMessageAnim.transform.GetComponent<TransformMessage>().transformInWorld = null;
    }
    IEnumerator ClearTransMessage(float timer)
    {
        yield return new WaitForSeconds(timer);
        transMessageAnim.SetBool("on", false);
        transMessageAnim.transform.GetComponent<TransformMessage>().transformInWorld = null;
    }
    #region Inventory
    public void UpdateInventoryDisplay()
    {
        inventoryDisplay.RedrawInventoryDisplay();
    }
    #endregion
}
