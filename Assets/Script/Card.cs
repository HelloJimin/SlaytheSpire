using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    Image[] images;
    Text[] texts;
    //private void Awake()
    //{
    //    Screen.SetResolution(3040, 1440, true);
    //}

    void Start()
    {
        card.color = CardColor.Red;
        card.type = CardType.Attack;
        card.grade = CardGrade.Legend;

        SpriteSetting();
        JsonManager.SaveJsonData(card, "Card", GetType().Name);
    }

    public virtual void Use(GameObject target) //실제 사용되는 카드 효과
    {
    }

    public void SpriteSetting()
    {
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();

        images[1].sprite = Resources.Load<Sprite>(card.cardImagePath) as Sprite;
        texts[1].text = card.name;
        texts[2].text = card.cost.ToString();
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
        switch (card.color)
        {
            case CardColor.Red:
                switch (card.type)
                {
                    case CardType.Attack:
                        images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/Red_Attack") as Sprite;
                        break;
                    case CardType.Skill:
                        images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/Red_Skill") as Sprite;
                        break;
                    case CardType.Power:
                        images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/Red_Power") as Sprite;
                        break;
                }
                break;
            case CardColor.Gray:
                switch (card.type)
                {
                    case CardType.Attack:
                        images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/Gray_Attack") as Sprite;
                        break;
                    case CardType.Skill:
                        images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/Gray_Skill") as Sprite;
                        break;
                    case CardType.Power:
                        images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/Gray_Power") as Sprite;
                        break;
                    case CardType.CC:
                        images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/Gray_Skill") as Sprite;
                        break;
                }
                break;
            case CardColor.Black:
                switch (card.type)
                {
                    case CardType.Curse:
                        images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/Black") as Sprite;
                        break;
                }
                break;
        }
        switch (card.grade)
        {
            case CardGrade.None:
                break;
            case CardGrade.Nomal:
                switch (card.type)
                {
                    case CardType.Attack:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Attack_Nomal") as Sprite;
                        break;
                    case CardType.Skill:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Nomal") as Sprite;
                        break;
                    case CardType.Power:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Power_Nomal") as Sprite;
                        break;
                    case CardType.Curse:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Nomal") as Sprite;
                        break;
                    case CardType.CC:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Nomal") as Sprite;
                        break;
                }
                images[3].sprite = Resources.Load<Sprite>("Sprite/Cards/Label_Nomal") as Sprite;
                break;
            case CardGrade.Rare:
                switch (card.type)
                {
                    case CardType.Attack:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Attack_Rare") as Sprite;
                        break;
                    case CardType.Skill:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Rare") as Sprite;
                        break;
                    case CardType.Power:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Power_Rare") as Sprite;
                        break;
                    case CardType.Curse:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Rare") as Sprite;
                        break;
                    case CardType.CC:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Rare") as Sprite;
                        break;
                }
                images[3].sprite = Resources.Load<Sprite>("Sprite/Cards/Label_Rare") as Sprite;
                break;
            case CardGrade.Legend:
                switch (card.type)
                {
                    case CardType.Attack:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Attack_Legend") as Sprite;
                        break;
                    case CardType.Skill:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Legend") as Sprite;
                        break;
                    case CardType.Power:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Power_Legend") as Sprite;
                        break;
                    case CardType.Curse:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Legend") as Sprite;
                        break;
                    case CardType.CC:
                        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Legend") as Sprite;
                        break;
                }
                images[3].sprite = Resources.Load<Sprite>("Sprite/Cards/Label_Legend") as Sprite;
                break;
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