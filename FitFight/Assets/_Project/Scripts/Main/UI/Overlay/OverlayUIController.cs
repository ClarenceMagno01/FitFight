using _Project.Scripts.Main.Extension;
using _Project.Scripts.Main.Game;
using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Game.PlayerTurn;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static _Project.Scripts.Main.Game.Events.OverlayUIEvents;

namespace _Project.Scripts.Main.UI
{
    public class OverlayUIController : MonoBehaviour
    {
        [Header("Text")] 
        [SerializeField] private TMP_Text _txtPlayerName;
        [SerializeField] private TMP_Text _txtHealth;
        [SerializeField] private TMP_Text _txtGold;
        [SerializeField] private TMP_Text _txtNumberOfCards;

        [Header("Buttons")] 
        [SerializeField] private Button _btnViewDeck;
        [SerializeField] private Button _btnSettings;
        [SerializeField] private Button _btnSurrender;
        [SerializeField] private Button _btnMainMenu;

        [Header("Panels")] 
        [SerializeField] private SettingsPanel _panelSettings;
        [SerializeField] private ViewDeckPanel _panelViewDeck;
        [SerializeField] private GameLosePanel _losePanel;
        
        [Header("Relic Bar")]
        [SerializeField] private GameObject _relicBar;
        [SerializeField] private RelicIcon _relicIconPrefab;
        
        private GameManager _gm;
        
        private void Awake()
        {
            _gm = GameManager.Instance;
        }
        
        private void OnEnable()
        {
            EventBus<InvalidateRelicBarEvent>.Register(InvalidateRelicBar);
            EventBus<UpdateHealthEvent>.Register(UpdateHealth);
            EventBus<UpdateGoldEvent>.Register(UpdateGold);
            EventBus<UpdateNumberOfCardsEvent>.Register(UpdateNumberOfCards);
            EventBus<ShowLosePanelEvent>.Register(ShowLosePanel);

            _btnViewDeck.onClick.AddListener(OnClickViewDeck);
            _btnSettings.onClick.AddListener(OnClickSettings);
            _btnSurrender.onClick.AddListener(OnClickSurrender);
            _btnMainMenu.onClick.AddListener(OnClickMainMenu);
        }

        private void OnDisable()
        {
            EventBus<InvalidateRelicBarEvent>.Deregister(InvalidateRelicBar);
            EventBus<UpdateHealthEvent>.Deregister(UpdateHealth);
            EventBus<UpdateGoldEvent>.Deregister(UpdateGold);
            EventBus<UpdateNumberOfCardsEvent>.Deregister(UpdateNumberOfCards);
            EventBus<ShowLosePanelEvent>.Deregister(ShowLosePanel);
            
            _btnSettings.onClick.RemoveListener(OnClickSettings);
            _btnViewDeck.onClick.RemoveListener(OnClickViewDeck);
            _btnSurrender.onClick.RemoveListener(OnClickSurrender);
            _btnMainMenu.onClick.RemoveListener(OnClickMainMenu);
        }

        private void Start()
        {
            InvalidateUI();
        }

        private void InvalidateUI()
        {
            _txtPlayerName.text = _gm.CharacterName;
            _txtHealth.text = $"{_gm.CurrentPlayerHealth}/{_gm.CurrentMaxHealth}";
            _txtGold.text = _gm.Gold.ToString();
            _txtNumberOfCards.text = _gm.CurrentCards.Count.ToString();
            InvalidateRelicBar();
        }

        private void InvalidateRelicBar(InvalidateRelicBarEvent ev = default)
        {
            _relicBar.transform.DestroyAllChildren();
            foreach (var relic in _gm.CurrentRelics)
            {
                RelicIcon relicIcon =  Instantiate(_relicIconPrefab, _relicBar.transform);
                relicIcon.SetData(relic);
                relicIcon.ShowPrice(false);
            }
        }

        private void UpdateHealth(UpdateHealthEvent ev)
        {
            _txtHealth.text = $"{ev.CurrentHealth}/{ev.MaxHealth}";
        }
        
        private void UpdateGold(UpdateGoldEvent ev) =>  _txtGold.text = _gm.Gold.ToString();
    
        private void UpdateNumberOfCards(UpdateNumberOfCardsEvent ev) =>  _txtNumberOfCards.text = _gm.CurrentCards.Count.ToString();

        private void ShowLosePanel(ShowLosePanelEvent ev)
        {
            _losePanel.gameObject.SetActive(ev.IsShow);
            _losePanel.SetGameSummary(ev.Summary);
        }
        
        #region Button Clicks

        private void OnClickSettings()
        {
            if(CardSystemController.IsDrawingCards)
                return;
            _panelSettings.gameObject.SetActive(true);
            EventBus<GameEvents.PauseGameEvent>.Raise(new GameEvents.PauseGameEvent());
        }

        private void OnClickSurrender()
        {
            if (GameController.IsGameStarted)
            {
                if(CardSystemController.IsDrawingCards)
                    return;
                _panelSettings.gameObject.SetActive(false);
                EventBus<GameEvents.PlayerLoseEvent>.Raise(new GameEvents.PlayerLoseEvent()); //Should Open Lose Panel
            }
            else
            {
                ShowLosePanel(new ShowLosePanelEvent
                {
                    IsShow = true,
                    Summary = new GameSummary
                    {
                        TotalCards = _gm.CurrentCards.Count,
                        TotalRelics = _gm.CurrentRelics.Count,
                        Gold = _gm.Gold
                    }
                });
            }
        }

        private void OnClickMainMenu()
        {
            GameManager.Instance.GoToMainMenu();
        }
        
        public void OnCloseSettings()
        {
            _panelSettings.gameObject.SetActive(false);
            EventBus<GameEvents.ResumeGameEvent>.Raise(new GameEvents.ResumeGameEvent());
        }

        private void OnClickViewDeck()
        {
            _panelViewDeck.gameObject.SetActive(true);
            _panelViewDeck.DisplayCards(_gm.CurrentCards);
            EventBus<GameEvents.PauseGameEvent>.Raise(new GameEvents.PauseGameEvent());
        }
        
        public void OnCloseViewDeck()
        {
            _panelViewDeck.gameObject.SetActive(false);
            EventBus<GameEvents.ResumeGameEvent>.Raise(new GameEvents.ResumeGameEvent());
        }

        #endregion
    }
}