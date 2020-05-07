using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public CharacterData data;

    private void Awake()
    {
        data.maxHP = 30;
        data.money = 10;
        
    }
    void Start()
    {
        
    }

}