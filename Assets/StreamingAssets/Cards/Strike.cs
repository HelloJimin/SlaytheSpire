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
        card.description = "피해를 " + card.value + " 줍니다.";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        base.Use(target);
        Debug.Log(AttackDamageCheck(player, target));
        target.Hit(AttackDamageCheck(GameManager.instance.player, target));
       UIManager.instance.EffectStart("atk3", target.transform.position);
    //   GoCenter();
    }

    public override void CardUpgrade()
    {
        card.value = 9;
        card.description = "피해를 " + card.value + " 줍니다.";
        base.CardUpgrade();
    }

}
