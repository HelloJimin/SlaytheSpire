using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{
    #region 싱글톤
    // 싱글톤 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            if (m_instance == null) // 싱글톤 변수에 오브젝트가 할당되지 않았다면
            {
                // 씬에서 게임매니저 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }

    private static GameManager m_instance;
    #endregion

    public bool isClear;
    bool isPlayerTurn;

    public int currentCost;
    public int maxCost;

    public List<string> myInventoryList = new List<string>();
    public List<string> myArtifactList = new List<string>();

    public GameObject myDeck;
    public GameObject myHand;
    public GameObject myCemetary;
    public GameObject myExhaustZone;
    public GameObject myArtifact;

    public Player player;
    public GameObject cardPrefab;

    //public List<Monster> monsters = new List<Monster>();
    public Dictionary<int, Monster> monsters = new Dictionary<int, Monster>();

    int currentFloor;
    public int CurrentFloor { get { return currentFloor; } set { currentFloor = value; if(value != 0) UIManager.instance.curretnFloorText.transform.parent.gameObject.SetActive(true);  UIManager.instance.curretnFloorText.text = currentFloor.ToString(); } }

    public PlayingData saveData;

    public int allMoney;
    public int monsterKill;
    public int eliteKill;
    
    //public delegate void BattleEndProcess();
    // public event BattleEndProcess battleEndProcess;

    public int currentRoomMoney;
    //GameObject battleUI;
    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
        player = FindObjectOfType<Player>();
        maxCost = 3;
        currentRoomMoney = 0;
        CurrentFloor = 0;
        isClear = false;
    }

    void Start()
    {
        // battleUI = UIManager.instance.powerZone.transform.parent.gameObject;
        myInventoryList = player.inventoryList;
        myArtifactList = player.artifactList;

        player.myTurnStart += StartPhase;
        UIManager.instance.SettingUI();
        UIManager.instance.GoToNeowRoom();


        if (JsonManager.CheckJsonData("PlayingData", "saveData"))
        {
            saveData = JsonManager.LoadJsonData<PlayingData>("PlayingData", "saveData");
            myInventoryList = saveData.myInventoryList;
            myArtifactList = saveData.myArtifactList;
            ArtifactSetting();

            player.data.currentHP = saveData.hp;
            player.data.maxHP = saveData.maxHp;
            player.data.money = saveData.money;
            monsterKill = saveData.monsterKill;
            eliteKill = saveData.eliteKill;
            allMoney = saveData.getAllMoney;
            CurrentFloor = saveData.floor;

            UIManager.instance.SettingUI();
            UIManager.instance.GoToMap();
            return;
        }
        ArtifactSetting();
    }

    public void StartGame(string level)
    {
        currentRoomMoney = 0;
        SettingMonsters(level);
        isPlayerTurn = true;
        SettingMyDeck(myDeck);
        player.BattleStart();
        TurnProcessing();
        UIManager.instance.SettingUI();
    }

    public void SavePlayingData()
    {
        SoundManager.instance.option.SaveOption();
        saveData.floor = CurrentFloor;
        saveData.hp = player.data.currentHP;
        saveData.maxHp = player.data.maxHP;
        saveData.money = player.data.money;
        saveData.getAllMoney = allMoney;
        saveData.monsterKill = monsterKill;
        saveData.eliteKill = eliteKill;
        saveData.myInventoryList = myInventoryList;
        saveData.myArtifactList = myArtifactList;
        JsonManager.SaveJsonData(saveData, "PlayingData", "saveData");
    }

    public void SettingMyDeck(GameObject after)
    {
        //인벤토리에서 덱으로
        for (int i = 0; i < myInventoryList.Count; i++)
        {
            // inventory[i].transform.SetParent(myDeck.transform);
            Card myCard = ObjectPoolManager.instance.GetCard(myInventoryList[i]);
            myCard.transform.SetParent(after.transform);
        }
        ShuffleDeck(myDeck);
    }

    public void AllTransferCards(GameObject before, GameObject after, bool active)
    {
        int retunCnt = before.transform.childCount;

        for (int i = 0; i < retunCnt; i++)
        {
            before.transform.GetChild(0).gameObject.SetActive(active);
            before.transform.GetChild(0).SetParent(after.transform);
        }
    }

    public static void ShuffleDeck(GameObject deck)
    {
        int random;

        for (int i = 0; i < deck.transform.childCount; ++i)
        {
            random = Random.Range(0, deck.transform.childCount);

            deck.transform.GetChild(i).SetSiblingIndex(random);
        }
    }

    public void Draw()
    {
        if (myDeck.transform.childCount <= 0)
        {
            AllTransferCards(myCemetary, myDeck, false);
            ShuffleDeck(myDeck);
        }
        myDeck.transform.GetChild(0).gameObject.SetActive(true);
        myDeck.transform.GetChild(0).SetParent(myHand.transform);
    }

    void TurnProcessing()
    {
        if (isPlayerTurn)
        {
            SoundManager.instance.PlaySound("PlayerTurn");
            StartCoroutine(player.MyStartPhase(1.0f));
        }
        else
        {
            Debug.Log("적턴");
            SoundManager.instance.PlaySound("EnemyTurn");

            int tum = 0;
            foreach (var item in monsters)
            {
                StartCoroutine(item.Value.MyStartPhase(1.0f + (0.5f * tum)));
                tum++;
            }
            //for (int i = 0; i < monsters.Count; i++)
            //{
            //    StartCoroutine(monsters[i].MyStartPhase(1.0f + (0.5f * i)));
            //}
            StartCoroutine(EndPhase());
        }
        UIManager.instance.SettingUI();
    }

    IEnumerator EndPhase()
    {
        yield return new WaitForSeconds(2f);
        ChangeTurn();
    }

    void StartPhase()
    {
        Debug.Log("내턴");
        currentCost = maxCost;
        while (myHand.transform.childCount < 5)
        {
            Draw();
        }
        UIManager.instance.SettingUI();
    }

    public void ChangeTurn()
    {
        if (isPlayerTurn)
        {
            SoundManager.instance.PlaySound("EndTurn");
            AllTransferCards(myHand, myCemetary, false);
            player.MyEndPhase();
            //for (int i = 0; i < monsters.Count; i++)
            //{
            //    monsters[i].YourEndPhase();
            //}
            foreach (var item in monsters)
            {
                item.Value.YourEndPhase();
            }
        }
        else
        {
            //for (int i = 0; i < monsters.Count; i++)
            //{
            //    monsters[i].MyEndPhase();
            //}
            foreach (var item in monsters)
            {
                item.Value.MyEndPhase();
            }
            player.YourEndPhase();
        }

        isPlayerTurn = !isPlayerTurn;
        TurnProcessing();
    }

    void SettingMonsters(string level)
    {
        int roomNumber=0;

        if (level == "Monster")
        {
            roomNumber = Random.Range(1, 8);
        }
        else if(level == "Elite")
        {
            roomNumber = Random.Range(1, 3);
        }
        else if (level == "Boss")
        {
            roomNumber = 1;
        }

        List<string> temp = JsonManager.LoadJsonData<List<string>>("MonsterRoom", level + roomNumber);
        for (int i = 0; i < temp.Count; i++)
        {
            Monster monster = ObjectPoolManager.instance.GetMonster(temp[i]);
            monster.gameObject.SetActive(true);
            //for (int k = 0; k < spons.Length; k++)
            //{
            //    if (spons[i] == false)
            //    {
            //        monster.transform.position = GameObject.Find("MonsterSpawnPoints").transform.GetChild(i).transform.position;
            //        spons[i] = true;
            //        break;
            //    }
            //}
            monster.transform.position = GameObject.Find("MonsterSpawnPoints").transform.GetChild(i).transform.position;
            monsters.Add(i,monster);
        }
    }

    public void BattleEnd()
    {
        if (monsters.Count != 0) return;
        // battleEndProcess.Invoke();

        player.data.power = 0;
        player.YourEndPhase();
        player.BattleEnd();

        ObjectPoolManager.instance.GetRewardMoneyButton("money");
        ObjectPoolManager.instance.GetRewardMoneyButton("nomal");
        currentCost = maxCost;
        DeckReset(myDeck);
        DeckReset(myHand);
        DeckReset(myCemetary);
        UIManager.instance.OpenRewardPanel();
        UIManager.instance.usedCardAnime.SetActive(false);
        SoundManager.instance.PlaySound("Victory");
    }

    public void DeckReset(GameObject before)
    {
        //int retunCnt = before.transform.childCount;

        //for (int i = 0; i < retunCnt; i++)
        //{
        //    ObjectPoolManager.instance.ReturnCard(before.transform.GetChild(0).GetComponent<Card>());
        //}
        while (before.transform.childCount != 0)
        {
            ObjectPoolManager.instance.ReturnCard(before.transform.GetChild(0).GetComponent<Card>());
        }
    }

    public void UpgradeInventoryCard()
    {
        string cardName = UIManager.instance.restUpgradePanel.transform.Find("ChoicePanel").GetComponentInChildren<Card>().GetType().Name;
        for (int i = 0; i < myInventoryList.Count; i++)
        {
            if (myInventoryList[i] == cardName)
            {
                myInventoryList[i] = myInventoryList[i] + "+";
                break;
            }
        }

        for (int i = 0; i < myInventoryList.Count; i++)
        {
            Debug.Log(myInventoryList[i]);
        }

        UIManager.instance.choice = ChoiceMode.Grab;
        UIManager.instance.alphaImage.SetActive(false);
        UIManager.instance.RestCardGoToCardPool();
        UIManager.instance.restUpgradePanel.SetActive(false);
        UIManager.instance.restRoom.SetActive(false);
        SoundManager.instance.PlaySound("UpgradeCard");
    }
    public void RestUpgradeCancelButton()
    {
        GameObject choicePanel = UIManager.instance.restUpgradePanel.transform.Find("ChoicePanel").gameObject;
        UIManager.instance.restUpgradePanel.SetActive(false);
        UIManager.instance.choice = global::ChoiceMode.RestUpgrade;
        Card[] cards = choicePanel.GetComponentsInChildren<Card>();

        ObjectPoolManager.instance.ReturnCard(cards[1]);
        cards[0].transform.SetParent(FindObjectOfType<MyAllcardList>().transform.Find("AllCards"));
    }
    public void MonstersDamageUIUpdate()
    {
        //for (int i = 0; i < monsters.Count; i++)
        //{
        //    monsters[i].SettingDamageUI();
        //}
        foreach (var item in monsters)
        {
            item.Value.SettingDamageUI();
        }
    }

    public int getMyKey(Monster monster)
    {
        var key= monsters.FirstOrDefault(x => x.Value == monster).Key;
        return key;
    }

    void ArtifactSetting()
    {
        for (int i = 0; i < myArtifactList.Count; i++)
        {
            Artifact artifact = ObjectPoolManager.instance.GetArtifact(myArtifactList[i]);
            artifact.transform.SetParent(myArtifact.transform);
            artifact.gameObject.SetActive(true);
            artifact.ActiveEffect();
        }
    }
}

[System.Serializable]
public struct PlayingData
{
    public List<string> myArtifactList;
    public List<string> myInventoryList;
    public int hp;
    public int maxHp;
    public int money;
    public int floor;
    public int monsterKill;
    public int eliteKill;
    public int getArtifactCnt;
    public int getAllMoney;
    public int volume;
}