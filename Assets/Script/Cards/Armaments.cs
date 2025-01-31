﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Armaments : Card
{
    public override void cardInit()
    {
        usedAnime = UIManager.instance.usedCardAnime;
        card.name = "전투장비";
        card.value = 5;
        card.cost = 1;
        card.color = CardColor.Red;
        card.type = CardType.Skill;
        card.grade = CardGrade.Nomal;
        card.target = CardTarget.All;
        card.description = "방어도를 5 얻습니다. 카드 1 장을 이번 전투동안 강화합니다.";
        base.cardInit();
    }

    public override void Use(Character target)
    {
        base.Use(target);

        target = GameManager.instance.player;
        target.GetShield(card.value);

        if (card.isUpgrade)
        {
            for (int i = 0; i < GameManager.instance.myHand.transform.childCount; i++)
            {
                var card = GameManager.instance.myHand.transform.GetChild(i).GetComponent<Card>();
                card.CardUpgrade();
            }
          //  GoCenter();
        }
        else
        {
            UIManager.instance.choice = ChoiceMode.Upgrade;
            UIManager.instance.ChoiceMode(true, 1);
            UIManager.instance.alphaImage.transform.Find("OkButton").GetComponent<Button>().onClick.RemoveAllListeners();
            UIManager.instance.alphaImage.transform.Find("OkButton").GetComponent<Button>().onClick.AddListener(UseEffect);
            transform.parent.SetParent(UIManager.instance.alphaImage.transform);
            transform.SetParent(null);
        UIManager.instance.turnEndButton.SetActive(false);
        }

       // GoCenter();
    }

    public void UseEffect()
    {
        Card target;

        target = UIManager.instance.choicePanel.GetComponentsInChildren<Card>()[0];
        target.CardUpgrade();
        target.transform.SetParent(GameManager.instance.myHand.transform);


        UIManager.instance.turnEndButton.SetActive(true);
        UIManager.instance.ChoiceMode(false);
        UIManager.instance.choice = ChoiceMode.Grab;
        GameManager.instance.myHand.transform.SetParent(UIManager.instance.battleSystem.transform);

     //   GoCenter();
    }
    public override void CardUpgrade()
    {
        card.description = "방어도를 5 얻습니다. 손에 있는 모든 카드를 이번 전투동안 강화합니다.";
        base.CardUpgrade();
    }
}
