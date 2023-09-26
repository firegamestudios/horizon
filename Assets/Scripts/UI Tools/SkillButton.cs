using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    CreationManager creationManager;

    private void Awake()
    {
        creationManager = FindAnyObjectByType<CreationManager>();
    }

   
    public void OnPointerClick(PointerEventData eventData)
    {
       
            creationManager.AddFeat(gameObject.name);

    }
}
