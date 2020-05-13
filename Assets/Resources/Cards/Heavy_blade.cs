using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy_blade : Card
{
    public int power;
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "대검";
        card.value = 14;
        card.cost = 2;
        card.color = CardColor.Red;
        card.type = CardType.Attack;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.Monster;
        power = 3;
        base.cardInit();
    }

    public override void Use(Character target)
    {
        Debug.Log(AttackDamageCheck(GameManager.instance.player));
        target.Hit(AttackDamageCheck(GameManager.instance.player));
        UIManager.instance.EffectStart("atk1", target.transform);
        GoCenter();
    }

    public override int AttackDamageCheck(Character user)
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
        return (int)damage;
    }

    public override void CardUpgrade()
    {
        power = 5;
        base.CardUpgrade();
    }

}
