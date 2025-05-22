using System;
using _Project.Scripts.Main.Game.Card;
using _Project.Scripts.Main.Game.Commands;
using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Game.PlayerTurn;
using _Project.Scripts.Main.Game.States;
using _Project.Scripts.Main.InputSystem;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.UI;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using Map;
using MoreMountains.Feedbacks;
using UnityEngine;
using static _Project.Scripts.Main.Game.Events.CardSystemEvents;
using static _Project.Scripts.Main.Game.Events.GameEvents;
using System.Collections.Generic;
using _Project.Scripts.Main.Data;

namespace _Project.Scripts.Main.Game
{
    [RequireComponent(typeof(GameStateController))]
    public class GameController : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;

        [Header("Feedbacks")] 
        [SerializeField] private MMFeedbacks _winFeedback;
        [SerializeField] private MMFeedbacks _loseFeedback;
        
        private GameUI _gameUI;
        private GameStateController _gameStateController;
        private GameManager _gm;
        private EntityManager _em;
        [HideInInspector] public bool isCombartStart;
        public bool IsPlayerDead => _em.PlayerEntity.IsDead;
        public static bool IsGameStarted;

        private void Awake()
        {
            _gameUI = GameObject.FindWithTag("GameUI").GetComponent<GameUI>();
            _gameStateController = GetComponent<GameStateController>();
        }

        private void Start()
        {
            IsGameStarted = true;
            _gm = GameManager.Instance;
            _em = EntityManager.Instance;
        }

        private void OnEnable()
        {
            EventBus<CardPhaseFinishedEvent>.Register(OnCardPhaseFinishedEvent);
            EventBus<PlayerLoseEvent>.Register(OnGameLose);
            EventBus<PauseGameEvent>.Register(PauseGame);
            EventBus<ResumeGameEvent>.Register(ResumeGame);
            
           _gameUI.btnStartExercise.onClick.AddListener(OnClickStartExercise);
           _gameUI.btnEndTurnNoSelected.onClick.AddListener(OnClickEndPlayerTurn);
           _gameUI.btnEndTurnForExercise.onClick.AddListener(OnClickEndPlayerTurn);
           _inputReader.PressXboxEvent += OnClickEndPlayerTurn;
        }

        private void OnDisable()
        {
            EventBus<CardPhaseFinishedEvent>.Deregister(OnCardPhaseFinishedEvent);
            EventBus<PlayerLoseEvent>.Deregister(OnGameLose);
            EventBus<PauseGameEvent>.Deregister(PauseGame);
            EventBus<ResumeGameEvent>.Deregister(ResumeGame);
            
            _gameUI.btnStartExercise.onClick.RemoveListener(OnClickStartExercise);
            _gameUI.btnEndTurnNoSelected.onClick.RemoveListener(OnClickEndPlayerTurn);
            _gameUI.btnEndTurnForExercise.onClick.RemoveListener(OnClickEndPlayerTurn);
            _inputReader.PressXboxEvent -= OnClickEndPlayerTurn;
        }

        internal void SpawnEntities()
        {
            _em.SpawnPlayer();
            _em.SpawnEnemies();
        }
        
        internal void PrepareRelicCommands()
        {
            foreach (var relic in _gm.CurrentRelics)
            {
                foreach (var command in relic.assets.commands)
                {
                    if (command is LateCommand lateCommand)
                    {
                        if (lateCommand.isPersistent)
                            CommandTriggerManager.Instance.AddPersistentCommand(lateCommand);
                        else
                            CommandTriggerManager.Instance.AddOneShotCommand(lateCommand);
                    }
                }
            }
        }

        private void Update()
        {
            bool isWon = isCombartStart && _em.Enemies.Count <= 0;
            if (isWon)
            {
                OnGameWon();
            }
            
            // //TODO: testing
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     _gameStateController.ChangeState<GameWonState>();
            //    //_gameStateController.ChangeState<GameLoseState>();
            // }
        }
        
        internal void DecreasePlayerDebuff()
        {
            _em.PlayerEntity?.debuff.Decrease();
        }

        internal void StartNewCardSet(Action onNewCardSet)
        {
           EventBus<StartNewCardSetEvent>.Raise(new StartNewCardSetEvent
           {
               OnStartNewCardSet = onNewCardSet
           });
        }
        
        internal void ShowEndTurnNoSelectedButton() => _gameUI.ShowStartExerciseButton(false);
        
        internal void TryResetPlayerBlock()
        {
            _em.PlayerEntity?.TryResetBlock();
        }

        internal void SaveCurrentHealth()
        {
            int currentHealth = _em.PlayerEntity.Health;
            _gm.SetCurrentPlayerHealth(currentHealth);
        }
        
        internal void PlayWinFeedback() =>  _winFeedback.PlayFeedbacks();
        internal void PlayLoseFeedback() =>  _loseFeedback.PlayFeedbacks();

