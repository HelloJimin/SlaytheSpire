using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    public enum CurrentAction
    {
        Debuff,
        Waiting,
        Attack,
        AttackDebuff,
        Split
    }

    public CurrentAction action;

    public override void Attack()
    {
        player.Hit(AttackDamageCheck());
        Animation("Atk", true);
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
        if (data.currentHP <= data.maxHP * 0.5f)
        {
            action = CurrentAction.Split;
            ActionCheck();
        }
    }

    public virtual void Split(string monsterName , int cnt , int hp , int money)
    {
        GameManager.instance.monsters.Remove(GameManager.instance.getMyKey(this));
        for (int i = 0; i < cnt; i++)
        {
            var monster = ObjectPoolManager.instance.GetMonster(monsterName);
            monster.gameObject.SetActive(true);

            for (int k = 0; k < GameObject.Find("MonsterSpawnPoints").transform.childCount; k++)
            {
                if (!GameManager.instance.monsters.ContainsKey(k))
                {
                    monster.transform.position = GameObject.Find("MonsterSpawnPoints").transform.GetChild(k).transform.position;
                    monster.Init(hp, money);
                    GameManager.instance.monsters.Add(k,monster);
                    break;
                }
            }
        }

        //GameManager.instance.monsters.Remove(this);
        GameManager.instance.currentRoomMoney += data.money;
        ObjectPoolManager.instance.ReturnMonster(this);
        UIManager.instance.SettingUI();
    }

    public virtual void Debuff()
    {

    }
}
