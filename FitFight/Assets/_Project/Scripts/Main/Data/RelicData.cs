using System;
using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Game.Exercise;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Main.Data
{
    [Serializable]
    public struct RelicInfo
    {
        public string name; 
        [Multiline(4)] public string description;
    }
    
    [Serializable]
    public struct RelicAssets
    {
        [SerializeField] public Sprite image;
        [SerializeReference] public ICommand[] commands;
    }
    
    [CreateAssetMenu(fileName = "RelicData", menuName = "Data/RelicData")]
    public class RelicData : ScriptableObject
    {
        [ReadOnly] public string id = System.Guid.NewGuid().ToString(); 
        public RelicInfo info;
        public int price = 9999;
        public RelicAssets assets;
    }
}