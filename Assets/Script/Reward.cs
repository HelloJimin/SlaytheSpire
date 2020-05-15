using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Reward : MonoBehaviour
{
    public Image icon;

    public void init(string type)
    {
        if (type == "money")
        {
            icon.sprite = Resources.Load<Sprite>("Sprite/Reward/gold") as Sprite;
            GetComponentInChildren<Text>().text = GameManager.instance.currentRoomMoney + "골드";
            GetComponent<Button>().onClick.AddListener(() => MoneyButton());

        }
        else if (type == "nomal")
        {
            icon.sprite = Resources.Load<Sprite>("Sprite/Reward/normalCardReward") as Sprite;
            GetComponentInChildren<Text>().text = "덱에 카드를 추가";
            GetComponent<Button>().onClick.AddListener(() => nomalCardButton());
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
        UIManager.instance.RewardbanerText.text = "카드를 선택하십시오";
        UIManager.instance.reward.transform.Find("Panel").gameObject.SetActive(false);
        UIManager.instance.reward.transform.Find("ChoicePanel").gameObject.SetActive(true);
        UIManager.instance.SettingUI();
        UIManager.instance.choice = ChoiceMode.Choice;

        UIManager.instance.choicePanel.SetActive(true);
        RandmCardRewardNomal();
        RandmCardRewardNomal();
        RandmCardRewardNomal();
        //카드선택 ㄱ
        ObjectPoolManager.instance.ReturnRewardButton(this);
    }

    public void RandmCardRewardNomal()
    {
        List<string> list = ObjectPoolManager.instance.cardList;
        GameObject choicePanel = UIManager.instance.reward.transform.Find("ChoicePanel").gameObject;
        bool ok = true;

        while (ok)
        {
            int random = Random.Range(0, list.Count);
            Card rewardCard = ObjectPoolManager.instance.GetCard(list[random]);

            if (rewardCard.name == "타격" || rewardCard.name == "수비")
            {
                ObjectPoolManager.instance.ReturnCard(rewardCard);
                continue;
            }

            if (rewardCard.card.type == CardType.CC || rewardCard.card.type == CardType.Curse)
            {
                ObjectPoolManager.instance.ReturnCard(rewardCard);
                continue;
            }

            bool check = false;

            for (int i = 0; i < choicePanel.transform.childCount; i++)
            {
                if (rewardCard.name == choicePanel.transform.GetChild(i).name)
                {
                    ObjectPoolManager.instance.ReturnCard(rewardCard);
                    check = true;
                    break;
                }
            }

            if (check)
            {
                continue;
            }

            if (rewardCard.card.grade == CardGrade.Nomal)
            {
                rewardCard.gameObject.transform.SetParent(choicePanel.transform);
                rewardCard.gameObject.SetActive(true);
                ok = false;
            }

        }
    }
}
