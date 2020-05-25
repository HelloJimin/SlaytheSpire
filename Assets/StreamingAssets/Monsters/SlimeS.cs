using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeS : Monster
{
    private void OnEnable()
    {
        spain.Initialize(true);
        int hp = Random.Range(9, 13);
        int money = Random.Range(9, 13);
        Init(hp, money);
        AnimeChangeStart("idle", true);
        ActionCheck();
    }

    public override void ActionCheck()
    {
        isAttack = Random.value > 0.3f;

        if (isAttack)
        {
            damage = 3;
            FindIntentImage("attack1");
        }
        else
        {
            FindIntentImage("debuff1");
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
            Lick();
        }
    }

    public override void Attack()
    {
        player.Hit(AttackDamageCheck());
        Animation("Atk", true);
    }

    void Lick()
    {
        player.Weak += 1;
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
        AnimeOneShotStart("damage");
    }
}
