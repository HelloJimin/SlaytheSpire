using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy_blade : Card
{
    public int power;
    public override void cardInit()
    {
        power = 3;
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "대검";
        card.value = 14;
        card.cost = 2;
        card.color = CardColor.Red;
        card.type = CardType.Attack;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.Monster;
        card.description = "피해를 14 줍니다. 힘의 효과가 " + power + " 배로 적용됩니다.";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        base.Use(target);
        Debug.Log(AttackDamageCheck(player,target));
        target.Hit(AttackDamageCheck(player,target));
        UIManager.instance.EffectStart("atk1", target.transform.position);
    //    GoCenter();
    }

    public override int AttackDamageCheck(Character user, Character target)
    {
        float damage;

        if (user.weak > 0)
        {
            damage = card.value - (card.value * 0.25f) + (user.data.power * power);
            if (damage < 0) damage = 0;
        }
        else
        {
            damage = (user.data.power*power) + card.value ;
        }

        if (target.Vulnerable > 0) damage *= 1.5f;

        return (int)damage;
    }

    public override void CardUpgrade()
    {
        power = 5;
        card.description = "피해를 14 줍니다. 힘의 효과가 " + power + " 배로 적용됩니다.";
        base.CardUpgrade();
    }

}
