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
        card.description = "힘을 " + card.value + " 얻습니다. 이번 턴이 끝날 때 힘을 " + card.value + " 잃습니다.";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        base.Use(target);
        target = player;
        player.Power += card.value;
        player.myTurnEnd += PowerDown;
        player.battleEnded += GameEnd;
        Debug.Log("파워업!:" + GameManager.instance.player.data.power);
    }

    public override void CardUpgrade()
    {
        card.value = 4;
        card.description = "힘을 " + card.value + " 얻습니다. 이번 턴이 끝날 때 힘을 " + card.value + " 잃습니다.";
        base.CardUpgrade();
    }
    void PowerDown()
    {
        player.Power -= card.value;
        GameManager.instance.player.myTurnEnd -= PowerDown;
        Debug.Log("파워다운!:" + GameManager.instance.player.data.power);
    }
    void GameEnd()
    {
        player.myTurnEnd -= PowerDown;
    }
}
