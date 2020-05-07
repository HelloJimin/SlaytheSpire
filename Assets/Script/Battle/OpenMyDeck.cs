using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMyDeck : MonoBehaviour
{
    bool isOpen;
    RectTransform[] childs;
    private void Awake()
    {
        isOpen = false;
        childs = GetComponentsInChildren<RectTransform>();
    }

    public void click()
    {
        if (isOpen)
        {
            isOpen = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            isOpen = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
