using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Extension;
using _Project.Scripts.Main.Game;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.UI;
using _Project.Scripts.Main.Utilities.Scripts;
using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.Main
{
    public class ShopUIController : MonoBehaviour, ICardRemovalListener
    {
        private const int CardRemovePrice = 75;
        
        [SerializeField] private Color _highlightColor;
        [SerializeField] private MMF_Player _buyFeedback;
        
        [Header("Buttons")]
        [SerializeField] private Button _btnLeave;
        [SerializeField] private Button _btnCardRemoval;
        
        [Header("Cards")]
        [SerializeField] private GameObject _cards;
        [SerializeField] private Cards _cardDatabase;
        
        [Header("Relics")]
        [SerializeField] private Transform _relicsParent;
        [SerializeField] private RelicIcon _relicPrefab;
        
        [Header("Card Removal")]
        [SerializeField] private CardRemovalPanel _cardRemovalPanel;
        [SerializeField] private GameObject _removeCardPopup;
        [SerializeField] private Button _btnConfirmRemove;
        [SerializeField] private Button _btnCancelRemove;
        [SerializeField] private TMP_Text _txtCardRemovePrice;
        [SerializeField] private TMP_Text _txtCardRemoveService;

        private UnityAction _confirmRemoveAction;
 
        private GameManager _gm;

        private void Awake()
        {
            _gm = GameManager.Instance;
        }
        
        private void OnEnable()
        {
            _btnCardRemoval.onClick.AddListener(OnClickCardRemoval);
            _btnLeave.onClick.AddListener(OnClickLeave);
            _btnCancelRemove.onClick.AddListener(OnCancelCardRemove);
        }

        private void OnDisable()
        {
            _btnCardRemoval.onClick.RemoveListener(OnClickCardRemoval);
            _btnLeave.onClick.RemoveListener(OnClickLeave);
            _btnCancelRemove.onClick.RemoveListener(OnCancelCardRemove);
        }

        private void Start()
        {
            LoadScenesAsync().Forget();
            InitPurchasableCards();
            InitPurchasableRelics();
            WaitForLayout().Forget();
            
            _cardRemovalPanel.SetListener(this);
            _txtCardRemovePrice.text = _gm.DiscountedPrice(CardRemovePrice).ToString();
        }
        
        private async UniTaskVoid LoadScenesAsync()
        {
            await SceneManager.LoadSceneAsync("OverlayUI",LoadSceneMode.Additive);
            await GameManager.Instance.LoadInputConfig();
        }

        #region Purchasable Cards
        
        private void InitPurchasableCards()
        {
            var randomNumbers = UniqueRandomGenerator.GetUniqueRandomNumbers(5, 0, _cardDatabase.list.Length);
            for (int i = _cards.gameObject.transform.childCount - 1; i >= 0; i--)
            {
                CardUI cardUi = _cards.transform.GetChild(i).GetComponent<CardUI>();
                cardUi.SetData(_cardDatabase.list[randomNumbers[i]]);
                cardUi.ShowPrice(true);
                AttachButton(cardUi);
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
                btn.onClick.AddListener(()=> OnClickCard(card.Data,go));
            }
        }
        
        private void OnClickCard(CardData card,GameObject go)
        {
            int price = GameManager.Instance.DiscountedPrice(card.info.price);
            if (_gm.Gold >= price)
            {
                _buyFeedback.PlayFeedbacks();
                _gm.AddCard(card);
                _gm.DecreaseGold(price);
                Destroy(go);
            }
            else
            {
                Debug.Log("Not enough gold!");
            }
        }
        
        #endregion
        
        #region Purchasable Relics

        private void InitPurchasableRelics()
        {
            _relicsParent.DestroyAllChildren();
            RelicData[] relicDatas =  RelicPoolManager.Instance.GetRandomPurchasableRelics(3);
            if (relicDatas.Length > 0)
            {
                for (int i = 0; i < relicDatas.Length; ++i)
                {
                    RelicData data = relicDatas[i];
                    RelicIcon icon = Instantiate(_relicPrefab, _relicsParent);
                    icon.SetData(data);
                    icon.ShowPrice(true);
                    AttachButton(icon);
                }
            }
            
            
            void AttachButton(RelicIcon icon)
            {
                GameObject go = icon.gameObject;
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
                btn.onClick.AddListener(()=> OnClickRelic(icon.Data,go));
            }
        }
        
        private async UniTask WaitForLayout()
        {
            await UniTask.WaitForEndOfFrame();
            _relicsParent.GetComponent<GridLayoutGroup>().enabled = false;
        }

        private void OnClickRelic(RelicData relic,GameObject go)
        {
            int price = GameManager.Instance.DiscountedPrice(relic.price);
            if (_gm.Gold >= price)
            {
                _buyFeedback.PlayFeedbacks();
                _gm.AddRelic(relic);
                _gm.DecreaseGold(price);
                Destroy(go);
            }
            else
            {
                Debug.Log("Not enough gold!");
            }
        }
        
        #endregion

        private void OnClickCardRemoval()
        {
            _cardRemovalPanel.gameObject.SetActive(true);
        }
        
        public void OnClickCardForRemoval(CardData card, GameObject go)
        {
            _removeCardPopup.SetActive(true);
            _confirmRemoveAction = () => OnConfirmCardRemove(card, go);
            _btnConfirmRemove.onClick.AddListener(_confirmRemoveAction);
        }

        #region Card Remove Confirmation Popup

        private void OnConfirmCardRemove(CardData card, GameObject go)
        {
            int price = GameManager.Instance.DiscountedPrice(CardRemovePrice);
            if (_gm.Gold >= price)
            {
                _buyFeedback.PlayFeedbacks();
                _gm.RemoveCard(card);
                _gm.DecreaseGold(price);
                Destroy(go);
                
                _txtCardRemoveService.text = "Sold Out!";
                _btnConfirmRemove.onClick.RemoveListener(_confirmRemoveAction);
                _btnCardRemoval.enabled = false;
                _removeCardPopup.SetActive(false);
                _cardRemovalPanel.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Not enough gold!");
            }
        }

        private void OnCancelCardRemove()
        {
            if (_confirmRemoveAction != null) 
                _btnConfirmRemove.onClick.RemoveListener(_confirmRemoveAction);
            _removeCardPopup.SetActive(false);
        }

        #endregion
        
        private void OnClickLeave()
        {
            GameManager.Instance.CurrentNodeState.Status = NodeStatus.Unlocked;
            SceneManager.LoadScene("MapScene");
        }

       
    }
}