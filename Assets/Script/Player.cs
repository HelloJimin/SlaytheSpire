using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
       // transform.Find("Canvas").transform.Find("MyHP").position = Camera.main.WorldToScreenPoint(transform.position);


        JsonManager.SaveJsonData(data, gameObject.name, gameObject.name);
        JsonManager.SaveJsonData(inventoryList, gameObject.name, gameObject.name + "List");
    }

    void IroncladSetting()
    {
        data.maxHP = 80;
        data.currentHP = data.maxHP;
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
        inventoryList.Add("Armaments");
        inventoryList.Add("Inflame");
        inventoryList.Add("Heavy_blade");
        inventoryList.Add("Wild_strike");
        inventoryList.Add("Flex");
        inventoryList.Add("Body_slam");
    }

}
