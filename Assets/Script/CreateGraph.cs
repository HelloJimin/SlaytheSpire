using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;


public class CreateGraph : MonoBehaviour
{
    [SerializeField] private Sprite dotSprite;
    private RectTransform graphContainer;

    public Button[] rooms;

    public List<List<GameObject>> lists;

    private void Start()
    {
        lists = transform.Find("Rooms").GetComponent<RoomMaker>().lists;

        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        rooms = transform.Find("Rooms").GetComponentsInChildren<Button>();
        ShowGraph();
    }

    private void ShowGraph()
    {
        //RectTransform lastRoomObject = null;

        //for (int i = 0; i < rooms.Length; i++)
        //{
        //    RectTransform roomObject = rooms[i].GetComponent<RectTransform>();
        //    if (lastRoomObject != null)
        //    {
        //        CreateDotConnection(lastRoomObject.anchoredPosition, roomObject.anchoredPosition);
        //    }
        //    lastRoomObject = roomObject;
        //}

        List<RectTransform> lastRoomObject = new List<RectTransform>();

        int start = 0;
        for (int i = 0; i < lists.Count; i++)
        {
            if (i == lists.Count - 1)
            {
                start = 0;
            }

            List<RectTransform> roomObject = new List<RectTransform>();

            for (int k = 0; k < start+1; k++)
            {
                roomObject.Add(lists[i][k].GetComponent<RectTransform>());

                if (lastRoomObject.Count > 0)
                {

                    int ran = UnityEngine.Random.Range(0, lastRoomObject.Count);
                    lists[i][k].GetComponent<Room>().parent = lastRoomObject[ran].gameObject;
                    lastRoomObject[ran].GetComponent<Room>().child = lists[i][k];
                }
            }
            lastRoomObject = roomObject;
            start = UnityEngine.Random.Range(0, 5);
        }


        lists[0][0].GetComponent<Room>().parent = new GameObject();
        RectTransform child = lists[15][0].GetComponent<RectTransform>();
        while (true)
        {
            child.GetComponent<Room>().isTrueRoad = true;
            RectTransform parent = child.GetComponent<Room>().parent.GetComponent<RectTransform>();

            if (parent == null)
            {
                break;
            }

            CreateDotConnection(parent.anchoredPosition, child.anchoredPosition);

            child = parent;
        }

        for (int i = 0; i < rooms.Length; i++)
        {
            if (!rooms[i].GetComponent<Room>().isTrueRoad)
            {
                //Destroy(rooms[i]);
                rooms[i].gameObject.SetActive(false);
            }
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        gameObject.GetComponent<Image>().sprite = dotSprite;
        gameObject.GetComponent<Image>().type = Image.Type.Tiled;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);

        rectTransform.localScale = new Vector3(3, 3, 3);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(distance/3, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

}
