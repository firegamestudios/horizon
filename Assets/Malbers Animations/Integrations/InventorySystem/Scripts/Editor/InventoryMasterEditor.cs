using UnityEditor;
using UnityEngine;

namespace MalbersAnimations.InventorySystem
{
    [CustomEditor(typeof(InventoryMaster))]
    [CanEditMultipleObjects]
    public class InventoryMasterEditor : Editor
    {
        SerializedProperty Editor_Tabs1, ListOfInventories, currentSelectedSlot, isOpen, lockCharacter,
            character, //playerCharacter, 
           // usingMalbersInventory,
            notifications, useButton, dropButton,
            equipButton, removeButton, unequipButton, inventoryPanel, buttonsPanel, infoPanel, RegisteredItems, EncryptionEnabled;


        private GUIStyle DescriptionStyle;
        public static GUIStyle StyleGray => MTools.Style(new Color(0.5f, 0.5f, 0.5f, 0.3f));
        private static readonly string[] tab1 = new string[] { "General", "Buttons", "References", "Debug" };
       
        
        //private GUIStyle styleDesc;
       // GUIStyle StyleRed => MTools.Style(new Color(1f, 0f, 0f, 0.3f));

        void OnEnable()
        {  
            Editor_Tabs1 = serializedObject.FindProperty("Editor_Tabs1");
            ListOfInventories = serializedObject.FindProperty("ListOfInventories");
            currentSelectedSlot = serializedObject.FindProperty("currentSelectedSlot");
            isOpen = serializedObject.FindProperty("isOpen");
            lockCharacter = serializedObject.FindProperty("lockCharacter");
            character = serializedObject.FindProperty("character");
          //  playerCharacter = serializedObject.FindProperty("playerCharacter");
           // usingMalbersInventory = serializedObject.FindProperty("usingMalbersInventory");
            notifications = serializedObject.FindProperty("notifications");
            useButton = serializedObject.FindProperty("useButton");
            dropButton = serializedObject.FindProperty("dropButton");
            equipButton = serializedObject.FindProperty("equipButton");
            removeButton = serializedObject.FindProperty("removeButton");
            unequipButton = serializedObject.FindProperty("unequipButton");
            inventoryPanel = serializedObject.FindProperty("inventoryPanel");
            buttonsPanel = serializedObject.FindProperty("buttonsPanel");
            infoPanel = serializedObject.FindProperty("infoPanel");
            RegisteredItems = serializedObject.FindProperty("RegisteredItems");
            EncryptionEnabled = serializedObject.FindProperty("EncryptionEnabled");
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            MalbersEditor.DrawDescription("Inventory System that integrates with the Weapon Manager, Pickable and Pick Up-Drop Components");
            CheckHelpBox();

            Editor_Tabs1.intValue = GUILayout.Toolbar(Editor_Tabs1.intValue, tab1);
            int Selection = Editor_Tabs1.intValue;
            if (Selection == 0) ShowGeneral();
            else if (Selection == 1) ShowButtons();
            else if (Selection == 2) ShowReferences();
            else if (Selection == 3) ShowDebug();



            serializedObject.ApplyModifiedProperties();
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

        public void ShowGeneral()
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(character);

                //EditorGUILayout.PropertyField(usingMalbersInventory, new GUIContent("Using Malbers Inventory", "Are we using Notifications in the system?"));

                EditorGUILayout.PropertyField(lockCharacter,
                    new GUIContent("Lock Character Input", "Should we lock the character input when the inventory is open?"));
                EditorGUILayout.PropertyField(notifications);
                EditorGUILayout.PropertyField(EncryptionEnabled, new GUIContent("Save System Encryption"));
            }
        }

        public void ShowButtons()
        {
            EditorGUILayout.PropertyField(useButton);
            EditorGUILayout.PropertyField(removeButton);
            EditorGUILayout.PropertyField(equipButton);
            EditorGUILayout.PropertyField(dropButton);
            EditorGUILayout.PropertyField(unequipButton);
        }

        public void ShowReferences()
        {
            EditorGUILayout.PropertyField(inventoryPanel, new GUIContent("Inventory Parent Object", "Place the Inventory Panel Object here"));
            EditorGUILayout.PropertyField(buttonsPanel, new GUIContent("Buttons Panel", "Place the Buttons Panel for the Inventory Buttons here"));
            EditorGUILayout.PropertyField(infoPanel, new GUIContent("Item Info Panel", "Place the Item Info Panel here"));
            EditorGUILayout.PropertyField(ListOfInventories, new GUIContent("Registered Inventories", "Place all your Inventories here"));
            EditorGUILayout.PropertyField(RegisteredItems, new GUIContent("Registered Items", "Place all your items here. Order does not matter."));
        }

        public void ShowDebug()
        {
            using (new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUILayout.PropertyField(isOpen, new GUIContent("Is the Inventory Open?", "DEBUG - Shows if Inventory is Open"));
                EditorGUILayout.PropertyField(currentSelectedSlot, new GUIContent("What is the current Slot Selected", "DEBUG - Shows the current Selected Slot"));
            }
        }
    }
}