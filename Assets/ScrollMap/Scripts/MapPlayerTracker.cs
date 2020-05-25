using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Map
{
    public class MapPlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelecting = false;
        public float enterNodeDelay = 1f;
        public MapManager mapManager;
        public MapView view;

        public static MapPlayerTracker Instance;

        public bool Locked { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SelectNode(MapNode mapNode)
        {
            if (Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            //mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();
            EnterNode(mapNode);
            //DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
        }

        private static void EnterNode(MapNode mapNode)
        {
            // we have access to blueprint name here as well
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false
            GameManager.instance.CurrentFloor++;

            switch (mapNode.Node.nodeType)
            {
                case NodeType.MinorEnemy:
                    UIManager.instance.GoToMonsterRoom();
                    SoundManager.instance.PlaySound("BattleStart");
                    break;
                case NodeType.EliteEnemy:
                    UIManager.instance.GoToEliteRoom();
                    SoundManager.instance.PlaySound("BattleStart");
                    break;
                case NodeType.RestSite:
                    UIManager.instance.GoToRestRoom();
                    break;
                case NodeType.Treasure:
                    UIManager.instance.GoToChestRoom();
                    break;
                case NodeType.Store:
                    UIManager.instance.GoToShopRoom();
                    SoundManager.instance.PlaySound("Shoper");
                    break;
                case NodeType.Boss:
                    UIManager.instance.GoToBossRoom();
                    SoundManager.instance.PlaySound("BossStart");
                    break;
                case NodeType.Mystery:
                    int random = UnityEngine.Random.Range(0, 5);
                    switch (random)
                    {
                        case 0:
                            UIManager.instance.GoToMonsterRoom();
                            SoundManager.instance.PlaySound("BattleStart");
                            break;
                        case 1:
                            UIManager.instance.GoToChestRoom();
                            break;
                        case 2:
                            UIManager.instance.GoToShopRoom();
                            break;
                        case 3:
                            Debug.Log("3번");
                            break;
                        case 4:
                            Debug.Log("4번");
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}