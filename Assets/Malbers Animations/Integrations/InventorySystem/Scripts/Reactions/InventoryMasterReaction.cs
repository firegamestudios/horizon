using MalbersAnimations.Reactions;
using System;

namespace MalbersAnimations.InventorySystem
{
    /// <summary> Reaction Script For Inventory Master</summary>
    [System.Serializable]
    public abstract class InventoryMasterReaction : Reaction
    {
        public override Type ReactionType => typeof(InventoryMaster);
    }
}