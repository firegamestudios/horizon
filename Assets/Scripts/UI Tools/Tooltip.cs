using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
   static Image tooltipImage;
    static Vector2 offset;

    public static bool active;
        
    public static TMP_Text tooltipTitle;
    public static TMP_Text tooltipDescription;

    private void Awake()
    {
        tooltipImage = transform.GetComponent<Image>();
        tooltipTitle = transform.Find("Tooltip Title").GetComponent<TMP_Text>();
        tooltipDescription = transform.Find("Tooltip Description").GetComponent<TMP_Text>();
    }
   
    private void Start()
    {
        transform.position = new Vector3(0, 8000f, 0);  
        offset = new Vector2 (0, 0);
    }

    public static void SetupTooltip(string title, string description)
    {
        active = true;
        print("Running SetupTooltip");
        tooltipTitle.text = title;  
        tooltipDescription.text = description;
        
        Vector3 mousePosition = Input.mousePosition;
        Vector3 desiredPosition = mousePosition + new Vector3(offset.x, offset.y, 0);
        if (tooltipImage.transform.position != desiredPosition)
        {
            tooltipImage.transform.position = desiredPosition;
        }
    }
    public static void ClearTooltip()
    {
        active = false;
        tooltipImage.transform.position = new Vector3(0, 8000f, 0);

    }
}
