using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RoomType
{
    Event,
    Campfire,
    Shop,
    Monster,
    Elite,
    Boss
}

public class Room : MonoBehaviour
{
    public RoomType roomType;
    public Image icon;
    public Button button;
    public int floorNum;
    public bool isPassed;

    void Start()
    {
        
    }
    
}
