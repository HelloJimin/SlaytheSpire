using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marbles : Artifact
{
    public override void SettingArtifactData()
    {
        data.grade = ArtifactGrade.Nomal;
        data.value = 1;
        data.name = "구슬 주머니";
        data.description = "전투 시작 시 적 전체에게 취약을 " + data.value + " 부여합니다.";
    }

    public override void ActiveEffect()
    {
        player.battleStart += VulnerableUP;
    }

    void VulnerableUP()
    {
        var monsters = GameManager.instance.monsters;
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].Vulnerable += 1;
        }
    }
}
