using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBarPanel : MonoBehaviour
{

    public List<ActionSlot> actionSlots = new List<ActionSlot>();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            actionSlots.Add(transform.GetChild(i).GetComponent<ActionSlot>());
        }
    }

    public void AddFeatToActionBar(string _feat)
    {
       
        //Check all slots
        for(int i = 0;i < actionSlots.Count;i++)
        {
            //If it's empty
            if (IsEmptySlot(actionSlots[i]))
            {
                //Fill with this feat
                actionSlots[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Feats/" + _feat);
                transform.GetChild(i).name = _feat;
                print("Setting up feat in ActionBar UI number: " + _feat);
                break;
            }
        }
    }

    bool IsEmptySlot(ActionSlot slot)
    {
        if(slot.transform.GetComponent<Image>().sprite.name == "Border")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
