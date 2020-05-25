using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LobbyManager : MonoBehaviour
{
    public Option option;
    public GameObject restartButton;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        option.LoadOptionData();

        if (JsonManager.CheckJsonData("PlayingData", "saveData"))
        {
            restartButton.SetActive(true);
        }
        else
        {
            restartButton.SetActive(false);
        }
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToReStart()
    {
        PlayerPrefs.DeleteAll();
        string[] allFiles = Directory.GetFiles(Application.persistentDataPath + "/Json/PlayingData");
        for (int i = 0; i < allFiles.Length; i++)
        {
            File.Delete(allFiles[i]);
        }
        SceneManager.LoadScene("GameScene");
    }
}
