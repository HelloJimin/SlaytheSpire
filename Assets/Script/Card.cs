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
public enum CardTarget
{
    Player,
    Monster,
    All
}

#endregion

public class Card : MonoBehaviour
{
    public CardData card;
    public GameObject usedAnime;
    public Player player;
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
        gameObject.name = card.name;
        card.isUpgrade = false; 
        player = FindObjectOfType<Player>();

        JsonManager.SaveJsonData(card, "Card", GetType().Name);
        card = JsonManager.LoadJsonData<CardData>("Card", GetType().Name);
        SpriteSetting();
    }

    public virtual void Use(Character target) //실제 사용되는 카드 효과
    {

    }

    public virtual void GoCenter(Character target)
    {
        //화면 중앙으로 간 뒤 효과가 실행됩니다.
        transform.DOLocalMove(new Vector3(100, 1000, 0), 0.2f)
        .OnComplete(()=>
        {
            StartCoroutine(StopCard(target));
            Use(target);
        });
    }

    public IEnumerator StopCard(Character target)
    {
        yield return new WaitForSeconds(0.1f);

        if (usedAnime.activeSelf)
        {
            StartCoroutine(StopCard(target));
        }
        else
        {
            //usedAnime.SetActive(true);
            UIManager.instance.UsedCardAnimeStart(GameManager.instance.myCemetary);
            UsedCard(GameManager.instance.myCemetary.transform);
        }
    }

    public void UsedCard(Transform cemetary)
    {
        gameObject.SetActive(false);
        transform.SetParent(cemetary);
        UIManager.instance.SettingUI();
    }

    [ContextMenu("load")]
    public void SpriteSetting()
    {
        TextUpdate();

        images = GetComponentsInChildren<Image>();

        images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/"+ card.color+"_"+card.type) as Sprite;
        images[1].sprite = Resources.Load<Sprite>("Sprite/CardImage/"+GetType().Name.ToLower()) as Sprite;
        images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/" + card.type + "_" +  card.grade) as Sprite;
        images[3].sprite = Resources.Load<Sprite>("Sprite/Cards/Label_" + card.grade) as Sprite;

        if (card.type == CardType.Power)
        {
            //y58 / y-71
            images[2].transform.localPosition = new Vector3(0, 58, 0);
            texts[0].transform.localPosition =  new Vector3(0, -71, 0);
        }

        if (card.type == CardType.CC || card.type==CardType.Curse)
        {
            images[0].sprite = Resources.Load<Sprite>("Sprite/Cards/Gray_Skill") as Sprite;
            images[2].sprite = Resources.Load<Sprite>("Sprite/Cards/skill_" + card.grade) as Sprite;
            images[4].gameObject.SetActive(false);
        }
    }

    void TextUpdate()
    {
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
        texts[3].text = card.description;
    }

    public bool CostCheck()
    {
        if (GameManager.instance.currentCost < card.cost)
        {
            return false;
        }
        GameManager.instance.currentCost -= card.cost;
        UIManager.instance.SettingUI();
        return true;
    }

    public bool TargetCheck(Character character)
    {
        if (card.target == CardTarget.Player)
        {
            if (character.isPlayer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (card.target == CardTarget.Monster)
        {
            if (character.isPlayer)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        return true;
    }

    public virtual void CardUpgrade()
    {
        if (card.isUpgrade)
        {
            return;
        }
        card.name = card.name + "+";
        card.isUpgrade = true;
        texts[1].text = card.name;
        gameObject.name = card.name;
        TextUpdate();
    }

    public virtual int AttackDamageCheck(Character user)
    {
        float damage;

        if (user.weak > 0)
        {
            damage = card.value - (card.value * 0.25f) + user.data.power;
            if (damage < 0) damage = 0;
        }
        else
        {
            damage = user.data.power + card.value;
        }
        return (int)damage;
    }
}

[System.Serializable]
public struct CardData
{
    public CardColor color;
    public CardType type;
    public CardGrade grade;
    public CardTarget target;

    public string name;
    public string description;
    public int value;
    public int cost;
    public bool isUpgrade;
}