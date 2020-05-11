﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public CharacterData data;
    public bool isPlayer;
    public int vulnerable;
    public int weak;
    public int frail;

    void Start()
    {
        
    }

    public void Hit(float damage)
    {
        damage = DefenseDamageCheck(damage);

        data.currentHP -= (int)damage;
        transform.Find("Canvas").transform.Find("MyHP").transform.Find("HPBar").GetComponent<Image>().fillAmount = (float)data.currentHP / (float)data.maxHP;
        transform.Find("Canvas/MyHP/HPText").GetComponent<Text>().text = data.currentHP + "/" + data.maxHP;
    }
    public int AttackDamageCheck(float damage)
    {
        float Damage;

        if (weak>0)
        {
            Damage = damage - (damage * 0.25f) + data.power;
            if (Damage < 0) Damage = 0;
        }
        else
        {
            Damage = damage + data.power;
        }
        return (int)Damage;
    }

    public int DefenseDamageCheck(float damage)
    {
        float Damage = damage;

        if (vulnerable > 0) Damage *= 1.5f;

        if (data.shield>0)
        {
            float temp = Damage;
            Damage -= data.shield;
            data.shield -= (int)temp;
            if (data.shield < 0) data.shield = 0;
        }

        if (Damage < 0) Damage = 0;

        return (int)Damage;
    }

    public void GetShield(int sheild)
    {
        data.shield += sheild + data.dexterity;
        if (frail > 0) data.shield -= (int)(data.shield * 0.25f);

        transform.Find("Canvas").transform.Find("MyHP").GetComponent<Outline>().enabled = true ;
        transform.Find("Canvas/MyHP/HPBar").GetComponent<Image>().color = Color.blue;
        transform.Find("Canvas/MyHP/SheildImage").gameObject.SetActive(true);
        transform.Find("Canvas/MyHP/SheildImage").GetComponentInChildren<Text>().text = data.shield.ToString() ;
    }
    public void ShieldBreak()
    {
        if (data.shield > 0) return;
        transform.Find("Canvas/MyHP/SheildImage").gameObject.SetActive(false);
        transform.Find("Canvas/MyHP/HPBar").GetComponent<Image>().color = Color.red;
        transform.Find("Canvas").transform.Find("MyHP").GetComponent<Outline>().enabled = false;
    }

    public void MyEndPhase()
    {
        if (frail > 0) frail--;
        if (vulnerable > 0) vulnerable--;
        if (weak > 0) weak--;
    }
    public void YourEndPhase()
    {
        data.shield = 0;
        ShieldBreak();
    }
}

[System.Serializable]
public struct CharacterData
{
    public string name;
    public int currentHP;
    public int maxHP;

    public int shield;
    public int power;
    public int dexterity;
    public int money;
}