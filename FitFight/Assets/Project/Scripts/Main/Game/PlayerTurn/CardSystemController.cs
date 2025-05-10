using System;
using System.Collections.Generic;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Game.Card;
using _Project.Scripts.Main.Game.Entity;
using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Game.Exercise;
using _Project.Scripts.Main.Game.PlayerTurn.Systems;
using _Project.Scripts.Main.InputSystem;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.UI;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using static _Project.Scripts.Main.Game.Events.CardSystemEvents;
using static _Project.Scripts.Main.Game.Events.CommandTriggerEvents;
using static _Project.Scripts.Main.Game.Events.GameCardEvents;

namespace _Project.Scripts.Main.Game.PlayerTurn
{
    public class CardSystemController : IStartable, IDisposable, ITickable
    {
        private const int DefaultDrawCount = 5;
        
        private readonly HeldCards _heldCards;
        private readonly InputReader _inputReader;
        
        private readonly TargetSystem _targetSystem;
        private CardSystem _cardSystem;
        
        private GameUI _gameUI;     
        private Camera _camera;
        private Vector2 _mousePointer;
        private int _uiLayerMask;
        private bool _isDisableCardInput;

        private GameCard _targetingCard;
        private Stack<GameCard> _selectedCards;
        private int _repCount;
        private bool _isExerciseQueued; 
        private bool _isExercisePaused;

        public static bool IsDrawingCards = false;
        
        [Inject]
        internal CardSystemController(IObjectResolver container,HeldCards heldCards,InputReader inputReader,TargetSystem targetSystem)
        {
            _heldCards = heldCards;
            _inputReader = inputReader;
            _targetSystem = targetSystem;
        }
        
        public void Start()
        {
            _inputReader.PointEvent += OnPoint;
            _inputReader.RingconLightPressEvent += OnRingconLightPressEvent;
            _inputReader.RingconLightPullEvent += OnRingconLightPullEvent;
            EventBus<StartNewCardSetEvent>.Register(OnStartNewCardSet);
            EventBus<StartExerciseEvent>.Register(OnStartExercise);
            EventBus<EndCardPhaseEvent>.Register(EndCardPhase);
            EventBus<CopyCardToDiscardPileEvent>.Register(OnCopyCardToDiscardPile);
            EventBus<StopInteractionEvent>.Register(StopInteraction);
            
            _cardSystem = new CardSystem(GameManager.Instance.CurrentCards);
            _gameUI = GameObject.FindWithTag("GameUI").GetComponent<GameUI>();
            _gameUI.SetDrawPileCount(_cardSystem.DrawPile.Count);
            _uiLayerMask = ~LayerMask.GetMask("UI");
            _camera = Camera.main;
        }
        
        public void Dispose()
        {
            Debug.Log("CardSystemController disposed");
            IsDrawingCards = false;
            _inputReader.PointEvent -= OnPoint;
            _inputReader.RingconLightPressEvent -= OnRingconLightPressEvent;
            _inputReader.RingconLightPullEvent -= OnRingconLightPullEvent;
            EventBus<StartNewCardSetEvent>.Deregister(OnStartNewCardSet);
            EventBus<StartExerciseEvent>.Deregister(OnStartExercise);
            EventBus<EndCardPhaseEvent>.Deregister(EndCardPhase);
            EventBus<CopyCardToDiscardPileEvent>.Deregister(OnCopyCardToDiscardPile);
            EventBus<StopInteractionEvent>.Deregister(StopInteraction);
        }

        private void OnStartNewCardSet(StartNewCardSetEvent ev)
        {
            StartNewCardSet(ev).Forget();
        }
        
