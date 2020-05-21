using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orichalcum : Artifact
{
    public override void SettingArtifactData()
    {
        data.grade = ArtifactGrade.Nomal;
        data.value = 6;
        data.name = "오리하르콘";
        data.description = "턴 종료 시 방어도가 없다면 방어도를 " + data.value + " 얻습니다.";
    }

    public override void ActiveEffect()
    {
        player.myTurnEnd += getSheild;
    }

    void getSheild()
    {
        if (player.data.shield <= 0)
        {
            player.data.shield += 6;
            player.SettingShieldUI(true);
        }
    }
}
