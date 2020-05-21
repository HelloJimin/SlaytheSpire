using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mango : Artifact
{
    public override void SettingArtifactData()
    {
        data.grade = ArtifactGrade.Nomal;
        data.value = 14;
        data.name = "망고";
        data.description = "획득 시 최대 체력 +" + data.value;
    }

    public override void ActiveEffect()
    {
        HpUp();
    }

    void HpUp()
    {
        player.data.maxHP += data.value;
        player.data.currentHP += data.value;
        player.SettingHPUI();
        UIManager.instance.SettingUI();
    }
}
