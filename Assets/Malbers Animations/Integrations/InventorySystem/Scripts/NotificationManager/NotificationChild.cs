using MalbersAnimations.Utilities;
using UnityEngine;
using UnityEngine.UI;


namespace MalbersAnimations.InventorySystem
{
    public class NotificationChild : MonoBehaviour
    {
        public Sprite icon; //More Icon types will come in future update.
        public string title;
        public string description;

        //Resource Variables
        public Image iconObject;
        public Text titleObject;
        public Text descObject;

        public MSimpleTransformer EnterAnimation;
        public MSimpleTransformer ExitAnimation;

       // public Animator notificationAnimator;
    }
}