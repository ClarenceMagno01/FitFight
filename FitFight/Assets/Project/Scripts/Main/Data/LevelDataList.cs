using System;
using System.Collections.Generic;
using _Project.Scripts.Main.Game.Entity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Main.Data
{
    [Serializable]
    public struct Rewards
    {
        public int gold;
        public bool hasRandomRelic;
        public int numberOfCardChoices;
        [Tooltip("If true, the game will give you a random card from the list of cards")]
        public bool specificRandomCardPool;
        [ShowIf("specificRandomCardPool")] public CardData[] cardPool;
    }
    
    [Serializable]
    public struct LevelData
    {
        public GameObject[] enemiesPrefab;
        public Rewards rewards;
    }
    
    [CreateAssetMenu(fileName = "LevelDataList", menuName = "DataList/LevelDataList")]
    public class LevelDataList : ScriptableObject
    {
        public LevelData[] dataList;
    }
}