        private async UniTaskVoid StartNewCardSet(StartNewCardSetEvent ev = default)
        {
            IsDrawingCards = true;
            _repCount = 0;
            DisableCardInput();
            //Draw cards with default count else draw first the remaining and draw another cards to make it up to default count
            if (_cardSystem.DrawPile.Count >= DefaultDrawCount)
            {
                _cardSystem.ShuffleDrawPile();
                await DrawNewCardSet(DefaultDrawCount);
            }
            else
            {
                int remaining = _cardSystem.DrawPile.Count;
                await DrawNewCardSet(remaining);
                _cardSystem.RecycleDiscardedCards();
                _cardSystem.ShuffleDrawPile();
                
                UpdatePilesUI();
                await DrawCards(DefaultDrawCount - remaining,false);
            }
            EnableCardInput();
            ev.OnStartNewCardSet?.Invoke();
            IsDrawingCards = false;
        }

        private async UniTask DrawNewCardSet(int drawCount)
        {
            int pileCount = _cardSystem.DrawPile.Count;
            _gameUI.SetDrawPileCount(pileCount);
            
            var drawnCards = _cardSystem.DrawCards(drawCount);
            await _heldCards.SpawnNewSet(drawnCards, () =>
            {
                _gameUI.SetDrawPileCount(--pileCount);
            });
        }

        private async UniTask DrawCards(int drawCount,bool autoEnableInput)
        {
            DisableCardInput();
            int pileCount = _cardSystem.DrawPile.Count;
            _gameUI.SetDrawPileCount(pileCount);
            
            var drawnCards = _cardSystem.DrawCards(drawCount);
            for (int i = 0; i < drawnCards.Count; i++)
            {
                await _heldCards.AddCard(drawnCards[i], _ =>
                {
                    _gameUI.SetDrawPileCount(--pileCount);
                });
            }
            if(autoEnableInput)
                EnableCardInput();
        }
        
        private void SelectCard(GameCard card)
        {
            if(_isDisableCardInput || !card.IsHovered)
                return;
            
            if (card.CanPickTarget && !_cardSystem.IsCardSelected(card.drawID))
            {
                DisableCardInput();
                _targetingCard = card;
                _targetSystem.StartTargeting(card.gameObject.transform.position);
            }
            else
            {
                if (_cardSystem.SelectDrawnCard(card.drawID))
                    card.Select();
            }
            TryShowButtonExercise();
        }

        private void SelectEnemy(Enemy enemy)
        {
            if (_targetingCard != null)
            {
                if (_cardSystem.SelectDrawnCard(_targetingCard.drawID))
                {
                    EnableCardInput();
                    _targetingCard.Select();
                    _targetingCard.targetIndex = enemy.spawnIndex;
                }
                StopTargeting();
            }
            TryShowButtonExercise();
        }
        
        private void StopTargeting()
        {
            _targetSystem.StopTargeting();
            _targetingCard?.UnHovered();
            _targetingCard = null;
        }
        
        private void UnselectCard(GameCard card)
        {
            if(_isDisableCardInput)
                return;

            if (_cardSystem.DeselectDrawnCard(card.drawID))
                card.UnSelect();
            TryShowButtonExercise();
        }

        private void TryShowButtonExercise()
        {
            bool isShow = _cardSystem.SelectedCards.Count > 0;
            _gameUI.ShowStartExerciseButton(isShow);
        }
        
        private void OnStartExercise(StartExerciseEvent ev)
        {
            StartExercise(ev).Forget();
        }
        
        private async UniTask StartExercise(StartExerciseEvent ev = default)
        {
            DisableCardInput();
            List<CardInstance> selectedCards = _cardSystem.SelectedCards;
            if (selectedCards.Count > 0)
            {
                _selectedCards = await _heldCards.ArrangeSelectedCards(selectedCards,0.2f);
                _selectedCards.Peek().Data.assets.exercise?.Restart();
                ev.OnStartExercise?.Invoke();
                _isExerciseQueued = true;
            }
        }
        
