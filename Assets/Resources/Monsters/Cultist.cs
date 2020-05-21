using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cultist : Monster
{
    bool isFirstTurn;

    bool IsFirstTurn { get { return isFirstTurn; } set { isFirstTurn = value; PropertySet("의식", Resources.LoadAll<Sprite>("Sprite/powers")[6], 3, "매 턴 힘이 3 증가합니다."); } }

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
        ResetStatusUI();
    }

    public override void ActionCheck()
    {
        FindIntentImage("buff");
    }

    public override void Action()
    {
        if (IsFirstTurn)
        {
            myTurnEnd += Ritual;
            isAttack = true;
            SettingDamageUI();
            IsFirstTurn = false;
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
