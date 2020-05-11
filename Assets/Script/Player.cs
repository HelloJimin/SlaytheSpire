using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : Character
{
    public List<string> inventoryList;

    void Awake()
    {
        isPlayer = true;
        if (gameObject.name == "Ironclad")
        {
            IroncladSetting();
        }
       // Camera.main.WorldToScreenPoint(transform.position);
        transform.Find("Canvas").transform.Find("MyHP").position = Camera.main.WorldToScreenPoint(transform.position);


        transform.Find("Canvas").transform.Find("MyHP").transform.Find("HPBar").GetComponent<Image>().fillAmount = (float)data.currentHP / (float)data.maxHP;
        transform.Find("Canvas").GetComponentInChildren<Text>().text = data.currentHP + "/" + data.maxHP;

        JsonManager.SaveJsonData(data, gameObject.name, gameObject.name);
        JsonManager.SaveJsonData(inventoryList, gameObject.name, gameObject.name + "List");
    }

    void IroncladSetting()
    {
        data.maxHP = 80;
        data.currentHP = 50;
        data.money = 100;

        for (int i = 0; i < 5; i++)
        {
            inventoryList.Add("Strike");
        }
        for (int i = 0; i < 4; i++)
        {
            inventoryList.Add("Defend");
        }
        inventoryList.Add("Bash");
    }
}
