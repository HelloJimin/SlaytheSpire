using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : Card
{
    public override void Use(GameObject target)
    {
       // target.GetComponent<Character>().data.sheild += card.value;
        target.GetComponent<Character>().GetShield(card.value);
        Debug.Log("실드량:" + target.GetComponent<Character>().data.shield);

        GoCenter();
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
        card.target = CardTarget.Player;
        base.cardInit();
    }
}
