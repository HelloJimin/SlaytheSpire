using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cultist : Monster
{
    bool isFirstTurn;

    private void OnEnable()
    {
        Init(1, 10);
        spain.AnimationName = "rally";
        isFirstTurn = true;
        damage = 6;
        myTurnEnd -= PowerUP;
        isAttack = false;
        SettingDamageUI();
        ActionCheck();
    }

    public override void ActionCheck()
    {
        FindIntentImage("buff");
    }

    public override void Action()
    {
        if (isFirstTurn)
        {
            myTurnEnd += Ritual;
            isAttack = true;
            SettingDamageUI();
            isFirstTurn = false;
            FindIntentImage("attack2");
            SkeletonAnimeStart("waving", false);
        }
        else
        {
            Attack();
            Animation("Atk", true);
        }

    }

    public override void Attack()
    {
        player.Hit(AttackDamageCheck());
    }

    void PowerUP()
    {
        data.power += 3;
        Debug.Log("지금파워:" + data.power);
    }
    void Ritual()
    {


        myTurnEnd += PowerUP;
        myTurnEnd -= SettingDamageUI;
        myTurnEnd += SettingDamageUI;
        myTurnEnd -= Ritual;
    }
}
