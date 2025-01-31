﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "수비";
        card.value = 5;
        card.cost = 1;
        card.color = CardColor.Red;
        card.type = CardType.Skill;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.All;
        card.description = "방어도를 " + card.value + " 얻습니다.";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        base.Use(target);
        player.GetShield(card.value);
        Debug.Log("실드량:" + player.data.shield);
    }

    public override void CardUpgrade()
    {
        card.value = 8;
        card.description = "방어도를 " + card.value + " 얻습니다.";
        base.CardUpgrade();
    }
}
