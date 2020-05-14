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
        base.cardInit();
    }

    public override void Use(Character target)
    {
        Debug.Log(AttackDamageCheck(GameManager.instance.player));
        target.Hit(AttackDamageCheck(GameManager.instance.player));
        UIManager.instance.EffectStart("atk2", target.transform);
     //   GoCenter();
    }

    public override void CardUpgrade()
    {
        card.cost = 0;
        base.CardUpgrade();
    }

    public override int AttackDamageCheck(Character user)
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
        return (int)damage;
    }
}
