using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slimed : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "점액";
        card.value = 0;
        card.cost = 1;
        card.color = CardColor.Gray;
        card.type = CardType.CC;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.All;
        card.description = "소멸";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        ObjectPoolManager.instance.ReturnCard(this);
    }

    public override void CardUpgrade()
    {
        card.value = 9;
        base.CardUpgrade();
    }

    public override void SpriteSetting()
    {
        base.SpriteSetting();
        images[4].gameObject.SetActive(true);
    }
}
