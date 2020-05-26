using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood_vial : Artifact
{
    public override void SettingArtifactData()
    {
        data.grade = ArtifactGrade.Nomal;
        data.value = 2;
        data.name = "피가 담긴 병";
        data.description = "전투 시작 시 체력을 " + data.value + " 회복합니다.";
    }

    public override void ActiveEffect()
    {
        player.battleStart += Heal;
    }

    void Heal()
    {
        player.GetHeal(data.value);
    }
}
