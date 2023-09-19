using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;
using Droidzone.Core;

namespace MoreMountains.InventoryEngine
{	
	[CreateAssetMenu(fileName = "SpeedBonusItem", menuName = "MoreMountains/InventoryEngine/SpeedBonusItem", order = 1)]
	[Serializable]
	/// <summary>
	/// Demo class for a health item
	/// </summary>
	public class SpeedBonusItem : InventoryItem 
	{
		[Header("Speed Bonus")]
		/// the amount of health to add to the player when the item is used
		public int SpeedBonus;
		public float TimeBonus;

        /// <summary>
        /// What happens when the object is used 
        /// </summary>
        public override bool Use(string playerID)
		{
			base.Use(playerID);
			// This is where you would increase your character's health,
			// with something like : 
			// Player.Life += HealthValue;
			// of course this all depends on your game codebase.

			GameManager.Pc.Speed(SpeedBonus, TimeBonus);
          
            Debug.LogFormat("increase character "+playerID+"'s speed by "+ SpeedBonus);
			return true;
		}
		
	}
}