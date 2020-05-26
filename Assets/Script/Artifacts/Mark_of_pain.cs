using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark_of_pain : Artifact
{
    public override void SettingArtifactData()
    {
        data.grade = ArtifactGrade.Boss;
        data.value = 1;
        data.name = "고통의 표식";
        data.description = "매 턴 시작 시 코스트를 " + data.value + "얻습니다. 전투를 시작할 때 2장의 부상을 뽑을 카드 더미에 넣습니다.";
    }

    public override void ActiveEffect()
    {
        GameManager.instance.currentCost += data.value;
        GameManager.instance.maxCost += data.value;
        player.battleStart += AddWound;
    }

    void AddWound()
    {
        for (int i = 0; i < 2; i++)
        {
            Card newcard = ObjectPoolManager.instance.GetCard("Wound");
            newcard.transform.SetParent(GameManager.instance.myDeck.transform);
        }
        GameManager.ShuffleDeck(GameManager.instance.myDeck);
    }
}
