using System;
using System.Collections.Generic;
using _Project.Scripts.Main.Game.Commands;
using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Game.Exercise;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Main.Data
{
    public enum CardType
    {
        Attack,
        Power,
        Skills,
    }
    
    [Serializable]
    public struct CardInfo
    {
        public int requiredReps;
        public string name; //Exercise name
        [Multiline(4)] public string description;
        public CardType type;
        public int price;
    }
    
    [Serializable]
    public struct CardAssets
    {
        [SerializeReference] public ExerciseBase exercise;
        [SerializeReference] public ICommand[] commands;
    }
     
    [CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData")]
    public class CardData : ScriptableObject
    {
        [ReadOnly] public string id = System.Guid.NewGuid().ToString(); 
        public CardInfo info;
        public CardAssets assets;
        [Header("Play Conditions")]
        public bool canPickTarget; //All targeting or random don't need this
        public bool isOneTimeUse; //Exhaust or Discard card once used
    } 
}