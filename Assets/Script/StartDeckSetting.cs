using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDeckSetting : MonoBehaviour
{
    public Queue<Card> deckList = new Queue<Card>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < DeckList.instance.deckList.Count; i++)
        {
            var startCard = DeckList.getCard();
            startCard.transform.position = transform.position;
            startCard.transform.SetParent(transform);
            deckList.Enqueue(startCard);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
