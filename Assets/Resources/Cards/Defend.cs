using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : Card
{
    public override bool Use(GameObject target)
    {
        if (target.GetComponent<Character>() == null) return false;

        Debug.Log("전:" + target.GetComponent<Character>().data.sheild);
        target.GetComponent<Character>().data.sheild += card.value;
        Debug.Log("후:" + target.GetComponent<Character>().data.sheild);

        return true;
    }
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "수비";
        card.value = 6;
        card.cost = 1;
        card.color = CardColor.Red;
        card.type = CardType.Skill;
        card.grade = CardGrade.Nomal;

        base.cardInit();
    }
}
