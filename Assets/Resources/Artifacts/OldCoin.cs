using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCoin : Artifact
{
    public override void SettingArtifactData()
    {
        data.grade = ArtifactGrade.Nomal;
        data.value = 300;
        data.name = "낡은 동전";
        data.description = "획득시 " + data.value + " 골드를 얻습니다.";
    }

    public override void ActiveEffect()
    {
        player.data.money += data.value;
        UIManager.instance.SettingUI();
    }
}
