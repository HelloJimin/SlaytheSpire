using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Reward : MonoBehaviour
{
    public Image icon;
    Artifact rewardArtifact;

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

            GameObject choicePanel = UIManager.instance.reward.transform.Find("ChoicePanel").gameObject;

            Card[] cards = choicePanel.GetComponentsInChildren<Card>();

            for (int i = 0; i < cards.Length; i++)
            {
                ObjectPoolManager.instance.ReturnCard(cards[i]);
            }

            icon.sprite = Resources.Load<Sprite>("Sprite/Reward/normalCardReward") as Sprite;
            GetComponentInChildren<Text>().text = "덱에 카드를 추가";
            GetComponent<Button>().onClick.AddListener(() => nomalCardButton());
        }
        else if (type == "elite")
        {
            GameObject choicePanel = UIManager.instance.reward.transform.Find("ChoicePanel").gameObject;

            Card[] cards = choicePanel.GetComponentsInChildren<Card>();

            for (int i = 0; i < cards.Length; i++)
            {
                ObjectPoolManager.instance.ReturnCard(cards[i]);
            }
            Debug.Log("엘리트로 들어옴");
            icon.sprite = Resources.Load<Sprite>("Sprite/Reward/normalCardReward") as Sprite;
            GetComponentInChildren<Text>().text = "덱에 카드를 추가";
            GetComponent<Button>().onClick.AddListener(() => eliteCardButton());
        }
        else if(type == "artifact")
        {
            GameObject choicePanel = UIManager.instance.reward.transform.Find("ChoicePanel").gameObject;

            rewardArtifact = ObjectPoolManager.instance.GetArtifact(true);
            rewardArtifact.transform.SetParent(choicePanel.transform);

            icon.sprite = rewardArtifact.sprite;
            GetComponentInChildren<Text>().text = rewardArtifact.name;
            GetComponent<Button>().onClick.AddListener(() => artifactButton());
        }
    }

    public void MoneyButton()
    {
        GameManager.instance.player.Money += GameManager.instance.currentRoomMoney;
        ObjectPoolManager.instance.ReturnRewardButton(this);
        UIManager.instance.SettingUI();
        SoundManager.instance.PlaySound("GetGold");
    }

    public void eliteCardButton()
    {
        UIManager.instance.RewardbanerText.text = "카드를 선택하십시오";
        UIManager.instance.reward.transform.Find("Panel").gameObject.SetActive(false);
        UIManager.instance.reward.transform.Find("ChoicePanel").gameObject.SetActive(true);
        UIManager.instance.SettingUI();
        UIManager.instance.choice = ChoiceMode.Choice;

        UIManager.instance.choicePanel.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            CardGrade cardGrade;

            if (Random.value > 0.5f)
            {
                cardGrade = CardGrade.Nomal;
            }
            else
            {
                if (Random.value > 0.5f)
                {
                    cardGrade = CardGrade.Rare;
                }
                else
                {
                    cardGrade = CardGrade.Legend;
                }
            }
            RandmCardRewardNomal(CardColor.Red, cardGrade);
        }
        //카드선택 ㄱ
        ObjectPoolManager.instance.ReturnRewardButton(this);
        SoundManager.instance.PlaySound("Click");
    }
    public void nomalCardButton()
    {
        UIManager.instance.RewardbanerText.text = "카드를 선택하십시오";
        UIManager.instance.reward.transform.Find("Panel").gameObject.SetActive(false);
        UIManager.instance.reward.transform.Find("ChoicePanel").gameObject.SetActive(true);
        UIManager.instance.SettingUI();
        UIManager.instance.choice = ChoiceMode.Choice;

        UIManager.instance.choicePanel.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            CardGrade cardGrade;

            if (Random.value > 0.3f)
            {
                cardGrade = CardGrade.Nomal;
            }
            else
            {
                if (Random.value > 0.3f)
                {
                    cardGrade = CardGrade.Rare;
                }
                else
                {
                    cardGrade = CardGrade.Legend;
                }
            }
            RandmCardRewardNomal(CardColor.Red, cardGrade);
        }
        //카드선택 ㄱ
        ObjectPoolManager.instance.ReturnRewardButton(this);
        SoundManager.instance.PlaySound("Click");
    }

    public void artifactButton()
    {
        GameManager.instance.myArtifactList.Add(rewardArtifact.GetType().Name);
        rewardArtifact.transform.SetParent(GameManager.instance.myArtifact.transform);
        rewardArtifact.gameObject.SetActive(true);
        rewardArtifact.ActiveEffect();


        ObjectPoolManager.instance.ReturnRewardButton(this);
        SoundManager.instance.PlaySound("Click");
    }

    public void RandmCardRewardNomal( CardColor cardColor, CardGrade cardGrade)
    {
        string[] list = ObjectPoolManager.instance.lists.cardList;
        GameObject choicePanel = UIManager.instance.reward.transform.Find("ChoicePanel").gameObject;
        bool ok = true;
        int max = 0;
        while (ok)
        {
            max++;
            if (max >= 20)
            {
                cardGrade = CardGrade.Nomal;
            }

            int random = Random.Range(0, list.Length);
            Card shopCard = ObjectPoolManager.instance.GetCard(list[random]);

            if (shopCard.name == "타격" || shopCard.name == "수비" || shopCard.name == "타격+" || shopCard.name == "수비+")
            {
                ObjectPoolManager.instance.ReturnCard(shopCard);
                continue;
            }

            if (shopCard.card.type == CardType.CC || shopCard.card.type == CardType.Curse)
            {
                ObjectPoolManager.instance.ReturnCard(shopCard);
                continue;
            }

            if (shopCard.card.color != cardColor)
            {
                ObjectPoolManager.instance.ReturnCard(shopCard);
                continue;
            }
            bool check = false;

            if (check)
            {
                continue;
            }

            if (shopCard.card.grade == cardGrade)
            {
                shopCard.gameObject.transform.SetParent(choicePanel.transform);
                shopCard.gameObject.SetActive(true);
                ok = false;
            }
        }
    }

    public void RandmCardRewardNomal()
    {
        string[] list = ObjectPoolManager.instance.lists.cardList;
        GameObject choicePanel = UIManager.instance.reward.transform.Find("ChoicePanel").gameObject;

        bool ok = true;

        while (ok)
        {
            int random = Random.Range(0, list.Length);
            Card rewardCard = ObjectPoolManager.instance.GetCard(list[random]);

            if (rewardCard.name == "타격" || rewardCard.name == "수비" || rewardCard.name == "타격+" || rewardCard.name == "수비+")
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
