using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

#region enum

public enum CardColor
{
    Red,
    Gray,
    Black
}
public enum CardGrade
{
    None,
    Nomal,
    Rare,
    Legend
}
public enum CardType
{
    Attack,
    Skill,
    Power,
    Curse,
    CC
}

#endregion

public class Card : MonoBehaviour
{
    public CardData card;
    public GameObject usedAnime;

    Image[] images;
    Text[] texts;

    void Start()
    {
        //usedAnime = transform.parent.parent.Find("UsedCardAnimation").gameObject;
        //card.color = CardColor.Red;
        //card.type = CardType.Attack;
        //card.grade = CardGrade.Legend;

        //JsonManager.SaveJsonData(card, "Card", GetType().Name);
        //SpriteSetting();
    }

    public virtual void cardInit()
    {
        card.cardImagePath = "Sprite/CardImage/" + GetType().Name.ToLower();
        JsonManager.SaveJsonData(card, "Card", GetType().Name);
        SpriteSetting();
    }

    public virtual bool Use(GameObject target) //실제 사용되는 카드 효과
    {
        return true;
    }

    public void GoCenter()
    {
        transform.DOLocalMove(new Vector3(100, 600, 0), 0.2f)
        .OnComplete(()=>
        {
            usedAnime.SetActive(true);
            UsedCard();
            UIManager.instance.SettingUI();
        });
    }

    public void UsedCard()
    {
        gameObject.SetActive(false);
        transform.SetParent(GameManager.instance.myCemetary.transform);
    }

    [ContextMenu("load")]
    public void SpriteSetting()
    {
        card = JsonManager.LoadJsonData<CardData>("Card", GetType().Name);
        gameObject.name = card.name;
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();

        switch (card.type)
        {
            case CardType.Attack:
                texts[0].text = "공격";
                break;
            case CardType.Skill:
                texts[0].text = "스킬";
                break;
            case CardType.Power:
                texts[0].text = "파워";
                break;
            case CardType.Curse:
                texts[0].text = "저주";
                break;
            case CardType.CC:
                texts[0].text = "상태이상";
                break;
        }

        texts[1].text = card.name;
        texts[2].text = card.cost.ToString();

        images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/"+ card.color+"_"+card.type) as Sprite;
        images[1].sprite = Resources.Load<Sprite>(card.cardImagePath) as Sprite;
        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/" + card.type + "_" +  card.grade) as Sprite;
        images[3].sprite = Resources.Load<Sprite>("Sprite/Cards/Label_" + card.grade) as Sprite;

        if (card.type == CardType.Power)
        {
            //y58 / y-71
            images[2].transform.localPosition = new Vector3(0, 58, 0);
            texts[0].transform.localPosition =  new Vector3(0, -71, 0);
        }
    }
}

[System.Serializable]
public struct CardData
{
    public CardColor color;
    public CardType type;
    public CardGrade grade;

    public string name;
    public string description;
    public int value;
    public int cost;
    public bool isUpgrade;
    public string cardImagePath;
}