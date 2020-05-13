using System.Collections;
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

        base.cardInit();
    }

    public override void Use(Character target)
    {
        target = GameManager.instance.player;
        target.GetShield(card.value);

        if (card.isUpgrade)
        {
            for (int i = 0; i < GameManager.instance.myHand.transform.childCount; i++)
            {
                var card = GameManager.instance.myHand.transform.GetChild(i).GetComponent<Card>();
                card.CardUpgrade();
            }
            GoCenter();
        }
        else
        {
            UIManager.instance.ChoiceMode(true, 1);
            UIManager.instance.alphaImage.transform.Find("OkButton").GetComponent<Button>().onClick.RemoveAllListeners();
            UIManager.instance.alphaImage.transform.Find("OkButton").GetComponent<Button>().onClick.AddListener(UseEffect);
            transform.parent.SetParent(UIManager.instance.alphaImage.transform);
            transform.SetParent(null);
        }

       // GoCenter();
    }

    public void UseEffect()
    {
        Card target;

        target = UIManager.instance.choicePanel.GetComponentsInChildren<Card>()[0];
        target.CardUpgrade();
        target.transform.SetParent(GameManager.instance.myHand.transform);

        UIManager.instance.ChoiceMode(false);
        GameManager.instance.myHand.transform.SetParent(UIManager.instance.battleSystem.transform);

        GoCenter();
    }
}
