using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Option : MonoBehaviour
{
    public GameObject OptionWindow;
    public Slider volume;
    public AudioSource audioSource;

    public void OptionWindowControll()
    {
        bool current = !OptionWindow.activeSelf;
        OptionWindow.SetActive(current);
        OptionWindow.transform.Find("OptionWindow").gameObject.SetActive(current);
        SaveOption();
    }

    public void LoadOptionData()
    {
        if (JsonManager.CheckJsonData("PlayingData", "Volume"))
        {
            volume.value = JsonManager.LoadJsonData<float>("PlayingData", "Volume");
            audioSource.volume = volume.value;
        }
        else
        {
            volume.value = 1;
            audioSource.volume = volume.value;
        }
    }

    public void SaveOption()
    {
        JsonManager.SaveJsonData(volume.value, "PlayingData", "Volume");
    }

    public void VolumeControll()
    {
        audioSource.volume = volume.value;
    }

    public void GameClose()
    {
        OptionWindowControll();
        Application.Quit();
    }

    public void Surrender()
    {
        PlayerPrefs.DeleteAll();

        string[] allFiles = Directory.GetFiles(Application.persistentDataPath + "/Json/PlayingData");
        for (int i = 0; i < allFiles.Length; i++)
        {
            File.Delete(allFiles[i]);
        }

        FindObjectOfType<EndingManager>().Die();
    }
}
