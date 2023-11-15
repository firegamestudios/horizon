using UnityEditor;
using UnityEngine;


namespace MalbersAnimations.InventorySystem
{
    [CustomEditor(typeof(InventorySlot)),CanEditMultipleObjects]
    public class InventorySlotEditor : Editor
    {
        SerializedProperty SlotID, item, selectedSlot, itemImage, quantity, hoverOverImage, selectedSlotImage, EquippedText,
                equippedSlot, draggable, Editor_Tabs1, OnItemUsed, OnItemDropped, OnItemEquipped, OnItemUnEquipped, OnItemRemoved;
        //slotType;

        private static string[] tab1 = new string[] { "Debug", "Events" };
        public static GUIStyle StyleGray => MTools.Style(new Color(0.5f, 0.5f, 0.5f, 0.3f));
        private GUIStyle styleDesc;
        GUIStyle StyleRed => MTools.Style(new Color(1f, 0f, 0f, 0.3f));

        private void OnEnable()
        {
            SlotID = serializedObject.FindProperty("SlotID");
            item = serializedObject.FindProperty("item");
            selectedSlot = serializedObject.FindProperty("selectedSlot");
            itemImage = serializedObject.FindProperty("itemImage");
            quantity = serializedObject.FindProperty("quantity");
            hoverOverImage = serializedObject.FindProperty("hoverOverImage");
            selectedSlotImage = serializedObject.FindProperty("selectedSlotImage");
            EquippedText = serializedObject.FindProperty("EquippedText");
            equippedSlot = serializedObject.FindProperty("equippedSlot");
            draggable = serializedObject.FindProperty("draggable");
            OnItemUsed = serializedObject.FindProperty("OnItemUsed");
            OnItemDropped = serializedObject.FindProperty("OnItemDropped");
            OnItemEquipped = serializedObject.FindProperty("OnItemEquipped");
            OnItemUnEquipped = serializedObject.FindProperty("OnItemUnEquipped");
            OnItemRemoved = serializedObject.FindProperty("OnItemRemoved");
            Editor_Tabs1 = serializedObject.FindProperty("Editor_Tabs1");
            //slotType = serializedObject.FindProperty("slotType");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            MalbersEditor.DrawDescription("Inventory Slot");

            Editor_Tabs1.intValue = GUILayout.Toolbar(Editor_Tabs1.intValue, tab1);
            int Selection = Editor_Tabs1.intValue;
            if (Selection == 0) ShowDebug();
            else if (Selection == 1) ShowEvents();

            serializedObject.ApplyModifiedProperties();


        }

        public void ShowDebug()
        {
            styleDesc = new GUIStyle(StyleRed)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                stretchWidth = true
            };

            styleDesc.normal.textColor = EditorStyles.label.normal.textColor;

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Do not edit slots without consulting documentation!", styleDesc);
                EditorGUILayout.PropertyField(SlotID, new GUIContent("Slot ID", "The Slot ID that is automatically assigned"));
                // EditorGUILayout.PropertyField(slotType, new GUIContent("Slot Type", "The Slot Type"));
                EditorGUILayout.PropertyField(item, new GUIContent("Item in Slot", "The item that is currently in the slot"));
                EditorGUILayout.PropertyField(selectedSlot, new GUIContent("Selected Slot?", "Is this currently the selected slot?"));
                EditorGUILayout.PropertyField(itemImage, new GUIContent("Item Image", "The image of the item that is currently in the slot"));
                EditorGUILayout.PropertyField(quantity, new GUIContent("Quantity of Item", "The quantity of the item that is in the slot"));
                EditorGUILayout.PropertyField(hoverOverImage, new GUIContent("Hover over Image?", "Is the mouse currently hovered over the image/slot?"));
                EditorGUILayout.PropertyField(selectedSlotImage, new GUIContent("Selected Slot Image", "The image displayed when the slot is selected"));
                EditorGUILayout.PropertyField(EquippedText, new GUIContent("Equipped Text", "The reference to the equipped text in the slot. The 'E' icon by default"));
                EditorGUILayout.PropertyField(equippedSlot, new GUIContent("Equipped Slot?", "Is this the equipped Slot?"));
                EditorGUILayout.PropertyField(draggable, new GUIContent("Draggable?", "Is this slot draggable?"));
            }
        }

        public void ShowEvents()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.PropertyField(OnItemDropped, new GUIContent("On Item Dropped", "Invoked when the item is dropped from the slot"));
                EditorGUILayout.PropertyField(OnItemUsed, new GUIContent("On Item Used", "Invoked when the item is used e.g. Consumable"));
                EditorGUILayout.PropertyField(OnItemEquipped, new GUIContent("On Item Equipped", "Invoked when the item (Weapon) is equipped"));
                EditorGUILayout.PropertyField(OnItemUnEquipped, new GUIContent("On Item Unequipped", "Invoked when the item (Weapon) is unequipped"));
                EditorGUILayout.PropertyField(OnItemRemoved, new GUIContent("On Item Removed", "Invoked when the item is removed from the slot"));

            }
            EditorGUILayout.EndVertical();


        }
    }
}
