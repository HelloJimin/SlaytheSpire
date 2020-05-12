using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wound : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "부상";
        card.value = 0;
        card.cost = 0;
        card.color = CardColor.Gray;
        card.type = CardType.CC;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.All;
        base.cardInit();
    }

    public override void Use(Character target)
    {
        GoCenter();
    }

    public override void CardUpgrade()
    {
        card.value = 9;
        base.CardUpgrade();
    }
}
