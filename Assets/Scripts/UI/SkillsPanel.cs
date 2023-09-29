using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsPanel : MonoBehaviour
{
    
    public List<GameObject> skillPanels;

  

    public void UpdateSkillPanels(string _classe)
    {
        switch (_classe)
        {
            case "Engineer":
                ClearPanels();
                skillPanels[0].SetActive(true);
                break;
            case "Biologist":
                ClearPanels();
                skillPanels[1].SetActive(true);
                break;
            case "Hunter":
                ClearPanels();
                skillPanels[2].SetActive(true);
                break;
            case "Hacker":
                ClearPanels();
                skillPanels[3].SetActive(true);
                break;
            default:
                break;
        }
    }

    void ClearPanels()
    {
        for (int i = 0; i < skillPanels.Count; i++)
        {
            skillPanels[i].gameObject.SetActive(false);
        }
    }

   
}
