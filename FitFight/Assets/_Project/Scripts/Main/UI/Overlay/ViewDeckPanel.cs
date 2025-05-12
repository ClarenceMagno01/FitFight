using System;
using System.Collections.Generic;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Utilities.Scripts;
using UnityEngine;

namespace _Project.Scripts.Main.UI
{
    public class ViewDeckPanel : MonoBehaviour
    {
        [SerializeField] private CardUI _cardUIPrefab;
        [SerializeField] private Transform _cardParent;
        
        private PrefabPool<CardUI> _cardUIPool;

        private void Awake()
        {
            _cardUIPool = new PrefabPool<CardUI>(_cardUIPrefab, _cardParent,30);
        }
        
        public void DisplayCards(List<CardData> data)
        {
            DestroyAllChildren(_cardParent);
            foreach (var card in data)
            {
                var cardUI = _cardUIPool.Get();
                cardUI.SetData(card);
                cardUI.ShowPrice(false);
            }
        }
        
        private void DestroyAllChildren(Transform tr)
        {
            for (int i = tr.childCount - 1; i >= 0; i--)
            {
                CardUI cardUI = tr.GetChild(i).GetComponent<CardUI>();
                _cardUIPool.Release(cardUI);
            }
        }
    }
}