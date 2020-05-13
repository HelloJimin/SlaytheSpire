using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
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

    public GameObject monsterPrefab;
    List<Character> monsterPool = new List<Character>();

    void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        string[] monsterFiles = { "NobGoblin" , "Cultist" };

        for (int i = 0; i < monsterFiles.Length; i++)
        {
          monsterPool.Add(CreateNewMonster(monsterFiles[i]));
        }
    }

    Character CreateNewMonster(string monsterFileName)
    {
        Debug.Log(monsterFileName);
        Character newMon = Instantiate(monsterPrefab,transform).AddComponent(System.Type.GetType(monsterFileName)).GetComponent<Character>();
        newMon.gameObject.SetActive(false);
        newMon.name = monsterFileName;
        return newMon;
    }

    public Character GetMonster(string monsterName)
    {
        Character mon;

        if (monsterPool.Count > 0)
        {
            for (int i = 0; i < monsterPool.Count; i++)
            {
                Debug.Log("몬스터풀 가져오기 완료");
                if (monsterPool[i].name == monsterName)
                {
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

    public void ReturnMonster(Character monster)
    {
        monster.gameObject.SetActive(false);
        monster.transform.SetParent(transform);
        monsterPool.Add(monster);
    }
}
