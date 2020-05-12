using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corruption : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "타락";
        card.cost = 3;
        card.color = CardColor.Red;
        card.type = CardType.Power;
        card.grade = CardGrade.Legend;
        card.target = CardTarget.All;
        base.cardInit();
    }

    public override void Use(Character target)
    {
        Debug.Log("타락카드 사용");
    }
}
