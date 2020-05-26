using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class EndingManager : MonoBehaviour
{
    public GameObject endingUI;
    public Text winOrLoseText;

    public Text dieMesage;
    public GameObject winMesage;

    public Text[] scoreList;
    public Text allScore;

    public void Win()
    {
        endingUI.SetActive(true);
        winMesage.SetActive(true);
        dieMesage.gameObject.SetActive(false);

        winOrLoseText.text = "승리?";
        WinMesageSetting();

    }
    public void Die()
    {
        SoundManager.instance.ChangeBGM("EndingBGM");
        StartCoroutine(DeadEnding());
    }

    public void GoToLobby()
    {
        PlayerPrefs.DeleteAll();
        string[] allFiles = Directory.GetFiles(Application.persistentDataPath + "/Json/PlayingData");
        for (int i = 0; i < allFiles.Length; i++)
        {
            File.Delete(allFiles[i]);
        }
        SceneManager.LoadScene("LobbyScene");
    }

    IEnumerator DeadEnding()
    {
        yield return new WaitForSeconds(1);
        endingUI.SetActive(true);
        SoundManager.instance.option.gameObject.SetActive(false);
        UIManager.instance.battleSystem.SetActive(false);
        UIManager.instance.battleUI.SetActive(false);
        winOrLoseText.text = "완패";
        winMesage.SetActive(false);
        dieMesage.gameObject.SetActive(true);
    }
    void WinMesageSetting()
    {
        int[] scores = { GameManager.instance.saveData.floor * 50, GameManager.instance.saveData.getAllMoney, GameManager.instance.saveData.monsterKill * 100, GameManager.instance.saveData.eliteKill * 300, 10000,0 };
        scoreList[0].text = "오른 층 [" + GameManager.instance.saveData.floor + "]     -    ";
        scoreList[1].text = "얻은 돈 [" + GameManager.instance.saveData.getAllMoney + "]     -    ";
        scoreList[2].text = "몬스터 킬 [" + GameManager.instance.saveData.monsterKill + "]     -    ";
        scoreList[3].text = "엘리트 킬 [" + GameManager.instance.saveData.eliteKill + "]     -    ";
        scoreList[4].text = "보스 킬 [" + 1 + "]     -    ";
        scoreList[5].text = "???";

        for (int i = 0; i < scores.Length; i++)
        {
            scoreList[i].text += scores[i];
        }

        int temp = 0;

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i]==0)
            {
                scoreList[i].gameObject.SetActive(false);
            }
            temp += scores[i];
        }

        allScore.text = "점수       " + temp.ToString();
    }
}
