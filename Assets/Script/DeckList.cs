using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckList : MonoBehaviour
{
    public static DeckList instance;

    public GameObject card;

    [SerializeField]
    public Queue<Card> deckList = new Queue<Card>();

    void Awake()
    {
        Cardinit();
    }

    void Cardinit()
    {
        instance = this;

        for (int i = 0; i < 20; i++)
        {
            deckList.Enqueue(CreateCard());
        }
        Debug.Log("초기 카드 생성 완료");
    }

    Card CreateCard()
    {
        Card newCard = Instantiate(card).GetComponent<Card>();
        newCard.transform.position = transform.position;
        newCard.transform.SetParent(transform);
        newCard.gameObject.SetActive(false);
        return newCard;
    }

    public static Card getCard()
    {
        if (instance.deckList.Count>0)
        {
            var card = instance.deckList.Dequeue();
            card.transform.SetParent(null);
            return card;
        }
        else
        {
            var card = instance.CreateCard();
            card.transform.SetParent(null);
            return card;
        }
    }
}
