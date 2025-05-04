using Map;
using UnityEngine;

namespace _Project.Scripts.Main.Game
{
    public enum NodeStatus
    {
        Locked,
        Unlocked,
    }
    
    public class NodeState
    {
        public Node Node { get; private set; }
        public NodeStatus Status { get; set; }

        public void SetNewNode(Node node)
        {
            Status = NodeStatus.Locked;
            Node = node;
        }

        public void Reset()
        {
            Status = NodeStatus.Locked;
            Node = null;
        }
    }
}