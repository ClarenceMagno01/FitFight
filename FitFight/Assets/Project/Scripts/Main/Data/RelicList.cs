using UnityEngine;

namespace _Project.Scripts.Main.Data
{
    [CreateAssetMenu(fileName = "RelicList", menuName = "DataList/RelicList")]
    public class RelicList : ScriptableObject
    {
        public RelicData[] list;
    }
}