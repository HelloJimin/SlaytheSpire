using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wound : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "부상";
        card.value = 0;
        card.cost = 0;
        card.color = CardColor.Gray;
        card.type = CardType.CC;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.All;
        card.description = "사용불가";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        ObjectPoolManager.instance.ReturnCard(this);
        Card newcard = ObjectPoolManager.instance.GetCard("Wound");
        newcard.transform.SetParent(GameManager.instance.myHand.transform);
        newcard.gameObject.SetActive(true);
    }

    public override void CardUpgrade()
    {
        card.value = 9;
        base.CardUpgrade();
    }

    public override IEnumerator StopCard(Character target)
    {
        yield return new WaitForSeconds(0.1f);

        if (usedAnime.activeSelf)
        {
            StartCoroutine(StopCard(target));
        }
    }
}
