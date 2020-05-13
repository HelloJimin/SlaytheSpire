using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "타격";
        card.value = 6;
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
       UIManager.instance.EffectStart("atk3", target.transform);
       GoCenter();
    }

    public override void CardUpgrade()
    {
        card.value = 9;
        base.CardUpgrade();
    }

}
