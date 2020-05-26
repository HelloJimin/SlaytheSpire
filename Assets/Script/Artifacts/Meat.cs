using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : Artifact
{
    public override void SettingArtifactData()
    {
        data.grade = ArtifactGrade.Nomal;
        data.value = 12;
        data.name = "고기 덩어리";
        data.description = "전투 종료 시 체력이 50% 이하라면 체력을 "+  data.value + " 회복합니다.";
    }

    public override void ActiveEffect()
    {
        player.battleEnded += heal;
    }

    void heal()
    {
        if (player.data.maxHP*0.5f >= player.data.currentHP)
        {
            player.GetHeal(data.value);
        }
    }
}
