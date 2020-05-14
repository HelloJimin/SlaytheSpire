using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Reward : MonoBehaviour
{
    public void init(string type)
    {
        if (type == "money")
        {
            GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprite/Reward/gold") as Sprite;
            GetComponentInChildren<Text>().text = GameManager.instance.currentRoomMoney + "골드";
        }
        else if (type == "nomal")
        {
            GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprite/Reward/normalCardReward") as Sprite;
            GetComponentInChildren<Text>().text = "덱에 카드를 추가";
        }
    }
    public void MoneyButton()
    {

        GameManager.instance.player.data.money += GameManager.instance.currentRoomMoney;
        ObjectPoolManager.instance.ReturnRewardButton(this);
        UIManager.instance.SettingUI();
    }
    public void nomalCardButton()
    {
        UIManager.instance.choice = ChoiceMode.Choice;

        UIManager.instance.choicePanel.SetActive(true);
        Card neca = GameManager.instance.AddCardToDeck("Strike");
        neca.gameObject.transform.SetParent(UIManager.instance.choicePanel.transform);
        neca.gameObject.SetActive(true);
        //카드선택 ㄱ
        ObjectPoolManager.instance.ReturnRewardButton(this);
        UIManager.instance.reward.SetActive(false);
        UIManager.instance.SettingUI();
    }
}
