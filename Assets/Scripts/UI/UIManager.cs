using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MalbersAnimations.UI;
using UnityEngine.UI;
using MoreMountains.InventoryEngine;
using MalbersAnimations;
using Cinemachine;

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

    [HideInInspector]
    public InventoryDisplay inventoryDisplay;

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
            malbersInput.enabled = false;
            brain.enabled = false;
        }
        else
        {
            malbersInput.enabled = true;
            brain.enabled = true;
        }
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

    #region Inventory
    public void UpdateInventoryDisplay()
    {
        inventoryDisplay.RedrawInventoryDisplay();
    }
    #endregion
}
