using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region 싱글톤
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null) // 싱글톤 변수에 오브젝트가 할당되지 않았다면
            {
                // 씬에서 게임매니저 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance;
    #endregion

    public GameObject alphaImage;
    public GameObject usedCardAnime;
    public GameObject powerZone;
    public Text costText;
    public Text myDeckText;
    public Text myCemeteryText;
    public Text myHPText;
    public Text myMoneyText;


    public bool isChoiceMode;
    public GameObject choicePanel;
    public int choiceSize;

    public GameObject battleSystem;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }

    public void SettingUI()
    {
        costText.text = GameManager.instance.currentCost + "/" + GameManager.instance.maxCost;
        myDeckText.text = GameManager.instance.myDeck.transform.childCount.ToString();
        myCemeteryText.text = GameManager.instance.myCemetary.transform.childCount.ToString();
        myHPText.text = GameManager.instance.player.data.currentHP + "/"+ GameManager.instance.player.data.maxHP;
        myMoneyText.text = GameManager.instance.player.data.money.ToString();
    }
}
