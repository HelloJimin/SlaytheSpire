using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class JsonManager : MonoBehaviour
{
    public static void SaveJsonData(object data, string folderName, string saveName)
    {
        //폴더가 없으면 폴더 생성
        if (!Directory.Exists(Application.persistentDataPath + "/Json/" + folderName))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Json/" + folderName);
        }

        //json데이터로 만듬
        string jsonData = JsonConvert.SerializeObject(data);
        //경로지정
        string path = Application.persistentDataPath + "/Json/" + folderName + "/" + saveName + ".json";
        //파일생성
        File.WriteAllText(path, jsonData);
    }

    public static T LoadJsonData<T>(string folderName, string loadName)
    {
        //경로지정
        string path = Path.Combine(Application.persistentDataPath + "/Json/" + folderName, loadName + ".json");
        //경로에 있는 json데이터파일 가져옴
        string jsonData = File.ReadAllText(path);
        //json데이터 반환
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    public static bool CheckJsonData(string folderName, string loadName)
    {
        if (File.Exists(Application.persistentDataPath+ "/Json/" + folderName +"/" + loadName + ".json"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
