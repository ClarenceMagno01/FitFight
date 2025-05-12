using System;
using System.Collections.Generic;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Game.Entity;

namespace _Project.Scripts.Main.Game.Card
{
    public interface ICard
    {
        void Activate(Action onActivated = null);
        CardData Data { get; set; }
    }
}