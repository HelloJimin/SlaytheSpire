using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedCardAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("setFalse", 0.8f);
    }

    void setFalse()
    {
        transform.gameObject.SetActive(false);
    }
}
