using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ironclad : MonoBehaviour
{
    public IroncladData data;
    
    void Awake()
    {
        data.maxCost = 3;
        data.currentCost = data.maxCost;
    }
}

[System.Serializable]
public struct IroncladData
{
    public int sheild;
    public int maxCost;
    public int currentCost;
    public int power;
    public int HP;
    public int money;
}