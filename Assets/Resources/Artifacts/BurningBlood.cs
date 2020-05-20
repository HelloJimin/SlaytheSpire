using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningBlood : Artifact
{
    public override void SettingArtifactData()
    {
        data.grade = ArtifactGrade.start;
        data.value = 6;
        data.name = "불타는 혈액";
        data.description = "전투 종료 시 체력을 " + data.value + " 회복합니다.";
    }

    public override void ActiveEffect()
    {
        player.battleEnded += Heal;
    }

    void Heal()
    {
        player.GetHeal(data.value);
    }
}
