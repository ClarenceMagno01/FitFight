using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands.Card
{
    public class CopyCardToDiscardPileCommand : CardBaseCommand
    {
        public override async UniTask ExecuteImmediate()
        {
            await base.ExecuteImmediate();
            if (CardDataVar == null)
            {
                Debug.LogError("CopyCardToDiscardPileCommand: CardDataVar is null");
                return;
            }
          
            EventBus<CardSystemEvents.CopyCardToDiscardPileEvent>.Raise(new CardSystemEvents.CopyCardToDiscardPileEvent
            {
                Data = CardDataVar
            });
        }
    }
}