using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningBlood : Artifact
{
    public override void SettingArtifactData()
    {
        data.value = 6;
        data.name = "불타는 혈액";
        data.description = "전투 종료 시 체력을 " + data.value + " 회복합니다.";
    }

    public override void ArtifactEffect()
    {
        //배틀 엔드일때 힐해줘야함;
       // player.myTurnEnd += Heal;
    }

    void Heal()
    {
        if ((player.data.currentHP += data.value) >= player.data.maxHP)
        {
            player.data.currentHP = player.data.maxHP;
        }
        else
        {
           player.data.currentHP += data.value;
        }
    }
}
