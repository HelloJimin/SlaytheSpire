using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Inflame : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "발화";
        card.value = 2;
        card.cost = 1;
        card.color = CardColor.Red;
        card.type = CardType.Power;
        card.grade = CardGrade.Rare;
        card.target = CardTarget.All;
        card.description = "힘을 " + card.value + " 얻습니다.";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        base.Use(target);
        player.Power += card.value;
        ObjectPoolManager.instance.ReturnCard(this);
        Debug.Log(player.Power);
       // GoCenter();
    }

    public override void CardUpgrade()
    {
        card.value = 3;
        card.description = "힘을 " + card.value + " 얻습니다.";
        base.CardUpgrade();
    }

}
