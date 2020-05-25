using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        #region 싱글톤
        // 싱글톤 접근용 프로퍼티
        public static MapManager instance
        {
            get
            {
                if (m_instance == null) // 싱글톤 변수에 오브젝트가 할당되지 않았다면
                {
                    // 씬에서 게임매니저 오브젝트를 찾아서 할당
                    m_instance = FindObjectOfType<MapManager>();
                }

                return m_instance;
            }
        }

        private static MapManager m_instance;
        #endregion


        public MapConfig config;
        public MapView view;

        public Map CurrentMap { get; private set; }

        private void Awake()
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey("Map"))
            {
                var mapJson = PlayerPrefs.GetString("Map");
                var map = JsonConvert.DeserializeObject<Map>(mapJson);
                // using this instead of .Contains()
                if (map.path.Any(p => p.Equals(map.GetBossNode().point)))
                {
                    // payer has already reached the boss, generate a new map
                    GenerateNewMap();
                }
                else
                {
                    CurrentMap = map;
                    // player has not reached the boss yet, load the current map
                    view.ShowMap(map);
                }
            }
            else
            {
                GenerateNewMap();
            }

            //view.firstParent.SetActive(false);
            view.firstParent.transform.position = new Vector3(2, -4.5f, -1);

            //보스아이콘 위치 내려주는코드
            Transform[] gameObjects = view.firstParent.transform.Find("MapParentWithAScroll").GetComponentsInChildren<Transform>();
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i].GetComponentInChildren<SpriteRenderer>()!=null )
                {
                    SpriteRenderer nodeImage = gameObjects[i].GetComponentInChildren<SpriteRenderer>();

                    if (nodeImage.sprite.name == "slime")
                    {
                        nodeImage.transform.localPosition = new Vector3(nodeImage.transform.localPosition.x, -0.3f, nodeImage.transform.localPosition.z);
                        break;
                    }
                }
            }
        }

        public void GenerateNewMap()
        {
            var map = MapGenerator.GetMap(config);
            CurrentMap = map;
            view.ShowMap(map);
        }

        public void SaveMap()
        {
            if (CurrentMap == null) return;

            var json = JsonConvert.SerializeObject(CurrentMap);
            PlayerPrefs.SetString("Map", json);
            PlayerPrefs.Save();
        }

        //private void OnApplicationQuit()
        //{
        //    SaveMap();
        //}
    }
}
