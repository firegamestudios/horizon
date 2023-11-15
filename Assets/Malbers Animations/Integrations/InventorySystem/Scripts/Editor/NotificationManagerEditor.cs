using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MalbersAnimations.InventorySystem
{
    [CustomEditor(typeof(NotificationManager)), CanEditMultipleObjects]
    public class NotificationManagerEditor : Editor
    {
        SerializedProperty activeTimer, useTitle, useDesc, notificationGO, notificationParent, Editor_Tabs1, OnNotificationOpen, OnNotificationClosed; //, activeNotifications;

        private GUIStyle DescriptionStyle;
        public static GUIStyle StyleGray => MTools.Style(new Color(0.5f, 0.5f, 0.5f, 0.3f));
        private static string[] tab1 = new string[] { "General", "Events" };
        private GUIStyle styleDesc;
        GUIStyle StyleRed => MTools.Style(new Color(1f, 0f, 0f, 0.3f));

        void OnEnable()
        {
            activeTimer = serializedObject.FindProperty("activeTimer");
            useTitle = serializedObject.FindProperty("useTitle");
            useDesc = serializedObject.FindProperty("useDesc");
            notificationGO = serializedObject.FindProperty("notificationGO");
            notificationParent = serializedObject.FindProperty("notificationParent");
            Editor_Tabs1 = serializedObject.FindProperty("Editor_Tabs1");
            OnNotificationOpen = serializedObject.FindProperty("OnNotificationOpen");
            OnNotificationClosed = serializedObject.FindProperty("OnNotificationClosed");
            //activeNotifications = serializedObject.FindProperty("activeNotifications");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            MalbersEditor.DrawDescription("Notification Manager system that can be used standalone or with the Inventory System");

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
                EditorGUILayout.PropertyField(activeTimer, new GUIContent("Active Timer", "The timer that the notification will remain active"));
                EditorGUILayout.PropertyField(useTitle, new GUIContent("Use Title", "Should the Title on the notification take a custom input, or one that is the default?"));
                EditorGUILayout.PropertyField(useDesc, new GUIContent("Use Desc", "Should the Description on the notification take a custom input, or one that is the default?"));
                EditorGUILayout.PropertyField(notificationGO, new GUIContent("Notification GO", "Set your Notification Prefab that you are using here"));
                EditorGUILayout.PropertyField(notificationParent, new GUIContent("Notification Parent", "Set your Notification Parent here. If using the Prefab example, it should be set to 'NotificationManager' otherwise the Vertical Layout Group/Stacking won't work"));
                //EditorGUILayout.Space();
                //EditorGUILayout.LabelField("The active notifications are for debugging purposes only!", styleDesc);
                //EditorGUILayout.PropertyField(activeNotifications, new GUIContent("Active Notifications", "This is for debugging purposes only - This shows what notifications are currently active at one time"));

            }
        }

        public void ShowEvents()
        {
            EditorGUILayout.PropertyField(OnNotificationOpen, new GUIContent("On Notification Open", "Invoked when the notification is opened"));
            EditorGUILayout.PropertyField(OnNotificationClosed, new GUIContent("On Notification Closed", "Invoked when the notification is closed"));
        }
    }
}