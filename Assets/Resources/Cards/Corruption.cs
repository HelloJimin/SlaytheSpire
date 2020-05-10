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
        base.cardInit();
    }

    public override bool Use(GameObject target)
    {
        Debug.Log("타락카드 사용");
        return true;
    }
}
