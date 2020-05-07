using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public CharacterData data;

    void Awake()
    {
        data.maxCost = 5;
        data.maxHP = 80;
        data.currentCost = data.maxCost;
        data.currentHP = data.maxHP;
        transform.Find("Canvas").transform.Find("MyHP").GetComponentInChildren<Image>().fillAmount = data.currentHP / data.maxHP;
        transform.Find("Canvas").GetComponentInChildren<Text>().text = data.currentHP + "/" + data.maxHP;
        Debug.Log(transform.Find("Canvas").gameObject.transform.Find("MyHP").GetComponentInChildren<Image>());
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
}