using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : Card
{
    private void Start()
    {
        card.name = "타격";
        card.value = 6;
        card.cost = 1;
        card.color = CardColor.Red;
        card.type = CardType.Attack;
        card.grade = CardGrade.Nomal;
        card.cardImagePath = "Sprite/CardImage/strike";

        SpriteSetting();
        JsonManager.SaveJsonData(card, "Card", GetType().Name);
    }
    public override void Use(GameObject target)
    {
        if (target.GetComponent<Monster>() == null) return;
        Debug.Log("전:" + target.GetComponent<Monster>().data.currentHP);
        target.GetComponent<Monster>().data.currentHP -= card.value;
        Debug.Log("후:" + target.GetComponent<Monster>().data.currentHP);
    }
}
