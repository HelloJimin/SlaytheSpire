using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMaker : MonoBehaviour
{
    public GameObject roomPrefab;

    public List<List<GameObject>> lists = new List<List<GameObject>>();

    void Awake()
    {
        //int shops = Random.Range(3, 4);
        //int events = Random.Range(1, 4);
        //int rests = Random.Range(3, 5);
        //int elites = Random.Range(2, 3);
        //int chests = 1;

        List<RoomType> roomTypes = new List<RoomType>();
        for (int i = 0; i < 16; i++)
        {
            roomTypes.Add(RoomType.Monster);
        }

        //roomTypes[15] = RoomType.Boss;

        //int random = 0;
        //for (int i = 0; i < shops; i++)
        //{
        //    random = Random.Range(random, roomTypes.Count - 1);
        //    roomTypes[2 + random] = RoomType.Shop;
        //}


        int startPoint = Random.Range(1, 3);

        List<GameObject> roooooms = new List<GameObject>();

        for (int i = 0; i < 1; i++)
        {
            roooooms.Add(CreateRoom(roomTypes[0], 0, i+2));
        }
        lists.Add(roooooms);

        for (int i = 1; i < 15; i++)
        {
            List<GameObject> roms = new List<GameObject>();
            for (int k = 0; k < 5; k++)
            {
                roms.Add(CreateRoom(roomTypes[i], i,k));
            }
            lists.Add(roms);
        }
        List<GameObject> rooms = new List<GameObject>();
        rooms.Add(CreateRoom(RoomType.Boss, 15,2));
        lists.Add(rooms);

        Debug.Log(lists.Count+"숫자입니다");

    }
    GameObject CreateRoom(RoomType roomType, int num , int k)
    {
        GameObject newRoom =  Instantiate(roomPrefab, transform);

        int random1 = Random.Range(-50, 50);
        int random2 = Random.Range(-50, 50);
        newRoom.transform.localPosition = new Vector3(-200+(k*100)+ random1, -1100 + (num * 150)+ random2, 0);

        newRoom.GetComponent<Room>().Init(roomType, num+1);
        return newRoom;
    }
}
