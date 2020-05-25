using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;



public class ObjectPoolManager : MonoBehaviour
{
    #region 싱글톤
    // 싱글톤 접근용 프로퍼티
    public static ObjectPoolManager instance
    {
        get
        {
            if (m_instance == null) // 싱글톤 변수에 오브젝트가 할당되지 않았다면
            {
                // 씬에서 게임매니저 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<ObjectPoolManager>();
            }

            return m_instance;
        }
    }

    private static ObjectPoolManager m_instance;
    #endregion

    public GameObject cardPrefab;
    public GameObject monsterPrefab;
    public GameObject artifactPrefab;
    public GameObject rewardButtonPrefab;
    public GameObject statusUIPrefab;

    List<Card> cardPool = new List<Card>();
    List<Monster> monsterPool = new List<Monster>();
    public List<Artifact> artifactPool = new List<Artifact>();
    Queue<Reward> rewardButtonPool = new Queue<Reward>();
    Queue<StatusUI> statusUIPool = new Queue<StatusUI>();

    public ObjectPoolList lists;

    void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        SettingJsonData();

        for (int i = 0; i < 50; i++)
        {
            statusUIPool.Enqueue(CreateStatusUI());
        }

        for (int i = 0; i < lists.monsterList.Count; i++)
        {
           monsterPool.Add(CreateNewMonster(lists.monsterList[i]));
        }

        for (int i = 0; i < lists.artifactList.Count; i++)
        {
            artifactPool.Add(CreateArtifact(lists.artifactList[i]));
        }

        for (int i = 0; i < lists.cardList.Count; i++)
        {
            for (int k = 0; k < 5; k++)
            {
               cardPool.Add(CreateCard(lists.cardList[i]));
            }
        }

