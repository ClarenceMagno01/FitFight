using _Project.Scripts.Main.Data;
using UnityEngine;

namespace _Project.Scripts.Main
{
    public interface ICardRemovalListener
    {
        void OnClickCardForRemoval(CardData card,GameObject go);
    }
}