namespace MalbersAnimations.InventorySystem
{
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "Malbers Animations/ID/Item Type", fileName = "New Item Type", order = -1000)]
    public class ItemType : IDs
    {
       public bool Stackable;

        #region CalculateID
#if UNITY_EDITOR
        private void Reset() => GetID();

        [UnityEngine.ContextMenu("Get ID")]
        private void GetID() => FindID<ItemType>();
#endif
        #endregion
    }
}

