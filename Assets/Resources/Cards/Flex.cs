using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flex : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "몸 풀기";
        card.value = 2;
        card.cost = 0;
        card.color = CardColor.Red;
        card.type = CardType.Skill;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.All;
        base.cardInit();
    }

    public override void Use(Character target)
    {
        target = GameManager.instance.player;
        target.data.power += card.value;
        GameManager.instance.player.myTurnEnd += PowerDown;
        Debug.Log("파워업!:" + GameManager.instance.player.data.power);
        GoCenter();
    }

    public override void CardUpgrade()
    {
        card.value = 4;
        base.CardUpgrade();
    }
    public void PowerDown()
    {
        GameManager.instance.player.data.power -= card.value;
        GameManager.instance.player.myTurnEnd -= PowerDown;
        Debug.Log("파워다운!:" + GameManager.instance.player.data.power);
    }
}
