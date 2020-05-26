using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NobGoblin : Monster
{
    bool isSkullBash;

    bool isFirst;

    private void OnEnable()
    {
        int hp = Random.Range(83, 87);
        int money = Random.Range(83, 87);
        Init(hp, money);
        isFirst = true;
        spain.AnimationName = "animation";
        ActionCheck();
        GameManager.instance.isEliteRoom = true;

    }

    public override void ActionCheck()
    {
        if (isFirst)
        {
            isAttack = false;
            FindIntentImage("buff");
            return;
        }

        isAttack = true;

        if (player.Vulnerable <= 0)
        {
            isSkullBash = true;
        }
        else
        {
            isSkullBash = false;
        }


        if (isSkullBash)
        {
            damage = 6;
            FindIntentImage("attackDebuff");
        }
        else
        {
            damage = 14;
            FindIntentImage("attack3");
        }
    }

    public override void Action()
    {
        if (isFirst)
        {
            Bellow();
            isFirst = false;
            return;
        }

        if (isSkullBash)
        {
            SkullBash();
        }
        else
        {
            Attack();
        }

        Animation("Atk", true);
        intentImage.gameObject.SetActive(false);
    }

    public override void Attack()
    {
        player.Hit(AttackDamageCheck());
    }

    void SkullBash()
    {
        player.Hit(AttackDamageCheck());
        player.Vulnerable += 2;
    }

    void Bellow()
    {
        FindObjectOfType<Player>().skillUseEvent += PowerUp;
        PropertySet("격분", Resources.LoadAll<Sprite>("Sprite/powers")[123], 2, "스킬 카드를 사용하면 힘을 2 얻습니다.");
    }

    void PowerUp()
    {
        Power += 1;
        SoundManager.instance.PlaySound("Buff");
    }

    public override void Dead()
    {
        base.Dead();
        FindObjectOfType<Player>().skillUseEvent -= PowerUp;
        GameManager.instance.eliteKill++;
    }
}
