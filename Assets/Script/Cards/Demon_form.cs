using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_form : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "악마의 형상";
        card.value = 2;
        card.cost = 3;
        card.color = CardColor.Red;
        card.type = CardType.Power;
        card.grade = CardGrade.Legend;
        card.target = CardTarget.All;
        card.description = "매 턴 시작 시 힘이 " + card.value + " 만큼 증가합니다." ;
        base.cardInit();
    }

    public override void Use(Character target)
    {
        base.Use(target);

        player.myTurnStart += DevilPower;
        player.PropertySet("Demon_form", Resources.LoadAll<Sprite>("Sprite/powers")[123], card.value, card.description);

        player.battleEnded += GameEnd;
        ObjectPoolManager.instance.ReturnCard(this);
    }

    void DevilPower()
    {
        player.Power += card.value;
    }

    void GameEnd()
    {
        player.myTurnStart -= DevilPower;
        player.RemoveStatusUI("Demon_form");
    }

    public override void CardUpgrade()
    {
        card.value = 3;
        card.description = "매 턴 시작 시 힘이 " + card.value + " 만큼 증가합니다.";
        base.CardUpgrade();
    }
}
