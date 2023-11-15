using UnityEngine;
using UnityEngine.UI;


namespace MalbersAnimations.InventorySystem
{
    [AddComponentMenu("Malbers/Inventory/Item Info Update")]

    public class ItemInfoUpdate : MonoBehaviour
    {
        // infoPanel is the parent object of the Information Panel
        public GameObject infoPanel;

        public Image icon;
        public Text nameText;
        public Text subtitleText;
        public Text descriptionText;

        public void UpdateInfoPanel(Item itemInfo)
        {
            if (itemInfo != null)
            {
                // Set the Information Panel Active
                infoPanel.SetActive(true);

                //Change the name of the Item Selected in the UI
                nameText.text = itemInfo.itemName;
                icon.sprite = itemInfo.icon;
                descriptionText.text = itemInfo.description;
                subtitleText.text = itemInfo.subtitleText;
            }
            else
            {
                // If there is no item in the slot, it sets the panel off
                infoPanel.SetActive(false);
            }
        }

        // Set the Information Panel Inactive (this is triggered if the player takes the mouse from over any item in the UI)
        public void ClosePanel()
        {
            infoPanel.SetActive(false);
        }
    }
}