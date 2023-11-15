using MalbersAnimations.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.InventorySystem
{
    public class NotificationManager : MonoBehaviour
    {
        //Content Variables
        public float activeTimer = 3f;
        public bool useTitle;
        public bool useDesc;
        public GameObject notificationGO;
        public Transform notificationParent;

        public List<NotificationChild> activeNotifications =new();
        public GameObjectEvent OnNotificationOpen = new();
        public GameObjectEvent OnNotificationClosed = new();

        #region singleton
        public static NotificationManager Instance;
        #endregion

        #region HiddenVariables
        //Hidden Variables
        [HideInInspector, SerializeField] private int Editor_Tabs1;
        #endregion
        private void Awake()
        {
            Instance = this;
            //notificationGO.SetActive(false);
        }

        public void OpenNotification(string TitleToUse = "", string DescToUse = "" , Sprite iconObject = null)
        {
            //We instantiate a new notification
            GameObject newNotification = Instantiate(notificationGO, notificationParent);
            NotificationChild notification = newNotification.GetComponentInChildren<NotificationChild>();
            activeNotifications.Add(notification);

            //Set the variables
            if (useTitle) notification.title = TitleToUse;
            if (useDesc) notification.description = DescToUse;
            notification.titleObject.text = notification.title;
            notification.descObject.text = notification.description;

            if (iconObject != null)
            {
                notification.iconObject.sprite = iconObject;
            }


            //Play the Animation.
           // notification.notificationAnimator.Play("Base Layer.AnimationIn");
            notification.EnterAnimation?.Activate();
            OnNotificationOpen.Invoke(notification.gameObject);
            StartCoroutine(Timer(notification));
        }

        public void CloseNotification(NotificationChild notification)
        {
           // notification.notificationAnimator.Play("Base Layer.AnimationOut");
            notification.ExitAnimation?.Activate();
            notification.icon = null;
            notification.title = "";
            notification.description = "";

            OnNotificationClosed.Invoke(notification.gameObject);
            activeNotifications.Remove(notification);
        }

        IEnumerator Timer(NotificationChild notification)
        {
            yield return new WaitForSeconds(activeTimer);
            CloseNotification(notification);
            yield return new WaitForSeconds(2f);
            Destroy(notification.transform.parent.gameObject);
        }
    }
}