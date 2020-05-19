using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAllcardList : MonoBehaviour
{
    private void OnDisable()
    {
        Card[] cards = transform.Find("AllCards").GetComponentsInChildren<Card>();

        for (int i = 0; i < cards.Length; i++)
        {
            ObjectPoolManager.instance.ReturnCard(cards[i]);
        }
    }
}
