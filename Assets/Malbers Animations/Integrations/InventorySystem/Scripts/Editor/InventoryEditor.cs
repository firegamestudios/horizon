using UnityEditor;
using UnityEngine;

namespace MalbersAnimations.InventorySystem
{
    [CustomEditor(typeof(Inventory)), CanEditMultipleObjects]

    public class InventoryEditor : Editor
    {
        SerializedProperty Editor_Tabs1, inventoryMaster, itemType, slotList, itemDroppingOffset,
            OnItemUsed, OnItemDropped, OnItemEquipped, OnItemUnEquipped, OnItemRemoved, inventoryName;


        private GUIStyle DescriptionStyle;
        public static GUIStyle StyleGray => MTools.Style(new Color(0.5f, 0.5f, 0.5f, 0.3f));
        private static readonly string[] tab1 = new string[] { "General", "Events" };
        private GUIStyle styleDesc;
        GUIStyle StyleRed => MTools.Style(new Color(1f, 0f, 0f, 0.3f));

        void OnEnable()
        {
            slotList = serializedObject.FindProperty("slotList");
            Editor_Tabs1 = serializedObject.FindProperty("Editor_Tabs1");
            itemDroppingOffset = serializedObject.FindProperty("itemDroppingOffset");
            OnItemUsed = serializedObject.FindProperty("OnItemUsed");
            OnItemDropped = serializedObject.FindProperty("OnItemDropped");
            OnItemEquipped = serializedObject.FindProperty("OnItemEquipped");
            OnItemUnEquipped = serializedObject.FindProperty("OnItemUnEquipped");
            OnItemRemoved = serializedObject.FindProperty("OnItemRemoved");
            itemType = serializedObject.FindProperty("itemType");
            inventoryMaster = serializedObject.FindProperty("inventoryMaster");
            inventoryName = serializedObject.FindProperty("inventoryName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            MalbersEditor.DrawDescription("Inventory System that integrates with the Weapon Manager, Pickable and Pick Up-Drop Components");
            CheckHelpBox();

            Editor_Tabs1.intValue = GUILayout.Toolbar(Editor_Tabs1.intValue, tab1);
            int Selection = Editor_Tabs1.intValue;
            if (Selection == 0) ShowGeneral();
            else if (Selection == 1) ShowEvents();

            serializedObject.ApplyModifiedProperties();
        }

        public void ShowGeneral()
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                styleDesc = new GUIStyle(StyleRed)
                {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleLeft,
                    stretchWidth = true
                };

                styleDesc.normal.textColor = EditorStyles.label.normal.textColor;
                EditorGUILayout.PropertyField(inventoryMaster, new GUIContent("Inventory Master", "Reference to the Inventory Master"));
                EditorGUILayout.PropertyField(inventoryName, new GUIContent("Inventory Name", "The Name of Inventory"));
                //EditorGUILayout.PropertyField(usingMalbersInventory, new GUIContent("Using Malbers Inventory", "Are we using Malbers Inventory - Leave this as TRUE if using the inventory!"));
                EditorGUILayout.PropertyField(itemType, new GUIContent("Item Type of Inventory", "What is the item type that this inventory holds?"));
                EditorGUILayout.PropertyField(itemDroppingOffset, new GUIContent("Item Dropping Offset", "Default is (0,0,0) - Set an offset if you wish to drop an item directly in front of you as an example"));
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Slots are automatically filled - Do not Manually Set!", styleDesc);

                EditorGUI.indentLevel++;
                using (new EditorGUI.DisabledGroupScope(true))
                {
                    EditorGUILayout.PropertyField(slotList,
                        new GUIContent("Inventory Slots", "All Inventory Slots go here - is auto generated based on number of Slot children"));

                }
                EditorGUI.indentLevel--;
            }
        }
        public void ShowEvents()
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(OnItemDropped);
                EditorGUILayout.PropertyField(OnItemUsed);
                EditorGUILayout.PropertyField(OnItemEquipped);
                EditorGUILayout.PropertyField(OnItemUnEquipped);
                EditorGUILayout.PropertyField(OnItemRemoved);
            }
        }

        private void CheckHelpBox()
        {
            if (DescriptionStyle == null)
            {
                DescriptionStyle = new GUIStyle(MTools.StyleGray)
                {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleLeft,
                    stretchWidth = true
                };
                DescriptionStyle.normal.textColor = EditorStyles.boldLabel.normal.textColor;
            }
        }
    }
}