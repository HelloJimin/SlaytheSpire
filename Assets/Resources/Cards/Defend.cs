using System.Collections;
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
        base.cardInit();
    }

    public override void Use(Character target)
    {
        GameManager.instance.player.GetShield(card.value);
        Debug.Log("실드량:" + GameManager.instance.player.data.shield);
      //  GoCenter();
    }

    public override void CardUpgrade()
    {
        card.value = 8;
        base.CardUpgrade();
    }
}
