using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Main.Data
{
    [CreateAssetMenu(fileName = "CardList", menuName = "DataList/CardList")]
    public class Cards : ScriptableObject
    {
        public CardData[] list;
    }
}