        private void RunCurrentExercise()
        {
            if (_isExerciseQueued && _selectedCards is {Count: > 0} && !_isExercisePaused)
            {
                GameCard card = _selectedCards.Peek();
                IExercise exercise = card.Data.assets.exercise;
                int requiredRep = card.Data.info.requiredReps;
                if (exercise == null)
                    throw new NullReferenceException("No exercise assigned to the card data");
                exercise.Run(() =>
                {
                    _repCount++;
                    if (_repCount == requiredRep)
                    {
                        _isExerciseQueued = false;
                        Debug.Log($"Exercise completed: {card.Data.info.name}");
                        exercise.Restart();
                        card.Activate(() =>
                        {
                            if(card.Data.info.type == CardType.Attack)
                                EventBus<AttackCardPlayedEvent>.Raise(new AttackCardPlayedEvent());
                            if (card.Data.isOneTimeUse)
                                _cardSystem.RemoveSelectedCardPermanently(card.drawID);
                            else
                                _cardSystem.DiscardSelectedCard(card.drawID);
                            _heldCards.RemoveCard(card);
                            _selectedCards.Pop();
                            _repCount = 0;
                            _gameUI.SetDiscardPileCount(_cardSystem.DiscardPile.Count);
                            _isExerciseQueued = true;
                        });
                    }
                });
            }
            else
            {
                // EndCardPhase(); //Probably just let the user click end turn
            }
        }
        
        private void EndCardPhase(EndCardPhaseEvent ev = default)
        {
            _isExerciseQueued = false;
            DisableCardInput();
            StopTargeting();
            _cardSystem.DiscardRemainingDrawnAndAllSelectedCards();
            _selectedCards?.Clear();
            _heldCards.ClearCards();
            UpdatePilesUI();
            EventBus<GameEvents.CardPhaseFinishedEvent>.Raise(new GameEvents.CardPhaseFinishedEvent());
        }

        private void UpdatePilesUI()
        {
            _gameUI.SetDrawPileCount(_cardSystem.DrawPile.Count);
            _gameUI.SetDiscardPileCount(_cardSystem.DiscardPile.Count);
        }

        private void OnCopyCardToDiscardPile(CopyCardToDiscardPileEvent ev)
        {
            _cardSystem.CopyCardToDiscardPile(ev.Data);
            _gameUI.SetDiscardPileCount(_cardSystem.DiscardPile.Count);
        }
        
        public void Tick()
        {
            RunCurrentExercise();
        }

        #region Card Input
        
        private void DisableCardInput()
        {
            EventBus<CardEffectEvent>.Raise(new CardEffectEvent { IsEnable = false });
            _isDisableCardInput = true;
        }

        private void EnableCardInput()
        {
            EventBus<CardEffectEvent>.Raise(new CardEffectEvent { IsEnable = true });
            _isDisableCardInput = false;
        }

        private void StopInteraction(StopInteractionEvent ev)
        {
            if (ev.IsStop)
            {
                CancelTargetingIfActive();
                EventBus<EnableColliderEvent>.Raise(new EnableColliderEvent {IsEnable = false});
                _isExercisePaused = true;
            }
            else
            {
                EventBus<EnableColliderEvent>.Raise(new EnableColliderEvent {IsEnable = true});
                _isExercisePaused = false;
            }
        }
        
        #endregion

        #region Gameplay Controller Inputs

        private void OnPoint(Vector2 input)
        {
            _mousePointer = input;
        }

        private void OnRingconLightPressEvent()
        {
            if (IsCardSelected(out GameCard gameCard))
                SelectCard(gameCard);

            if (_targetSystem.IsTargeting)
            {
                if (_targetSystem.IsTouchEntity(out IEntity entity) && entity is Enemy enemy)
                    SelectEnemy(enemy);
            }
        }
        
        private void OnRingconLightPullEvent()
        {
            if (IsCardSelected(out GameCard gameCard))
                UnselectCard(gameCard);

            CancelTargetingIfActive();
        }

        private void CancelTargetingIfActive()
        {
            if (_targetSystem.IsTargeting)
            {
                EnableCardInput();
                StopTargeting();
            }
            _targetingCard = null;
        }

        #endregion
        
        private bool IsCardSelected(out GameCard gameCard)
        {
            Vector2 worldPoint = _camera.ScreenToWorldPoint(_mousePointer);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0f, _uiLayerMask);
            if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out GameCard card))
            {
                gameCard = card;
                return true;
            }

            gameCard = null;
            return false;
        }
        
    }
}