using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#region enum

enum CardColor
{
    Red,
    Gray,
    Black
}
enum CardGrade
{
    None,
    Nomal,
    Rare,
    Legend
}
enum CardType
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
    CardColor color;
    CardType type;
    CardGrade grade;

    int Cost;
    bool isUpgrade;

    Image[] images;
    
    void Start()
    {
        color = CardColor.Red;
        type = CardType.Attack;
        grade = CardGrade.Legend;

        SpriteSetting();
    }

    void Update()
    {
        
    }

    void SpriteSetting()
    {
        images = GetComponentsInChildren<Image>();

        switch (color)
        {
            case CardColor.Red:
                switch (type)
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
                switch (type)
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
                switch (type)
                {
                    case CardType.Curse:
                        images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/Black") as Sprite;
                        break;
                }
                break;
        }
        switch (grade)
        {
            case CardGrade.None:
                break;
            case CardGrade.Nomal:
                switch (type)
                {
                    case CardType.Attack:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Attack_Nomal") as Sprite;
                        break;
                    case CardType.Skill:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Nomal") as Sprite;
                        break;
                    case CardType.Power:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Power_Nomal") as Sprite;
                        break;
                    case CardType.Curse:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Nomal") as Sprite;
                        break;
                    case CardType.CC:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Nomal") as Sprite;
                        break;
                }
                images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Label_Nomal") as Sprite;
                break;
            case CardGrade.Rare:
                switch (type)
                {
                    case CardType.Attack:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Attack_Rare") as Sprite;
                        break;
                    case CardType.Skill:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Rare") as Sprite;
                        break;
                    case CardType.Power:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Power_Rare") as Sprite;
                        break;
                    case CardType.Curse:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Rare") as Sprite;
                        break;
                    case CardType.CC:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Rare") as Sprite;
                        break;
                }
                images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Label_Rare") as Sprite;
                break;
            case CardGrade.Legend:
                switch (type)
                {
                    case CardType.Attack:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Attack_Legend") as Sprite;
                        break;
                    case CardType.Skill:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Legend") as Sprite;
                        break;
                    case CardType.Power:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Power_Legend") as Sprite;
                        break;
                    case CardType.Curse:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Legend") as Sprite;
                        break;
                    case CardType.CC:
                        images[1].sprite = Resources.Load<Sprite>("Sprite/Cards/Skill_Legend") as Sprite;
                        break;
                }
                images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/Label_Legend") as Sprite;
                break;
        }
    }
}
