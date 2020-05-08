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

        JsonManager.SaveJsonData(card, "Card", GetType().Name);
        SpriteSetting();
    }
    public override bool Use(GameObject target)
    {
        if (target == null || target.GetComponent<Monster>() == null) return false;

        Debug.Log("전:" + target.GetComponent<Monster>().data.currentHP);
        target.GetComponent<Monster>().data.currentHP -= card.value;
        Debug.Log("후:" + target.GetComponent<Monster>().data.currentHP);

       // StartCoroutine(GoCenter());
        transform.SetParent(transform.parent.parent);
        transform.gameObject.SetActive(false);
        Debug.Log(card.color);
        return true;
    }
}
