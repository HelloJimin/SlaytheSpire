using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bash : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "강타";
        card.value = 8;
        card.cost = 2;
        card.color = CardColor.Red;
        card.type = CardType.Attack;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.Monster;

        base.cardInit();
    }

    public override void Use(GameObject target)
    {
        target.GetComponent<Character>().Hit(player.AttackDamageCheck(card.value));
        target.GetComponent<Character>().vulnerable += 2;
        GoCenter();
    }
}
