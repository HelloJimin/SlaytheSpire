using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body_slam : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "몸통박치기";
        card.value = 0;
        card.cost = 1;
        card.color = CardColor.Red;
        card.type = CardType.Attack;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.Monster;
        card.description = "현재 방어도만큼 피해를 줍니다.";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        base.Use(target);

        Debug.Log(AttackDamageCheck(player,target));
        target.Hit(AttackDamageCheck(player, target));
        UIManager.instance.EffectStart("atk2", target.transform.position);
     //   GoCenter();
    }

    public override void CardUpgrade()
    {
        card.cost = 0;
        base.CardUpgrade();
    }

    public override int AttackDamageCheck(Character user, Character target)
    {
        card.value = GameManager.instance.player.data.shield;
        float damage;

        if (user.weak > 0)
        {
            damage = card.value - (card.value * 0.25f) + user.data.power;
            if (damage < 0) damage = 0;
        }
        else
        {
            damage = user.data.power + card.value;
        }

        if (target.Vulnerable > 0) damage *= 1.5f;

        return (int)damage;
    }
}
