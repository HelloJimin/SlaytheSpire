using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;

public class Character : MonoBehaviour
{
    public SkeletonAnimation spain;
    public CharacterData data;
    public bool isPlayer;
    public int vulnerable;
    public int weak;
    private int frail;
    private MouseDescription mouseDescription;

    public Dictionary<string , StatusUI> statusUIs = new Dictionary<string, StatusUI>();

    #region 프로퍼티

    public virtual int Vulnerable
    {
        get
        {
            return vulnerable;
        }
        set
        {
            vulnerable = value;
            PropertySet("vulnerable", Resources.LoadAll<Sprite>("Sprite/powers")[109], vulnerable , "취약 상태. 대미지를 50% 추가로 받습니다.");
            if (vulnerable >0)
            {
               SoundManager.instance.PlaySound("Debuff");
            }
        }
    }

    public virtual int Weak
    {
        get
        {
            return weak;
        }
        set
        {
            weak = value;
            PropertySet("weak", Resources.LoadAll<Sprite>("Sprite/powers")[147], weak,"약화 상태. 대미지가 30% 감소합니다.");
            if (weak > 0)
            {
                SoundManager.instance.PlaySound("Debuff");
            }
        }
    }

    public virtual int Frail
    {
        get
        {
            return frail;
        }
        set
        {
            frail = value;
            PropertySet("frail", Resources.LoadAll<Sprite>("Sprite/powers")[64] , frail , "손상 상태. 카드로부터 얻는 방어도가 25% 감소합니다.");
            if (frail > 0)
            {
                SoundManager.instance.PlaySound("Debuff");
            }
        }
    }

    public virtual int Power
    {
        get
        {
            return data.power;
        }
        set
        {
            data.power = value;
            PropertySet("power", Resources.LoadAll<Sprite>("Sprite/powers")[112] , data.power, "공격력이 증가합니다." );
            if (value > 0)
            {
                SoundManager.instance.PlaySound("Buff");
            }
            if (value < 0)
            {
                SoundManager.instance.PlaySound("Debuff");
            }
        }
    }

    public virtual int Dexterity
    {
        get
        {
            return data.dexterity;
        }
        set
        {
            data.dexterity = value;
            PropertySet("dexterity", Resources.LoadAll<Sprite>("Sprite/powers")[18], data.dexterity , "방어력이 증가합니다.");
            if (value > 0)
            {
                SoundManager.instance.PlaySound("Buff");
            }
            if (value < 0)
            {
                SoundManager.instance.PlaySound("Debuff");
            }
        }
    }
    #endregion

    public delegate void MyTurnEnd();
    public event MyTurnEnd myTurnEnd;
    public delegate void MyTurnStart();
    public event MyTurnStart myTurnStart;
    public delegate void YourTurnEnd();
    public event YourTurnEnd yourTurnEnd;

    public SkeletonAnimation anime;

    public Text HPText;

    public GameObject statusPanel;



    public virtual void Start()
    {
        HPText = transform.Find("Canvas/MyHP/HPText").GetComponent<Text>();
        anime = GetComponent<SkeletonAnimation>();
        statusPanel = transform.Find("Canvas/StatusPanel").gameObject;
        yourTurnEnd += CoolDown;
        yourTurnEnd += ShieldBreak;
        SettingHPUI();
    }

