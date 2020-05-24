using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fungi : Monster
{
    private void OnEnable()
    {
        int hp = Random.Range(23, 29);
        int money = Random.Range(23, 29);
        Init(hp, money);
        spain.AnimationName = "Idle";
        ActionCheck();
        PropertySet("Spore", Resources.LoadAll<Sprite>("Sprite/powers")[67], 2, "이 몬스터를 쓰러뜨리면 취약을 2 얻습니다.");
    }

    public override void ActionCheck()
    {
        isAttack = Random.value > 0.4f;

        if (isAttack)
        {
            damage = 6;
            FindIntentImage("attack1");
        }
        else
        {
            FindIntentImage("buff");
        }
    }

    public override void Action()
    {
        if (isAttack)
        {
            Attack();
        }
        else
        {
            Grow();
        }
    }

    public override void Attack()
    {
        player.Hit(AttackDamageCheck());
        AnimeOneShotStart("Attack");
        Animation("Atk", true);
    }

    void Grow()
    {
        Power += 3;
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
        AnimeOneShotStart("Hit");
    }

    public override void Dead()
    {
        player.Vulnerable += 2;
        base.Dead();
    }
}
