using Droidzone.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TooltipButton doesn't have Start and Update, only OnPointerEnter and Exit
/// </summary>

public class TooltipXP : TooltipButton
{
   
    private void Update()
    {
        if(GameManager.Pc != null)
        {
            title = "Experience";
            description = "Current XP: " + GameManager.Pc.PlayerData.XP.ToString() + " / Needed for next level: " + GameManager.Pc.nextLevelXP.ToString();
        }

      
    }
}
