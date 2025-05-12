using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Extension;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.Utilities.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.Main.UI
{
    public class CardRewardPicker : MonoBehaviour
    {
        [SerializeField] private CardUI _cardPrefab;
        [SerializeField] private Transform _cardsParent;
        [SerializeField] private Color _highlightColor;
        [SerializeField] private Button _btnSkip;
        
        private void OnEnable()
        {
            _btnSkip.onClick.AddListener(OnClickSkip);
        }

        private void OnDisable()
        {
            _btnSkip.onClick.RemoveListener(OnClickSkip);
        }

        public void ShowChoices(CardData[] cardPool,int numberOfChoice)
        {
            _cardsParent.DestroyAllChildren();
            var randomNums = UniqueRandomGenerator.GetUniqueRandomNumbers(numberOfChoice, 0, cardPool.Length);
            foreach (var i in randomNums)
            {
                CardUI card = Instantiate(_cardPrefab, _cardsParent);
                card.SetData(cardPool[i]);
                card.ShowPrice(false);
                AttachButton(card);
            }
            void AttachButton(CardUI card)
            {
                Button btn = card.gameObject.AddComponent<Button>();
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
                btn.onClick.AddListener(()=> OnClickCard(card.Data));
            }
        }

        private void OnClickCard(CardData card)
        {
            GameManager.Instance.AddCard(card);
            gameObject.SetActive(false);
            SceneManager.LoadScene("MapScene");
        }
        
        private void OnClickSkip()
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene("MapScene");
        }
    }
}