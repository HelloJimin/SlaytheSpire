using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bash : Card
{
    public int vulPower;

    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "강타";
        card.value = 8;
        card.cost = 2;
        card.color = CardColor.Red;
        card.type = CardType.Attack;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.Monster;
        vulPower = 2;
        base.cardInit();
    }

    public override void Use(Character target)
    {
        Debug.Log(AttackDamageCheck(GameManager.instance.player));
        target.Hit(AttackDamageCheck(GameManager.instance.player));
        target.vulnerable += vulPower;
        UIManager.instance.EffectStart("atk2", target.transform);
      //  GoCenter();
    }

    public override void CardUpgrade()
    {
        card.value = 10;
        vulPower = 3;
        base.CardUpgrade();
    }
}
