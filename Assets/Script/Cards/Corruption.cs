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
        card.description = "스킬 카드의 비용이 0이 됩니다. 스킬 카드를 사용할 때마다 소멸시킵니다.";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        base.Use(target);
        Debug.Log("타락카드 사용");
    }
}
