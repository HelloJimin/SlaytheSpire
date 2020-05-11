using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    private void Awake()
    {
        data.maxHP = 30;
        data.money = 10;
        data.currentHP = data.maxHP;

        transform.Find("Canvas").transform.Find("MyHP").transform.Find("HPBar").GetComponent<Image>().fillAmount = (float)data.currentHP / (float)data.maxHP;
        transform.Find("Canvas").GetComponentInChildren<Text>().text = data.currentHP + "/" + data.maxHP;

    }
    void Start()
    {
    }

}