    public virtual void Hit(float damage)
    {
        damage = DefenseDamageCheck(damage);
        
        data.currentHP -= (int)damage;
        if (data.currentHP<0) data.currentHP = 0;

        SettingHPUI();
        //ShieldBreak();
        UIManager.instance.CameraShake();
        SoundManager.instance.PlaySound("FastAtk1");

        GameObject hudText = Instantiate(UIManager.instance.hudDamageTextPrefab); // 생성할 텍스트 오브젝트
        hudText.transform.SetParent(UIManager.instance.battleUI.transform.parent);
        hudText.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,3,0)); // 표시될 위치
        hudText.GetComponent<DamageText>().damage = (int)damage; // 데미지 전달
    }

    public int DefenseDamageCheck(float damage)
    {
        float Damage = damage;

        if (data.shield>0)
        {
            float temp = Damage;
            Damage -= data.shield;
            data.shield -= (int)temp;

            if (data.shield <= 0)
            {
                data.shield = 0;
                SettingShieldUI(false);
                SoundManager.instance.PlaySound("DefenseBreak");
            }
            else
            {
                SettingShieldUI(true);
            }
        }

        if (Damage < 0) Damage = 0;

        return (int)Damage;
    }
    public void GetHeal(int heal)
    {
        if ((data.currentHP + heal) >= data.maxHP)
        {
            data.currentHP = data.maxHP;
        }
        else
        {
            data.currentHP += heal;
        }
        SoundManager.instance.PlaySound("Heal");
        UIManager.instance.SettingUI();
    }

    public void GetShield(int sheild)
    {
        data.shield += sheild + data.dexterity;
        if (frail > 0) data.shield -= (int)(data.shield * 0.25f);
        SoundManager.instance.PlaySound("GainDefense");
        SettingShieldUI(true);
    }

    public void ShieldBreak()
    {
        if (data.shield > 0) return;

        SettingShieldUI(false);
    }

    public void SettingShieldUI(bool isShield)
    {
        if (isShield)
        {
            transform.Find("Canvas").transform.Find("MyHP").GetComponent<Outline>().enabled = true;
            transform.Find("Canvas/MyHP/HPBar").GetComponent<Image>().color = Color.blue;
            transform.Find("Canvas/MyHP/SheildImage").gameObject.SetActive(true);
            transform.Find("Canvas/MyHP/SheildImage").GetComponentInChildren<Text>().text = data.shield.ToString();
        }
        else
        {
            transform.Find("Canvas/MyHP/SheildImage").gameObject.SetActive(false);
            transform.Find("Canvas/MyHP/HPBar").GetComponent<Image>().color = Color.red;
            transform.Find("Canvas").transform.Find("MyHP").GetComponent<Outline>().enabled = false;
        }
    }

    public void MyEndPhase()
    {
        if (myTurnEnd == null) return;
        myTurnEnd.Invoke();
    }

    public virtual void YourEndPhase()
    {
        data.shield = 0;
        yourTurnEnd.Invoke();
    }

    public IEnumerator MyStartPhase(float delay)
    {
        yield return new WaitForSeconds(delay);
        myTurnStart.Invoke();
    }

    void CoolDown()
    {
        if (Frail > 0) Frail--;
        if (Vulnerable > 0) Vulnerable--;
        if (Weak > 0) Weak--;
    }

    public virtual void Animation(string aniName, bool isLeft)
    {
        float position;

        if (isLeft)
        {
            position = -0.5f;
        }
        else
        {
            position = 1;
        }

        if (aniName == "Atk")
        {
            transform.DOMove(new Vector3(transform.position.x + position, transform.position.y, transform.position.z), 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Flash);
        }
        else
        {

        }
    }

    public void SettingHPUI()
    {
        HPText.text = data.currentHP + "/" + data.maxHP;
        HPText.transform.parent.Find("HPBar").GetComponent<Image>().fillAmount = (float)data.currentHP / (float)data.maxHP;
    }

    public void PropertySet(string Key , Sprite sprite, int value, string disc)
    {
        string key = Key;
        if (!statusUIs.ContainsKey(key))
        {
            StatusUI newStatusUI = ObjectPoolManager.instance.GetStatusUI(transform.Find("Canvas/StatusPanel").gameObject, sprite);
            statusUIs.Add(key, newStatusUI);
            statusUIs[key].GetComponent<CommonTouchUI>().description = disc;
        }

        if (statusUIs.ContainsKey(key))
        {
            statusUIs[key].SettingUI(value);
        }

        if (value == 0)
        {
            RemoveStatusUI(key);
        }

    }

    public void RemoveStatusUI(string key)
    {
        if (statusUIs.ContainsKey(key))
        {
            ObjectPoolManager.instance.ReturnStatusUI(statusUIs[key]);
            statusUIs.Remove(key);
        }
    }

    public void DataInit()
    {
        Dexterity = 0;
        Power = 0;
        Vulnerable = 0;
        Weak = 0;
        Frail = 0;
    }

    public void AnimeOneShotStart(string name)
    {
        string original = spain.AnimationName;
        spain.AnimationState.SetAnimation(0, name, false);
        spain.AnimationState.AddAnimation(0, original, true, 0f);
        //StartCoroutine(returnAnime(original));
    }

    public void AnimeChangeStartForDelay(string name, bool loop, float delay)
    {
        spain.AnimationState.AddAnimation(0, name, loop, delay);
    }
    public void AnimeChangeStart(string name, bool loop)
    {
        spain.AnimationState.SetAnimation(0, name, loop);
    }

}

[System.Serializable]
public struct CharacterData
{
    public string name;
    public int currentHP;
    public int maxHP;

    public int shield;
    public int power;
    public int dexterity;
    public int money;
}