using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData data;

    void Start()
    {
        
    }

    public void Hit(float damage)
    {
        data.currentHP -= (int)damage;
    }
}

[System.Serializable]
public struct CharacterData
{
    public string name;
    public int currentHP;
    public int maxHP;

    public int sheild;
    public int power;
    public int money;
}