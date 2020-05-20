using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Inflame : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "발화";
        card.value = 2;
        card.cost = 1;
        card.color = CardColor.Red;
        card.type = CardType.Power;
        card.grade = CardGrade.Rare;
        card.target = CardTarget.All;
        card.description = "힘을 " + card.value + " 얻습니다.";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        player.Power += card.value;
        Debug.Log(player.Power);
       // GoCenter();
    }

    public override void CardUpgrade()
    {
        card.value = 3;
        card.description = "힘을 " + card.value + " 얻습니다.";
        base.CardUpgrade();
    }

    public override void GoCenter(Character target)
    {
        transform.DOLocalMove(new Vector3(100, 1700, 0), 0.2f)
        .OnComplete(() =>
    {
        Use(target);
        StartCoroutine(StopCard(player));
        ObjectPoolManager.instance.ReturnCard(this);
        UIManager.instance.SettingUI();
    });
    }
}
