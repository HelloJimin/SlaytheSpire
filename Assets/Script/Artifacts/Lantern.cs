using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : Artifact
{
    bool isFirstTurn;

    public override void SettingArtifactData()
    {
        isFirstTurn = false;
        data.grade = ArtifactGrade.Boss;
        data.value = 1;
        data.name = "전등";
        data.description = "전투 시작 후 첫 턴에 코스트를 " + data.value + "얻습니다.";
    }

    public override void ActiveEffect()
    {
        Debug.Log("발동");
        player.myTurnStart += CostUp;
        player.battleEnded += FirstTurnReset;
    }

    void CostUp()
    {
        if (isFirstTurn)
        {
            return;
        }
        isFirstTurn = true;
        GameManager.instance.currentCost += data.value;
        UIManager.instance.SettingUI();
    }

    private void FirstTurnReset()
    {
        isFirstTurn = false;
    }
}
