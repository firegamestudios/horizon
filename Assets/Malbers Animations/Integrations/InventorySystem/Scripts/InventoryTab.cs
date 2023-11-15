using UnityEngine;
using UnityEngine.UI;

namespace MalbersAnimations.InventorySystem
{
    public class InventoryTab : MonoBehaviour
    {
        public GameObject[] tabs;
        public Image[] HeaderImgs;
        public Color headerActiveColor;
        public Color headerDisabledColor;

        public void TurnOnTabs(int tab)
        {
            Inventory inventory = tabs[tab - 1].GetComponent<Inventory>();
            //make all tabs inactive
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i].SetActive(false);
                HeaderImgs[i].color = headerDisabledColor;


            }

            //Unfocus on all slots on that current tab.
            for (int i = 0; i < inventory.slotList.Count; i++)
            {
                inventory.slotList[i].draggable.UnfocusOnItem(inventory.slotList[i]);
            }

            //change tabs
            tabs[tab - 1].SetActive(true);
            HeaderImgs[tab - 1].color = headerActiveColor;
        }
    }
}