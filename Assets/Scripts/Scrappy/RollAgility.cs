using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAgility : MonoBehaviour
{
    public void OnAgilityRolled(GameObject pc)
    {
        print(pc.name + " interacted with this");
    }
}
