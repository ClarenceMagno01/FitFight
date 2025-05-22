using System;
using System.Collections.Generic;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Utilities.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

namespace _Project.Scripts.Main.UI
{
    public class ViewDeckPanel : MonoBehaviour
    {
        [Header("Card Display")]
        [SerializeField] private CardUI _cardUIPrefab;
        [SerializeField] private Transform _cardParent;

        [Header("Video Display")]
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [SerializeField] private TextMeshProUGUI _videoNameText;

        private PrefabPool<CardUI> _cardUIPool;
        private List<CardData> _cardDataList;
        private int _currentVideoIndex;

        private void Awake()
        {
            _cardUIPool = new PrefabPool<CardUI>(_cardUIPrefab, _cardParent, 30);

            // Button listeners
            _nextButton.onClick.AddListener(ShowNextVideo);
            _previousButton.onClick.AddListener(ShowPreviousVideo);

            // Make sure VideoPlayer renders to RawImage
            if (_videoPlayer && _rawImage)
                _videoPlayer.targetTexture = new RenderTexture(1920, 1080, 0); // Set resolution as needed
        }

        public void DisplayCards(List<CardData> data)
        {
            DestroyAllChildren(_cardParent);
            _cardDataList = data;
            _currentVideoIndex = 0;

            foreach (var card in data)
            {
                var cardUI = _cardUIPool.Get();
                cardUI.SetData(card);
                cardUI.ShowPrice(false);
            }

            ShowVideoAtIndex(_currentVideoIndex);
        }

        private void DestroyAllChildren(Transform tr)
        {
            for (int i = tr.childCount - 1; i >= 0; i--)
            {
                CardUI cardUI = tr.GetChild(i).GetComponent<CardUI>();
                _cardUIPool.Release(cardUI);
            }
        }

        private void ShowVideoAtIndex(int index)
        {
            if (_cardDataList == null || _cardDataList.Count == 0 || index < 0 || index >= _cardDataList.Count)
                return;

            var card = _cardDataList[index];
            if (_videoPlayer != null && card.videoClip != null)
            {
                _videoPlayer.clip = card.videoClip;
                _videoPlayer.Play();

                // Set video name
                if (_videoNameText != null)
                {
                    _videoNameText.text = card.videoClip.name;
                }
            }

            if (_rawImage != null && _videoPlayer.targetTexture != null)
            {
                _rawImage.texture = _videoPlayer.targetTexture;
            }
        }

        public void ShowNextVideo()
        {
            if (_cardDataList == null || _cardDataList.Count == 0) return;

            _currentVideoIndex = (_currentVideoIndex + 1) % _cardDataList.Count;
            ShowVideoAtIndex(_currentVideoIndex);
        }

        public void ShowPreviousVideo()
        {
            if (_cardDataList == null || _cardDataList.Count == 0) return;

            _currentVideoIndex = (_currentVideoIndex - 1 + _cardDataList.Count) % _cardDataList.Count;
            ShowVideoAtIndex(_currentVideoIndex);
        }
    }
}
