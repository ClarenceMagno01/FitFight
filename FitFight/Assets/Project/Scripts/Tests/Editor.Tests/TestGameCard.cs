using System;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Game.Card;
using UnityEngine;

namespace _Project.Scripts.Tests.Editor.Tests
{
    internal class TestGameCard : ICard
    {
        internal TestGameCard(CardData data)
        {
            Data = data;
        }
        
        public void Activate(Action onActivated = null)
        {
            Debug.Log("Activated");
        }

        public CardData Data { get; set; }
    }
}