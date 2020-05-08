using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public CharacterData data;
    public GameObject card;
    public List<Card> inventory;
    void Awake()
    {
        data.maxCost = 5;
        data.maxHP = 80;
        data.currentCost = data.maxCost;
        data.currentHP = 50;
        transform.Find("Canvas").transform.Find("MyHP").transform.Find("HPBar").GetComponent<Image>().fillAmount = (float)data.currentHP / (float)data.maxHP;
        transform.Find("Canvas").GetComponentInChildren<Text>().text = data.currentHP + "/" + data.maxHP;

        for (int i = 0; i < 5; i++)
        {
             data.inventoryList.Add("Strike");
        }
        for (int i = 0; i < 5; i++)
        {
             data.inventoryList.Add("Defend");
        }

        JsonManager.SaveJsonData(data, "Ironclad", GetType().Name);

        for (int i = 0; i < data.inventoryList.Count; i++)
        {
            //Debug.Log(System.Type.GetType(data.inventoryList[i]));
            //card.AddComponent(System.Type.GetType(data.inventoryList[i]));
            GameObject test = Instantiate(card);
            test.AddComponent(System.Type.GetType(data.inventoryList[i]));
            Card newCard = test.GetComponent<Card>();
            newCard.transform.SetParent(transform.Find("Canvas").transform.Find("MyHand"));
            newCard.transform.localScale = new Vector3(2, 2, 2);
        }
    }
}

[System.Serializable]
public struct CharacterData
{
    public int sheild;
    public int maxCost;
    public int currentCost;
    public int power;
    public int currentHP;
    public int maxHP;
    public int money;
    //  public List<Card> inventory;
    public List<string> inventoryList;
}