        #region Pause/Resume
        private void PauseGame(PauseGameEvent ev = default)
        {
            EventBus<StopInteractionEvent>.Raise(new StopInteractionEvent{IsStop = true});
            Time.timeScale = 0;
        }
        
        private void ResumeGame(ResumeGameEvent ev = default)
        {
            EventBus<StopInteractionEvent>.Raise(new StopInteractionEvent{IsStop = false});
            Time.timeScale = 1;
        }
        
        #endregion
        
        #region Enemy Actions

        internal void ResetEnemiesBlock()
        {
            foreach (var enemy in _em.Enemies)
                enemy.ResetBlock();
        }
        
        internal void DecreaseEnemiesDebuff()
        {
            foreach (var enemy in _em.Enemies)
                enemy.debuff.Decrease();
        }

        internal async UniTask DoEnemyAttacks()
        {
            await UniTask.Delay(500);
            foreach (var enemy in _em.Enemies)
            {
                if (_em.PlayerEntity && !_em.PlayerEntity.IsDead)
                {
                    await enemy.AttackWithFeedback();
                    await UniTask.Delay(1000);
                }
                else
                    break;
            }
        }
        
        #endregion

        #region Lose/Win Panels

        public void ShowLosePanel()
        {
            EventBus<OverlayUIEvents.ShowLosePanelEvent>.Raise(new OverlayUIEvents.ShowLosePanelEvent
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
        
        public void ShowWinPanel()
        {
            if (_gm.SelectedMapNodeType == NodeType.Boss)
            {
                _gameUI.congratsPanel.gameObject.SetActive(true);
                _gameUI.congratsPanel.SetGameSummary(new GameSummary
                {
                    TotalCards = _gm.CurrentCards.Count,
                    TotalRelics = _gm.CurrentRelics.Count,
                    Gold = _gm.Gold
                });
            }
            else
            {
                _gameUI.winPanel.gameObject.SetActive(true);
                _gameUI.winPanel.SetRewards(_gm.GetCurrentLevelData().rewards);
            }

        }
        
        #endregion
        
        #region Event Listeners
        
        private void OnCardPhaseFinishedEvent(CardPhaseFinishedEvent ev)
        {
            _gameUI.HideExerciseWithEndTurnButton();
            _gameUI.ShowEndTurnForExerciseButton(false);
            _gameStateController.ChangeState<GameEnemyTurnState>();
        }
        
        private void OnGameLose(PlayerLoseEvent ev)
        {
            isCombartStart = false;

            var exerciseCounts = new Dictionary<ExerciseType, int>(_activatedExerciseCounts); // assuming your dictionary

            _gameStateController.ChangeState<GameLoseState>();

            EventBus<OverlayUIEvents.ShowLosePanelEvent>.Raise(new OverlayUIEvents.ShowLosePanelEvent
            {
                IsShow = true,
                Summary = new GameSummary
                {
                    TotalCards = _gm.CurrentCards.Count,
                    TotalRelics = _gm.CurrentRelics.Count,
                    Gold = _gm.Gold,
                    TimeElapsedFormatted = TimerManager.Instance?.GetFormattedTime() ?? "0:00",
                }
            });
        }

        private void OnGameWon()
        {
            isCombartStart = false;
            _gameStateController.ChangeState<GameWonState>();
        }
        
        #endregion
        
        #region Button Action
        
        private void OnClickStartExercise()
        {
            if (_gm.IsRingconAndGamepadEnabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            _gameUI.HideExerciseWithEndTurnButton();
            EventBus<StartExerciseEvent>.Raise(new StartExerciseEvent
            {
                OnStartExercise = () =>
                {
                    Debug.Log("CardSystemEvents.OnStartExercise");
                    _gameUI.ShowEndTurnForExerciseButton(true);
                }
            });
        }
        
        private void OnClickEndPlayerTurn()
        {
            if(GameCard.IsCardActivating || CardSystemController.IsDrawingCards)
                return;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EventBus<EndCardPhaseEvent>.Raise(new EndCardPhaseEvent());
        }
        
        #endregion
        
        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            IsGameStarted = false;
            Time.timeScale = 1;
        }

        private Dictionary<ExerciseType, int> _activatedExerciseCounts = new Dictionary<ExerciseType, int>();

        public void RegisterActivatedExerciseType(ExerciseType type)
        {
            if (_activatedExerciseCounts.ContainsKey(type))
                _activatedExerciseCounts[type]++;
            else
                _activatedExerciseCounts[type] = 1;

            Debug.Log($"ExerciseType {type} activated count: {_activatedExerciseCounts[type]}");
        }

        public Dictionary<ExerciseType, int> GetActivatedExerciseCounts()
        {
            return new Dictionary<ExerciseType, int>(_activatedExerciseCounts);
        }

        public void ResetActivatedExerciseCounts()
        {
            _activatedExerciseCounts.Clear();
        }
        
        
    }
}