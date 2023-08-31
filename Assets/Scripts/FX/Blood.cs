using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    public List<Transform> bloods = new();

    int dice;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bloods.Add(transform.GetChild(i));
        }
    }

    public void ActivateBlood()
    {
        dice = Random.Range(0, bloods.Count);
        bloods[dice].gameObject.SetActive(true);
        StartCoroutine(DeactivateWithDelay());
    }

    IEnumerator DeactivateWithDelay()
    {
        yield return new WaitForSeconds(5f);
        bloods[dice].gameObject.SetActive(false);
    }
}
