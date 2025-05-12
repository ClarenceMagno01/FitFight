using System;
using System.Collections.Generic;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.UI;
using _Project.Scripts.Main.Utilities.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Main
{
    public class CardRemovalPanel : MonoBehaviour
    {
        [SerializeField] private Color _highlightColor;
        
        [SerializeField] private CardUI _cardUIPrefab;
        [SerializeField] private Transform _cardParent;
        
        private PrefabPool<CardUI> _cardUIPool;
        private GameManager _gm;
        private ICardRemovalListener _listener;

        private void Awake()
        {
            _gm = GameManager.Instance;
            _cardUIPool = new PrefabPool<CardUI>(_cardUIPrefab, _cardParent,30);
        }
        
        public void SetListener(ICardRemovalListener listener) => _listener = listener;

        private void Start()
        {
            DisplayCards(_gm.CurrentCards);
        }

        private void DisplayCards(List<CardData> data)
        {
            DestroyAllChildren(_cardParent);
            foreach (var card in data)
            {
                var cardUI = _cardUIPool.Get();
                cardUI.SetData(card);
                cardUI.ShowPrice(false);
                AttachButton(cardUI);
            }
            
            void AttachButton(CardUI card)
            {
                GameObject go = card.gameObject;
                Button btn = go.AddComponent<Button>();
                btn.colors = new ColorBlock
                {
                    normalColor = btn.colors.normalColor,
                    highlightedColor = _highlightColor,
                    pressedColor = btn.colors.pressedColor,
                    selectedColor = btn.colors.selectedColor,
                    disabledColor = btn.colors.selectedColor,
                    colorMultiplier = btn.colors.colorMultiplier,
                    fadeDuration = btn.colors.fadeDuration
                };
                btn.onClick.AddListener(()=> _listener?.OnClickCardForRemoval(card.Data,go));
            }
        }
        
        private void DestroyAllChildren(Transform tr)
        {
            for (int i = tr.childCount - 1; i >= 0; i--)
            {
                Transform child = tr.GetChild(i);
                CardUI cardUI = child.GetComponent<CardUI>();
                _cardUIPool.Release(cardUI);
            }
        }
    }
}