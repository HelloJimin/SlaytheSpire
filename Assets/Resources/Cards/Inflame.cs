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
        base.cardInit();
    }

    public override void Use(GameObject target)
    {
        GameManager.instance.player.data.power += card.value;
        GoCenter();
    }

    public override void CardUpgrade()
    {
        card.value = 3;
        base.CardUpgrade();
    }

    public override void GoCenter()
    {
        transform.DOLocalMove(new Vector3(100, 600, 0), 0.2f)
    .OnComplete(() =>
    {
        usedAnime.SetActive(true);
        UsedCard(UIManager.instance.powerZone.transform);
        UIManager.instance.SettingUI();
    });
    }
}
