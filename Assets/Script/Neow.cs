using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Neow : MonoBehaviour
{
    public void MaxHPUP()
    {
        GameManager.instance.player.data.maxHP += 8;
        GameManager.instance.player.data.currentHP = GameManager.instance.player.data.maxHP;
        UIManager.instance.SettingUI();

        transform.Find("Canvas/RewardButton1").gameObject.SetActive(false);
    }

}