        for (int i = 0; i < 5; i++)
        {
            rewardButtonPool.Enqueue(CreateButtons());
        }
    }
    void SettingJsonData()
    {
        lists.monsterList = new List<string>();
        lists.artifactList = new List<string>();
        lists.cardList = new List<string>();
        lists.monsterRoomSetting = new List<List<string>>();

        LoadJsonData(ref lists.cardList, "Cards");
        LoadJsonData(ref lists.artifactList, "Artifacts");
        LoadJsonData(ref lists.monsterList, "Monsters");

        LoadRoomData();
    }
    void LoadRoomData()
    {
        int roomNum = 1;
        List<string> temp = new List<string>();

        temp.Add("NobGoblin");
        JsonManager.SaveJsonData(temp, "MonsterRoom", "Elite1");
        temp.Clear();

        temp.Add("Lagavulin");
        JsonManager.SaveJsonData(temp, "MonsterRoom", "Elite2");
        temp.Clear();

        temp.Add("SlimeKing");
        JsonManager.SaveJsonData(temp, "MonsterRoom", "Boss1");
        temp.Clear();

        temp.Add("Cultist");
        JsonManager.SaveJsonData(temp, "MonsterRoom", "Monster"+ roomNum);
        temp.Clear();
        roomNum++;
        temp.Add("LouseRed");
        temp.Add("LouseGreen");
        JsonManager.SaveJsonData(temp, "MonsterRoom", "Monster"+ roomNum);
        temp.Clear();
        roomNum++;
        temp.Add("JawWorm");
        JsonManager.SaveJsonData(temp, "MonsterRoom", "Monster"+ roomNum);
        temp.Clear();
        roomNum++;
        temp.Add("SlimeM");
        temp.Add("SlimeM");
        JsonManager.SaveJsonData(temp, "MonsterRoom", "Monster"+ roomNum);
        temp.Clear();
        roomNum++;
        temp.Add("BlueSlaver");
        JsonManager.SaveJsonData(temp, "MonsterRoom", "Monster"+ roomNum);
        temp.Clear();
        roomNum++;
        temp.Add("Fungi");
        temp.Add("Fungi");
        JsonManager.SaveJsonData(temp, "MonsterRoom", "Monster"+ roomNum);
        temp.Clear();
        roomNum++;
        temp.Add("SlimeL");
        JsonManager.SaveJsonData(temp, "MonsterRoom", "Monster"+ roomNum);
    }

    void LoadJsonData(ref List<string> list, string name)
    {
        if (JsonManager.CheckJsonData("ObjectPoolList", name))
        {
            list = JsonManager.LoadJsonData<List<string>>("ObjectPoolList", name);
        }
        else
        {
            string path = Application.streamingAssetsPath + "/"+ name;
            string[] scripts = Directory.GetFiles(path, "*.cs");

            list.Clear();
            for (int i = 0; i < scripts.Length; i++)
            {
                list.Add(scripts[i].Substring(path.Length + 1).Split('.')[0]);
            }
            JsonManager.SaveJsonData(list, "ObjectPoolList", name);
        }
    }

    #region CREATE

    StatusUI CreateStatusUI()
    {
        StatusUI newUI = Instantiate(statusUIPrefab, transform.Find("StatusUIs")).GetComponent<StatusUI>();
        newUI.gameObject.SetActive(false);
        return newUI;
    }

    Artifact CreateArtifact(string name)
    {
        GameObject test = Instantiate(artifactPrefab, transform.Find("Artifacts"));
        test.AddComponent(System.Type.GetType(name));

        Artifact newArtifact = test.GetComponent<Artifact>();
       // newArtifact.transform.localPosition = new Vector3(2, 2, 2);
        newArtifact.Init();
        newArtifact.gameObject.SetActive(false);
        return newArtifact;
    }

    Card CreateCard(string name)
    {
        GameObject test = Instantiate(cardPrefab, transform.Find("Cards"));
        test.AddComponent(System.Type.GetType(name));

        Card newCard = test.GetComponent<Card>();
       // newCard.transform.localScale = new Vector3(2, 2, 2);
        newCard.cardInit();
        newCard.gameObject.SetActive(false);

        return newCard;
    }

    Reward CreateButtons()
    {
        Reward newButton = Instantiate(rewardButtonPrefab, transform.Find("Buttons")).GetComponent<Reward>();

        newButton.gameObject.SetActive(false);
        return newButton;
    }

    Monster CreateNewMonster(string monsterFileName)
    {
        Debug.Log(monsterFileName+ " 생성 완료");
        Monster newMon = Instantiate(monsterPrefab, transform.Find("Monsters")).AddComponent(System.Type.GetType(monsterFileName)).GetComponent<Monster>();
        newMon.gameObject.SetActive(false);
        newMon.name = monsterFileName;
        return newMon;
    }

    #endregion

    #region GET
    public Artifact GetArtifact(string artifactName)
    {
        Artifact artifact;
        if (artifactPool.Count > 0)
        {
            for (int i = 0; i < artifactPool.Count; i++)
            {
                if (artifactPool[i].GetType().Name == artifactName)
                {
                    artifact = artifactPool[i];
                    artifact.gameObject.SetActive(false);
                    artifactPool.Remove(artifact);

                    return artifact;
                }
            }
            artifact = CreateArtifact(artifactName);
            return artifact;
        }
        artifact = CreateArtifact(artifactName);
        return artifact;
    }

    public Artifact GetArtifact(bool isRandom)
    {
        Artifact artifact;
        if (artifactPool.Count > 0)
        {
            artifact = artifactPool[ Random.Range(0,artifactPool.Count)];
            artifact.gameObject.SetActive(false);
            artifactPool.Remove(artifact);
            return artifact;
        }
        else
        {
            return default;
        }
    }

    public Card GetCard(string cardName)
    {
        Card card;
        if (cardPool.Count > 0)
        {
            for (int i = 0; i < cardPool.Count; i++)
            {
                if (cardPool[i].GetType().Name == cardName)
                {
                    card = cardPool[i];
                    card.gameObject.SetActive(false);
                    cardPool.Remove(card);
                    return card;
                }
            }

            if (cardName.Contains("+"))
            {
                card = CreateCard(cardName.Replace("+",""));
                card.CardUpgrade();
                return card;
            }
            else
            {
                card = CreateCard(cardName);
                return card;
            }
        }

        if (cardName.Contains("+"))
        {
            card = CreateCard(cardName.Replace("+", ""));
            card.CardUpgrade();
            return card;
        }
        else
        {
            card = CreateCard(cardName);
            return card;
        }
    }

    public Monster GetMonster(string monsterName)
    {
        Monster mon;

        if (monsterPool.Count > 0)
        {
            for (int i = 0; i < monsterPool.Count; i++)
            {
                if (monsterPool[i].name == monsterName)
                {
                    Debug.Log("몬스터풀 가져오기 완료");
                    mon = monsterPool[i];
                    mon.gameObject.SetActive(true);
                    monsterPool.Remove(monsterPool[i]);
                    return mon;
                }
            }
            Debug.Log("몬스터 풀에 몬스터가 부족해서 생성합니다");
            mon = CreateNewMonster(monsterName);
            return mon;
        }
        Debug.Log("몬스터풀이 아예 비어있어서 생성합니다");
        mon = CreateNewMonster(monsterName);
        return mon;
    }

    public void GetRewardMoneyButton(string type)
    {
        Reward button = rewardButtonPool.Dequeue();
        button.transform.SetParent(UIManager.instance.reward.transform.Find("Panel").transform);
        button.gameObject.SetActive(true);
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        button.transform.localScale = new Vector3(1, 1, 1);
        button.init(type);
    }
    public StatusUI GetStatusUI(GameObject statusPanel, Sprite sprite )
    {
        StatusUI statusUI = statusUIPool.Dequeue();
        statusUI.transform.position = new Vector3(0, 0, -1);
        statusUI.transform.SetParent(statusPanel.transform);
        statusUI.gameObject.SetActive(true);
        statusUI.Init(sprite);
        return statusUI;
    }
    #endregion

    #region RETURN

    public void ReturnArtifact(Artifact artifact)
    {
        artifactPool.Add(artifact);
        artifact.gameObject.SetActive(false);
        artifact.transform.SetParent(transform.Find("Artifacts"));
    }

    public void ReturnMonster(Monster monster)
    {
        monster.gameObject.SetActive(false);
        monster.transform.SetParent(transform.Find("Monsters"));
        monsterPool.Add(monster);
    }

    public void ReturnCard(Card card)
    {
        cardPool.Add(card);
        // card.cardInit();
        card.gameObject.SetActive(false);
        card.Price = 0;
        card.transform.SetParent(transform.Find("Cards"));
    }

    public void ReturnRewardButton(Reward button)
    {
        rewardButtonPool.Enqueue(button);
        button.gameObject.SetActive(false);
        button.transform.SetParent(transform.Find("Buttons"));
    }
    public void ReturnStatusUI(StatusUI ui)
    {
        statusUIPool.Enqueue(ui);
        ui.gameObject.SetActive(false);
        ui.transform.SetParent(transform.Find("StatusUIs"));
    }
    #endregion
}
[System.Serializable]
public struct ObjectPoolList
{
    public List<string> monsterList;
    public List<string> cardList;
    public List<string> artifactList;
    public List<List<string>> monsterRoomSetting;
}