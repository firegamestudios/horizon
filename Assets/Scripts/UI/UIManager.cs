using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MalbersAnimations.UI;
using UnityEngine.UI;

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
}
