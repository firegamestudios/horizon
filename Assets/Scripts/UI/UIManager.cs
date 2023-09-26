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
using System.Linq;

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
    PlayerData playerData;

    public List<TMP_Text> attributesTexts;

    public List<TMP_Text> skillsTexts;

    public Transform featsGrid;
    public GameObject featPrefab;

    public ActionBarPanel actionBarPanel;

    //Holosign
    public GameObject holosignUI;
    public TMP_Text holosignText;

    public GameObject crossHair;

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
    public void SetupAttributes(int[] _atts)
    {
        attributesTexts[0].text = "Strength: " + _atts[0].ToString();
        attributesTexts[1].text = "Agility: " + _atts[1].ToString();
        attributesTexts[2].text = "Endurance: " + _atts[2].ToString();
        attributesTexts[3].text = "Crafting: " + _atts[3].ToString();
        attributesTexts[4].text = "Computer: " + _atts[4].ToString();
    }
    public void SetupSkills(PlayerData _playerData)
    {
        skillsTexts[0].text = "Melee Damage: " + _playerData.MeleeDamage.ToString();
        skillsTexts[1].text = "Ranged Damage: " + _playerData.RangedDamage.ToString();
        skillsTexts[2].text = "Hacking: " + _playerData.Hacking.ToString();
        skillsTexts[3].text = "Healing: " + _playerData.Healing.ToString();
        skillsTexts[4].text = "Leadership: " + _playerData.Leadership.ToString();
        skillsTexts[5].text = "Genetic Engineering: " + _playerData.GenEngineering.ToString();
        skillsTexts[6].text = "Piloting: " + _playerData.Piloting.ToString();
        skillsTexts[7].text = "Tracking: " + _playerData.Tracking.ToString();
        skillsTexts[8].text = "Taming: " + _playerData.Taming.ToString();
        skillsTexts[9].text = "Hack Lock: " + _playerData.HackLock.ToString();

        playerData = _playerData;

    }

    public void SetupFeats()
    {
        for (int i = 0; i < playerData.feats.Length; i++)
        {
            print("Setting up feat in Inventory UI number: " + i.ToString());
            GameObject featGameObject = Instantiate(featPrefab, Vector3.zero, Quaternion.identity, featsGrid);
            featGameObject.name = playerData.feats[i];
            featGameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Feats/"+featGameObject.name);
            featGameObject.GetComponent<FeatTooltipButton>().SetupThisTooltip();
            //try to add icon to action bar
            actionBarPanel.AddFeatToActionBar(featGameObject.name);
        }
    }
    #endregion

    #region Messages

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
    #endregion

    #region Inventory
    public void UpdateInventoryDisplay()
    {
        inventoryDisplay.RedrawInventoryDisplay();
    }
    #endregion

    #region Holosign
    public void Holosign(string myText)
    {
        holosignText.text = myText;
        holosignUI.SetActive(true);
    }
    public void CloseHolosign()
    {
        holosignUI.SetActive(false);
        EnableCrosshair(true);
    }
    #endregion

    #region Generic Tools
    public void EnableCrosshair(bool on)
    {
        crossHair.SetActive(on);
    }
    #endregion
}
