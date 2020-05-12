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
        card.target = CardTarget.Monster;
        base.cardInit();
    }

    public override void Use(GameObject target)
    {
        Debug.Log(player.AttackDamageCheck(card.value));
       target.GetComponent<Character>().Hit(player.AttackDamageCheck(card.value));
       GoCenter();
    }

    public override void CardUpgrade()
    {
        card.value = 9;
        base.CardUpgrade();
    }

}
