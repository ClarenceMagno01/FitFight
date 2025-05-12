using System;
using System.Linq;
using _Project.Scripts.Main.Game;
using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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

        private void OnEnable()
        {
            EventBus<MapEvent.OnShowEvent>.Register(OnShowMap);
        }

        private void OnDisable()
        {
            EventBus<MapEvent.OnShowEvent>.Deregister(OnShowMap);
        }

        private void OnShowMap(MapEvent.OnShowEvent ev)
        {
            NodeState nodeState = GameManager.Instance.CurrentNodeState;
            if (nodeState.Status == NodeStatus.Unlocked && nodeState.Node != null)
            {
                MapNode node = view.GetNode(nodeState.Node.point);
                UnlockNode(node);
                nodeState.Reset();
            }
        }
        
        private void UnlockNode(MapNode mapNode)
        {
            Locked = false;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            //   mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();
            //  DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
        }

        public void SelectNode(MapNode mapNode)
        {
            if (Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    EnterNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                Vector2Int currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                Node currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                    EnterNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }
        
        
        private static void EnterNode(MapNode mapNode)
        {
            // we have access to blueprint name here as well
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false
            switch (mapNode.Node.nodeType)
            {
                case NodeType.MinorEnemy:
                case NodeType.EliteEnemy:
                case NodeType.Boss:
                    MMSoundManagerAllSoundsControlEvent.Trigger(MMSoundManagerAllSoundsControlEventTypes.Free);
                    GameManager.Instance.SetLevelDataByType(mapNode.Node.nodeType);
                    GameManager.Instance.CurrentNodeState.SetNewNode(mapNode.Node);
                    SceneManager.LoadScene("Bootstrapper");
                    break;
                case NodeType.RestSite:
                    break;
                case NodeType.Treasure:
                    break;
                case NodeType.Store:
                    GameManager.Instance.CurrentNodeState.SetNewNode(mapNode.Node);
                    SceneManager.LoadScene("ShopScene");
                    break;
                case NodeType.Mystery:
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