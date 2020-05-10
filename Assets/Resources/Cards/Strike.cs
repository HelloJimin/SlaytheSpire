using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "타격";
        card.value = 6;
        card.cost = 1;
        card.color = CardColor.Red;
        card.type = CardType.Attack;
        card.grade = CardGrade.Nomal;
        base.cardInit();
    }

    public override bool Use(GameObject target)
    {
        if (target == null || target.GetComponent<Monster>() == null) return false;

        target.GetComponent<Monster>().data.currentHP -= card.value;
        Debug.Log("후:" + target.GetComponent<Monster>().data.currentHP);
        //transform.SetParent(transform.parent.parent);
        //transform.gameObject.SetActive(false);
        GoCenter();
        return true;
    }
}
