using UnityEngine;
using MalbersAnimations.Controller;
using MalbersAnimations.Scriptables;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MalbersAnimations.InventorySystem
{
    [AddComponentMenu("Malbers/Inventory/Inventory Item")]
    public class InventoryItem : MonoBehaviour
    {
        //public TransformReference inventory  = new();
        [Tooltip("The type of Inventory the item will be stored")]
        public Item inventoryItem;
        [Tooltip("The amount of items of the same type to add to the inventory when picked")]
        public IntReference quantity = new IntReference(1);

        public int Quantity => quantity.Value;


        private Pickable IsPickable;
        //private InventoryMaster master;

        private void OnEnable()
        {
            if (IsPickable == null)
            {
                IsPickable = GetComponent<Pickable>();
            }

            //if (inventory.Value != null)
            //    master = inventory.Value.GetComponent<InventoryMaster>(); //find the Inventory Master

            IsPickable?.OnPicked?.AddListener(OnItemPicked);

            //print("Fired OnItemPicked Listener");
        }
        private void OnDisable()
        {
            IsPickable?.OnPicked?.RemoveListener(OnItemPicked);
            print("Fired OnDisable OnItemPicked");
        }

        /// <summary>  Listen to the Pick Event on the Pickable and Find the I </summary>
        /// <param name="Picker"></param>
        private void OnItemPicked(GameObject Picker)
        {
            //master?.AddItemToInventory(inventoryItem, Quantity);
 

            //Old way
            InventoryMaster invMaster = FindAnyObjectByType<InventoryMaster>();
          //  var invMaster = Picker.GetComponent<InventoryMaster>();
            invMaster?.AddItemToInventory(inventoryItem, Quantity);

            print("Fired OnItemPicked()");

            //if (Picker.TryGetComponent<InventoryItem>(out var inventory))
            //{
            //    Picker.gameObject.TryGetComponent<InventoryMaster>(out var tempInv);
            //    tempInv.AddItemToInventory(inventoryItem, Quantity);
            //}

        }
        private void Reset()
        {
            //inventory.UseConstant = false;
            //inventory.Variable = MTools.GetInstance<TransformVar>("Inventory Master");
        }


        //#region HiddenVariables
        ////Hidden Variables
        //[HideInInspector, SerializeField] private int Editor_Tabs1;
        //#endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(InventoryItem)), CanEditMultipleObjects]
    public class InventoryItemEditor : Editor
    {
        private SerializedProperty 
           // isUsingMalbersInventory, OnItemUsed, OnItemDropped, OnItemEquipped, OnItemUnEquipped, OnItemRemoved,
            inventory, inventoryItem, quantity;
            

       // private static readonly string[] tab1 = new string[] { "General", "Events" };
        private void OnEnable()
        {
            //isUsingMalbersInventory = serializedObject.FindProperty("isUsingMalbersInventory");
            inventoryItem = serializedObject.FindProperty("inventoryItem");
            //inventory = serializedObject.FindProperty("inventory");
            quantity = serializedObject.FindProperty("quantity");
            //itemQtyToGive = serializedObject.FindProperty("Quantity");
            //OnItemUsed = serializedObject.FindProperty("OnItemUsed");
            //OnItemDropped = serializedObject.FindProperty("OnItemDropped");
            //OnItemEquipped = serializedObject.FindProperty("OnItemEquipped");
            //OnItemUnEquipped = serializedObject.FindProperty("OnItemUnEquipped");
            //OnItemRemoved = serializedObject.FindProperty("OnItemRemoved");
            //Editor_Tabs1 = serializedObject.FindProperty("Editor_Tabs1");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            MalbersEditor.DrawDescription("Inventory Item Component");

            //Editor_Tabs1.intValue = GUILayout.Toolbar(Editor_Tabs1.intValue, tab1);
            //int Selection = Editor_Tabs1.intValue;
            //if (Selection == 0) ShowGeneral();
            //else if (Selection == 1) ShowEvents();

            ShowGeneral();

            serializedObject.ApplyModifiedProperties();
        }

        public void ShowGeneral()
        {
            //EditorGUILayout.PropertyField(inventory);
            EditorGUILayout.ObjectField(inventoryItem);
            EditorGUILayout.PropertyField(quantity);
        }

        //public void ShowEvents()
        //{
        //    EditorGUILayout.PropertyField(OnItemDropped, new GUIContent("On Item Dropped", "Invoked when the item is dropped from the slot"));
        //    EditorGUILayout.PropertyField(OnItemUsed, new GUIContent("On Item Used", "Invoked when the item is used e.g. Consumable"));
        //    EditorGUILayout.PropertyField(OnItemEquipped, new GUIContent("On Item Equipped", "Invoked when the item (Weapon) is equipped"));
        //    EditorGUILayout.PropertyField(OnItemUnEquipped, new GUIContent("On Item Unequipped", "Invoked when the item (Weapon) is unequipped"));
        //    EditorGUILayout.PropertyField(OnItemRemoved, new GUIContent("On Item Removed", "Invoked when the item is removed from the slot"));
        //}
    }
#endif

}

