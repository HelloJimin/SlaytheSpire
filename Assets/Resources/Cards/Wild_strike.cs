using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild_strike : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "난폭한 타격";
        card.value = 12;
        card.cost = 1;
        card.color = CardColor.Red;
        card.type = CardType.Attack;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.Monster;
        base.cardInit();
    }

    public override void Use(Character target)
    {
        Debug.Log(AttackDamageCheck(GameManager.instance.player));
        target.Hit(AttackDamageCheck(GameManager.instance.player));
        GameManager.instance.AddCardToDeck("Wound");
        GoCenter();
    }

    public override void CardUpgrade()
    {
        card.value = 17;
        base.CardUpgrade();
    }
}
