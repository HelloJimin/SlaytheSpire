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
        LoadInvenData();
        LoadPlayerData();
        //isPlayer = true;
        //if (gameObject.name == "Ironclad")
        //{
        //    IroncladSetting();
        //}
        //// Camera.main.WorldToScreenPoint(transform.position);
        //// transform.Find("Canvas").transform.Find("MyHP").position = Camera.main.WorldToScreenPoint(transform.position);


        //transform.Find("Canvas").transform.Find("MyHP").transform.Find("HPBar").GetComponent<Image>().fillAmount = (float)data.currentHP / (float)data.maxHP;
        //transform.Find("Canvas").GetComponentInChildren<Text>().text = data.currentHP + "/" + data.maxHP;
        //LoadInvenData();
        ////JsonManager.SaveJsonData(data, gameObject.name, gameObject.name);
        ////JsonManager.SaveJsonData(inventoryList, gameObject.name, gameObject.name + "List");
    }

    void CreateInvenList()
    {
        for (int i = 0; i < 5; i++)
        {
            inventoryList.Add("Strike");
        }
        for (int i = 0; i < 4; i++)
        {
            inventoryList.Add("Defend");
        }
        inventoryList.Add("Bash");
        //inventoryList.Add("Armaments");
        //inventoryList.Add("Inflame");
        //inventoryList.Add("Heavy_blade");
        //inventoryList.Add("Wild_strike");
        //inventoryList.Add("Flex");
        //inventoryList.Add("Body_slam");
    }

    void LoadPlayerData()
    {

        if (JsonManager.CheckJsonData(gameObject.name, gameObject.name))
        {
            data = JsonManager.LoadJsonData<CharacterData>(gameObject.name, gameObject.name);
        }
        else
        {
            IroncladSetting();
            JsonManager.SaveJsonData(data, gameObject.name, gameObject.name);
        }
    }

    void LoadInvenData()
    {
        if (JsonManager.CheckJsonData(gameObject.name, gameObject.name + "ItemList"))
        {
            inventoryList = JsonManager.LoadJsonData<List<string>>(gameObject.name, gameObject.name + "ItemList");
        }
        else
        {
            CreateInvenList();
            JsonManager.SaveJsonData(inventoryList, gameObject.name, gameObject.name + "ItemList");
        }
    }

    void IroncladSetting()
    {
        data.maxHP = 80;
        data.currentHP = data.maxHP;
        data.money = 100;

    }

}
