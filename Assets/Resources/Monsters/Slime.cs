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

        if (data.currentHP <= 0)
        {
            Dead();
        }
    }

    public void Split(string monsterName , int cnt , int hp , int money)
    {
        for (int i = 0; i < cnt; i++)
        {
            var monster = ObjectPoolManager.instance.GetMonster(monsterName);
            monster.gameObject.SetActive(true);
            monster.transform.position = GameObject.Find("MonsterSpawnPoints").transform.
                GetChild(Random.Range(0, GameObject.Find("MonsterSpawnPoints").transform.childCount)).transform.position;
            monster.Init(hp, money);
            GameManager.instance.monsters.Add(monster);
        }
      
        GameManager.instance.monsters.Remove(this);
        GameManager.instance.currentRoomMoney += data.money;
        ObjectPoolManager.instance.ReturnMonster(this);
        UIManager.instance.SettingUI();
    }

    public virtual void Debuff()
    {

    }
}
