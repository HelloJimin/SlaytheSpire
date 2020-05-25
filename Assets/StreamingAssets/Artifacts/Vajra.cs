using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vajra : Artifact
{
    public override void SettingArtifactData()
    {
        data.grade = ArtifactGrade.Nomal;
        data.value = 1;
        data.name = "바주라";
        data.description = "힘을 " + data.value + " 얻은 채로 전투를 시작합니다.";
    }

    public override void ActiveEffect()
    {
        player.battleStart += PowerUp;
    }

    void PowerUp()
    {
        player.Power += data.value;
        UIManager.instance.SettingUI();
    }
}
