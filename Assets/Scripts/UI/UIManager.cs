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
using Droidzone.Core;

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

    public Image xpBar;

    //feats
    public GameObject featButtonPrefab;
    public Transform featsGrid;
    public ActionBar actionBar;

    //Ai Inworld
    public GameObject globalChatCanvas;
    public AudioCapture audioCapture;
    public GameObject aiCanvas;
    float aiCanvasTimer = 5f;
    float aiCanvasTimerReset = 5f;

    [HideInInspector]
    public InventoryDisplay inventoryDisplay;
    public InventoryInputManager inventoryInputManager;

    
    PlayerData playerData;
   
    public List<TMP_Text> attributeTexts;

    public List<TMP_Text> skillTexts;

    
    private void Update()
    {
       /// UI Manager is responsible for the control of player input

       //Inventory priority
        if (inventoryDisplay.IsOpen)
        {
            //InputAndCamera(false, false);
            FreezePlayer();
        }
        else
        {
            //if Inventory Closed, Check if Dialogue Box is open and freeze
            if(globalChatCanvas.activeInHierarchy == true)
            {
                FreezePlayer();

                inventoryInputManager.enabled = false;

                if(aiCanvas.activeInHierarchy == false)
                {
                    aiCanvas.SetActive(true);
                }
                
            }
            else
            {
                UnfreezePlayer();
                inventoryInputManager.enabled = true;
                aiCanvasTimer -= Time.deltaTime;
                if(aiCanvasTimer < 0)
                {
                    aiCanvasTimer = aiCanvasTimerReset;
                    aiCanvas.SetActive(false);
                }
            }

        }

      
    }

    void FreezePlayer()
    {
        GameManager.Pc.FreezeCamera(false);
        GameManager.Pc.FreezePlayer();
    }
    void UnfreezePlayer()
    {
        GameManager.Pc.FreezeCamera(true);
        GameManager.Pc.UnfreezePlayer();
    }
    #region SetupPlayer
    public void SetupPlayerName(string myName, string myClass, string myRace)
    {
        playerNameText.text = myName + ", " + myRace + " " + myClass;
    }

    public void SetupAttributes(PlayerData _playerData)
    {
        playerData = _playerData;
        attributeTexts[0].text = "Strength: " + playerData.attributes[0];
        attributeTexts[1].text = "Agility: " + playerData.attributes[1];
        attributeTexts[2].text = "Endurance: " + playerData.attributes[2];
        attributeTexts[3].text = "Crafting: " + playerData.attributes[3];
        attributeTexts[4].text = "Computer: " + playerData.attributes[4];

        //skills
        skillTexts[0].text = "Melee Damage: " + playerData.MeleeDamage.ToString();
        skillTexts[1].text = "Ranged Damage: " + playerData.RangedDamage.ToString();
        skillTexts[2].text = "Hacking: " + playerData.Hacking.ToString();
        skillTexts[3].text = "Healing: " + playerData.Healing.ToString();
        skillTexts[4].text = "Leadership: " + playerData.Leadership.ToString();
        skillTexts[5].text = "Genetic Engineering: " + playerData.GenEngineering.ToString();
        skillTexts[6].text = "Piloting: " + playerData.Piloting.ToString();
        skillTexts[7].text = "Tracking: " + playerData.Tracking.ToString();
        skillTexts[8].text = "Taming: " + playerData.Taming.ToString();
        skillTexts[9].text = "Hack Lock: " + playerData.HackLock.ToString();

        if(GameManager.Pc.nextLevelXP > 0)
        {
            print("fillAmount = " + playerData.XP / GameManager.Pc.nextLevelXP);
            xpBar.fillAmount = playerData.XP / GameManager.Pc.nextLevelXP;
        }

        //add feats to sheet and action bar
        for (int i = 0; i < playerData.feats.Length; i++)
        {
            GameObject newIcon = Instantiate(featButtonPrefab, featsGrid);
            newIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Feats/" + playerData.feats[i]);
            newIcon.name = playerData.feats[i].ToString();
            newIcon.GetComponent<TooltipFeat>().SetupFeatTooltip();
            AddFeatToActionBar(newIcon.name);
        }
      
    }

    public void AddFeatToActionBar(string _feat)
    {
        for (int i = 0; i < actionBar.transform.childCount; i++)
        {
            if(actionBar.transform.GetChild(i).name == "None")
            {
               Image actionBarIcon = actionBar.transform.GetChild(i).GetComponent<Image>();
                actionBarIcon.sprite = Resources.Load<Sprite>("Feats/" + playerData.feats[i]);
                actionBarIcon.name = _feat;
                actionBarIcon.GetComponent<TooltipFeat>().SetupFeatTooltip();
                break;
            }
        }
    }

    #endregion

   

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
        float timer = (float)message.Length / 5;
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
