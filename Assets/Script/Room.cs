using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RoomType
{
    Event,
    Rest,
    Shop,
    Chest,
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
    public GameObject parent = null;
    public GameObject child = null;
    public bool isTrueRoad;
    void Start()
    {
        
    }

    public void Init(RoomType room, int fNum)
    {
        roomType = room;
        floorNum = fNum;
        isPassed = false;
        icon = GetComponent<Image>();

        icon.sprite = Resources.Load<Sprite>("Sprite/MapScene/Icons/"+roomType.ToString().ToLower()) as Sprite;
    }
}
