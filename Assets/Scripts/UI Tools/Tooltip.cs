using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
   static Image tooltipImage;
    static Vector2 offset;

    public static bool active;

    private static TMP_Text tooltipTitle;
    private static TMP_Text tooltipDescription;

  
    private void Awake()
    {
        tooltipImage = GameObject.Find("Canvas").transform.Find("Tooltip Panel").GetComponent<Image>();
        tooltipTitle = tooltipImage.transform.Find("Tooltip Title").GetComponent<TMP_Text>();
        tooltipDescription = tooltipImage.transform.Find("Tooltip Description").GetComponent<TMP_Text>();
    }
   
    private void Start()
    {
        transform.position = new Vector3(0, 8000f, 0);  
        offset = new Vector2 (300, -300);
    }

    private void Update()
    {
        if(active)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 desiredPosition = mousePosition + new Vector3(offset.x, offset.y, 0);
            if (tooltipImage.transform.position != desiredPosition)
            {
                tooltipImage.transform.position = desiredPosition;
            }
        }
        else
        {
            ClearTooltip();
        }
       
    }

    public static void SetupTooltip(string title, string description, Vector2 _offset)
    {
        active = true;

        tooltipTitle.text = title;
        tooltipDescription.text = description;
        
        offset = _offset;
      
    }
    public static void ClearTooltip()
    {
        active = false;
        tooltipImage.transform.position = new Vector3(0, 8000f, 0);

    }
}
