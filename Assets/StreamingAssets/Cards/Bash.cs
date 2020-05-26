using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bash : Card
{
    public int vulPower;

    public override void cardInit()
    {
        vulPower = 2;
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "강타";
        card.value = 8;
        card.cost = 2;
        card.color = CardColor.Red;
        card.type = CardType.Attack;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.Monster;
        card.description = "피해를 " + card.value + "줍니다. 취약을 " + vulPower + "부여합니다.";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        base.Use(target);

        Debug.Log(AttackDamageCheck(player, target));

        target.Hit(AttackDamageCheck(player, target));
        target.Vulnerable += vulPower;
        UIManager.instance.EffectStart("atk2", target.transform.position);
    }

    public override void CardUpgrade()
    {
        card.value = 10;
        vulPower = 3;
        card.description = "피해를 " + card.value + "줍니다. 취약을 " + vulPower + "부여합니다.";
        base.CardUpgrade();
    }
}
