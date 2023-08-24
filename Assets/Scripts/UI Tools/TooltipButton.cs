using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string title;
    [TextArea(3,4)]
    public string description;

    public Vector2 offset;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Tooltip.active)
        {
            Tooltip.SetupTooltip(title, description, offset);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Tooltip.active)
            Tooltip.ClearTooltip();
    }
}
