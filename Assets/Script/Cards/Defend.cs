using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : Card
{
    private void Start()
    {
        card.name = "수비";
        card.value = 5;
        card.cost = 1;
        card.color = CardColor.Red;
        card.type = CardType.Skill;
        card.grade = CardGrade.Nomal;
        card.cardImagePath = "Sprite/CardImage/defend";

        SpriteSetting();
        JsonManager.SaveJsonData(card, "Card", GetType().Name);
    }
    public override void Use(GameObject target)
    {
        if (target.GetComponent<PlayerData>() == null) return;
        Debug.Log("전:" + target.GetComponent<PlayerData>().data.sheild);
        target.GetComponent<PlayerData>().data.sheild += card.value;
        Debug.Log("후:" + target.GetComponent<PlayerData>().data.sheild);
    }
}
