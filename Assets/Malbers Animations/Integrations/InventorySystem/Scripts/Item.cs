using UnityEngine;
using MalbersAnimations.Events;
using MalbersAnimations.Reactions;
using Newtonsoft.Json;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MalbersAnimations.InventorySystem
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Malbers Inventory/New Item")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Item : ScriptableObject
    {
        // The ID of every Item needs to be different in order to be saved and loaded - FOR SAVE SYSTEM - DO NOT CHANGE
        [Tooltip("The ID of every Item needs to be different in order to be saved and loaded - FOR SAVE SYSTEM - DO NOT CHANGE")]
        [JsonProperty] public int ID;
        //Item Type
        [FormerlySerializedAs("itemType")]
        [Tooltip("The type of Inventory the Item will be placed.")]
        public ItemType type;
        // The UI icon of the item 
        [Tooltip("UI Icon of the item")]
        public Sprite icon;
        //The Items Name
        [JsonProperty]
        public string itemName;
        [Tooltip("The items larger description")]
        [TextArea(3, 10), FormerlySerializedAs("itemDescription")]
        public string description;
        [Tooltip("The items subtitle - use it for witty one-liners or disable the gameobject on the InfoPanel if you don't want to use it")]
        public string subtitleText;

        [Tooltip("If you want an item to be stackable, set this parameter to True. Set it to false if the Item is a weapon or armor")]
        public bool Stackable;

        [Tooltip("The maxmimum number allowed in one stack")]
        public int maxStacks = 99;

        [Tooltip("The games in-world prefab - set this so that you can drop the item and it spawns back into the world")]
        public GameObject inWorldPrefab;

        [Tooltip("Is the item equippable? - E.g Weapons ")]
        public bool Equippable;

        [Tooltip("Is the item consumable - E.g Food, Potions")]
        public bool Usable;

        [Tooltip("Is the item droppable - can we drop it ?")]
        public bool Droppable;

        [Tooltip("Is the item discardable? Can it be deleted?")]
        public bool Discardable;

        //Events
        [Tooltip("Invoked when the item is used")]
        public GameObjectEvent OnItemUsed = new();

        [Tooltip("Invoked when the item is dropped")]
        public GameObjectEvent OnItemEquipped = new();

        [Tooltip("Invoked when the item is equipped")]
        public GameObjectEvent OnItemUnEquipped = new();

        [Tooltip("Invoked when the item is unequipped")]
        public GameObjectEvent OnItemDropped = new();

        [Tooltip("Invoked when the item is removed/deleted")]
        public GameObjectEvent OnItemRemoved = new();

        //Reactions
        [JsonIgnore]
        [Tooltip("Apply a reaction to the Inventory Owner when the item is used ")]
        public ItemReactions UseReaction;
        [JsonIgnore]
        [Tooltip("Apply a reaction to the Inventory Owner when the item is Equipped")]
        public ItemReactions EquipReaction;
        [JsonIgnore]
        [Tooltip("Apply a reaction to the Inventory Owner when the item is Unequipped")]
        public ItemReactions UnequipReaction;
        [JsonIgnore]
        [Tooltip("Apply a reaction to the Inventory Owner when the item is Dropped")]
        public ItemReactions DropReaction;
        [JsonIgnore]
        [Tooltip("Apply a reaction to the Inventory Owner when the item is Removed")]
        public ItemReactions RemoveReaction;

        //public List<ItemReactions> reactionsList;
        [System.Serializable]
        public class ItemReactions
        {
            public string name;
            [SerializeReference, SubclassSelector]
            public Reaction reaction;
        }

#if UNITY_EDITOR

        [SerializeField, HideInInspector] private int Editor_Tabs1;
        private void Reset()
        {
            UseReaction = new() { name = "Use", reaction = new UseItemReaction()};
            EquipReaction = new() { name = "Equip", reaction = new EquipItemReaction() };
            UnequipReaction = new() { name = "Unequip", reaction = new UnequipItemReaction() };
            DropReaction = new() { name = "Drop", reaction = new DropItemReaction() };
            RemoveReaction = new() { name = "Remove", reaction = new RemoveItemReaction() };
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Item)), CanEditMultipleObjects]
    public class ScriptableItemEditor : Editor
    {
        private SerializedProperty ID, itemIcon, itemName, itemDescription, subtitleText, Stackable, inWorldPrefab,
            Equippable, Usable, Droppable, Discardable, UseReaction, EquipReaction, UnequipReaction, DropReaction, RemoveReaction,
            itemType, maxStacks, OnItemUsed, OnItemEquipped, OnItemUnEquipped, OnItemDropped, OnItemRemoved
            , Editor_Tabs1
            ;


        private static readonly string[] tab1 = new string[] { "General", "Reactions", "Events" };
        private void OnEnable()
        {
            ID = serializedObject.FindProperty("ID");

            Editor_Tabs1 = serializedObject.FindProperty("Editor_Tabs1");

            itemName = serializedObject.FindProperty("itemName");
            itemIcon = serializedObject.FindProperty("icon");
            itemType = serializedObject.FindProperty("type");
            itemDescription = serializedObject.FindProperty("description");
            subtitleText = serializedObject.FindProperty("subtitleText");
            maxStacks = serializedObject.FindProperty("maxStacks");
            Stackable = serializedObject.FindProperty("Stackable");
            inWorldPrefab = serializedObject.FindProperty("inWorldPrefab");
            Equippable = serializedObject.FindProperty("Equippable");
            Usable = serializedObject.FindProperty("Usable");
            Droppable = serializedObject.FindProperty("Droppable");
            Discardable = serializedObject.FindProperty("Discardable");
            UseReaction = serializedObject.FindProperty("UseReaction");
            EquipReaction = serializedObject.FindProperty("EquipReaction");
            UnequipReaction = serializedObject.FindProperty("UnequipReaction");
            DropReaction = serializedObject.FindProperty("DropReaction");
            RemoveReaction = serializedObject.FindProperty("RemoveReaction");
            OnItemUsed = serializedObject.FindProperty("OnItemUsed");
            OnItemEquipped = serializedObject.FindProperty("OnItemEquipped");
            OnItemUnEquipped = serializedObject.FindProperty("OnItemUnEquipped");
            OnItemDropped = serializedObject.FindProperty("OnItemDropped");
            OnItemRemoved = serializedObject.FindProperty("OnItemRemoved");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Editor_Tabs1.intValue = GUILayout.Toolbar(Editor_Tabs1.intValue, tab1);
            int Selection = Editor_Tabs1.intValue;

            switch (Selection)
            {
                case 0: ShowGeneral(); break;
                case 1: ShowReactions(); break;
                case 2: ShowEvents(); break;

                default:
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
        private void ShowGeneral()
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                MalbersEditor.DrawDescription("Inventory Item Data");
                EditorGUILayout.PropertyField(ID);
                EditorGUILayout.PropertyField(itemName, new GUIContent("Name", "The name of the item"));
                EditorGUILayout.PropertyField(itemType, new GUIContent("Type", "The Inventory Type of the item"));
                EditorGUILayout.PropertyField(itemIcon, new GUIContent("Icon", "The Inventory Icon of the item"));
                EditorGUILayout.PropertyField(inWorldPrefab, new GUIContent("In World Prefab", "The Prefab of the Item"));
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(subtitleText);
                EditorGUILayout.PropertyField(itemDescription);
            }
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUI.DisabledGroupScope(true))
                {
                    EditorGUILayout.PropertyField(Stackable);
                    ItemType itemTypeTemp = (ItemType)itemType.objectReferenceValue;
                    Stackable.boolValue = itemTypeTemp.Stackable;
                }
                    
                if (Stackable.boolValue == true)
                {
                    EditorGUILayout.PropertyField(maxStacks);
                }

                EditorGUILayout.PropertyField(Equippable);
                EditorGUILayout.PropertyField(Usable);
                EditorGUILayout.PropertyField(Droppable);
                EditorGUILayout.PropertyField(Discardable);


                //There's no Need to  Disable Stackable


                //ItemType itemTypeTemp = (ItemType)itemType.objectReferenceValue;
                //if (itemTypeTemp != null)
                //{
                //    switch (itemTypeTemp.ID)
                //    {
                //        case 2: //Weapon
                //            using (new EditorGUI.DisabledGroupScope(true))
                //            {
                //                EditorGUILayout.PropertyField(Stackable, new GUIContent("Stackable", "Is the item stackable? NO for Weapons or Armour"));
                //            }
                //            EditorGUILayout.PropertyField(Equippable, new GUIContent("Equippable", "Is the item equippable?"));
                //            EditorGUILayout.PropertyField(Usable, new GUIContent("Usable", "Is the item usable?"));
                //            EditorGUILayout.PropertyField(Droppable, new GUIContent("Droppable", "Is the item droppable?"));
                //            EditorGUILayout.PropertyField(Discardable, new GUIContent("Discardable", "Is the item discardable/removable?"));
                //            break;
                //        case 3: //Armour
                //            using (new EditorGUI.DisabledGroupScope(true))
                //            {
                //                EditorGUILayout.PropertyField(Stackable, new GUIContent("Stackable", "Is the item stackable? NO for Weapons or Armour"));
                //            }
                //            EditorGUILayout.PropertyField(Equippable, new GUIContent("Equippable", "Is the item equippable?"));
                //            EditorGUILayout.PropertyField(Usable, new GUIContent("Usable", "Is the item usable?"));
                //            EditorGUILayout.PropertyField(Droppable, new GUIContent("Droppable", "Is the item droppable?"));
                //            EditorGUILayout.PropertyField(Discardable, new GUIContent("Discardable", "Is the item discardable/removable?"));
                //            break;
                //        case 5: //Key Item
                //            using (new EditorGUI.DisabledGroupScope(true))
                //            {
                //                EditorGUILayout.PropertyField(Stackable, new GUIContent("Stackable", "Is the item stackable? NO for Weapons or Armour"));
                //                if (Stackable.boolValue == true)
                //                {
                //                    EditorGUILayout.PropertyField(maxStacks, new GUIContent("Max Stacks", "The maxmimum number allowed in one stack"));
                //                }
                //                EditorGUILayout.PropertyField(Equippable, new GUIContent("Equippable", "Is the item equippable?"));
                //                EditorGUILayout.PropertyField(Usable, new GUIContent("Usable", "Is the item usable?"));
                //                EditorGUILayout.PropertyField(Droppable, new GUIContent("Droppable", "Is the item droppable?"));
                //                EditorGUILayout.PropertyField(Discardable, new GUIContent("Discardable", "Is the item discardable/removable?"));
                //            }

                //            break;
                //        default:
                //            EditorGUILayout.PropertyField(Stackable, new GUIContent("Stackable", "Is the item stackable? NO for Weapons or Armour"));
                //            if (Stackable.boolValue == true)
                //            {
                //                EditorGUILayout.PropertyField(maxStacks, new GUIContent("Max Stacks", "The maxmimum number allowed in one stack"));
                //            }
                //            EditorGUILayout.PropertyField(Equippable, new GUIContent("Equippable", "Is the item equippable?"));
                //            EditorGUILayout.PropertyField(Usable, new GUIContent("Usable", "Is the item usable?"));
                //            EditorGUILayout.PropertyField(Droppable, new GUIContent("Droppable", "Is the item droppable?"));
                //            EditorGUILayout.PropertyField(Discardable, new GUIContent("Discardable", "Is the item discardable/removable?"));
                //            break;
                //    }
                //}
            }
        }

        private void ShowReactions()
        {

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
            MalbersEditor.DrawDescription("Inventory Button Reactions");
                EditorGUI.indentLevel++;
                if (Usable.boolValue == true)
                {
                    EditorGUILayout.PropertyField(UseReaction);
                }
                if (Equippable.boolValue == true)
                {
                    EditorGUILayout.PropertyField(EquipReaction);
                    EditorGUILayout.PropertyField(UnequipReaction);
                }
                if (Droppable.boolValue == true)
                {
                    EditorGUILayout.PropertyField(DropReaction);
                }
                if (Discardable.boolValue == true)
                {
                    EditorGUILayout.PropertyField(RemoveReaction);
                }
                EditorGUI.indentLevel--;
            }
        }

        private void ShowEvents()
        {
           // using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                MalbersEditor.DrawDescription("Item Events");

                if (Usable.boolValue == true)
                {
                    EditorGUILayout.PropertyField(OnItemUsed);
                }
                if (Droppable.boolValue == true)
                {
                    EditorGUILayout.PropertyField(OnItemDropped);
                }
                if (Discardable.boolValue == true)
                {
                    EditorGUILayout.PropertyField(OnItemRemoved);
                }
                if (Equippable.boolValue == true)
                {
                    EditorGUILayout.PropertyField(OnItemEquipped);
                    EditorGUILayout.PropertyField(OnItemUnEquipped);
                }
            }
        }
    }
#endif

}


