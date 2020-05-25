using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public GameObject endingUI;
    public Text winOrLoseText;

    public Text dieMesage;

    public void Win()
    {
        endingUI.SetActive(true);
        winOrLoseText.text = "승리?";
        dieMesage.gameObject.SetActive(false);
    }
    public void Die()
    {
        StartCoroutine(DeadEnding());
    }

    public void GoToLobby()
    {
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
        dieMesage.gameObject.SetActive(true);
    }
}
