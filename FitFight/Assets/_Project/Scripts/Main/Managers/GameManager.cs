using System.Collections.Generic;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Game;
using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.InputSystem;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using _Project.Scripts.Main.Utilities.Scripts.Singleton;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Map;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using static _Project.Scripts.Main.Game.Events.OverlayUIEvents;

namespace _Project.Scripts.Main.Managers
{
    
    public class GameManager : PersistentSingletonMonoBehaviour<GameManager>
    {
        [SerializeField,Required] private PlayerConfig _playerConfig;
        
        [Header("Current Game Stats")]
        [ShowInInspector, ReadOnly] public string CharacterName { get; private set; }
        [ShowInInspector, ReadOnly] public int CurrentPlayerHealth { get; private set; }
        [ShowInInspector, ReadOnly] public int CurrentMaxHealth { get; private set; }
        [ShowInInspector] public int Gold { get; private set; }
        [ShowInInspector, ReadOnly] public List<CardData> CurrentCards { get; private set; }
        [ShowInInspector, ReadOnly] public HashSet<RelicData> CurrentRelics { get; private set; } = new();
        
        [Header("Level Data")]
        [ShowInInspector] public NodeType SelectedMapNodeType { get; set; }
        [SerializeField] private LevelDataList _minorLevels;
        [SerializeField] private LevelDataList _eliteLevels;
        [SerializeField] private LevelDataList _bossLevels;
        
        public Map.Map CurrentMap { get; set; }
        public NodeState CurrentNodeState { get; set; } = new();
        private LevelData? _currentLevelData;

        [Header("Special Status")] 
        [SerializeField] public bool isMembershipCard;
        
        [Header("Input Config")]
        [SerializeField] private InputConfig _inputConfig;
        public bool IsRingconAndGamepadEnabled => _inputConfig.isRingconAndGamepadEnabled;

        protected override void Awake()
        {
            base.Awake();
            ApplyPlayerConfig();
        }
        
        private void ApplyPlayerConfig()
        {
            CharacterName = _playerConfig.characterName;
            CurrentMaxHealth = _playerConfig.initialHealth;
            CurrentPlayerHealth = _playerConfig.initialHealth;
            Gold = _playerConfig.initialGold;
            CurrentCards = new List<CardData>(_playerConfig.initialCards.list);
            if(_playerConfig.starterRelic)
                AddRelic(_playerConfig.starterRelic);
        }
        
        public void SetLevelDataByType(NodeType nodeType)
        {
            SelectedMapNodeType = nodeType;
            switch (nodeType)
            {
                case NodeType.MinorEnemy:
                {
                    int count = _minorLevels.dataList.Length;
                    int rng = Random.Range(0, count);
                    _currentLevelData = _minorLevels.dataList[rng];
                    break;
                }
                case NodeType.EliteEnemy:
                {
                    int count = _eliteLevels.dataList.Length;
                    int rng = Random.Range(0, count);
                    _currentLevelData = _eliteLevels.dataList[rng];
                    break;
                }
                case NodeType.Boss:
                {
                    int count = _bossLevels.dataList.Length;
                    int rng = Random.Range(0, count);
                    _currentLevelData = _bossLevels.dataList[rng];
                    break;
                }
                default:
                    Debug.LogError("Invalid node type");
                    break;
            }
        }

        public LevelData GetCurrentLevelData()
        {
            if (_currentLevelData == null)
            {
                Debug.LogWarning("Current level data is not assigned, will assign default level");
                _currentLevelData = _minorLevels.dataList[0];
            }
            return _currentLevelData.Value;
        }
        
        public void AddCard(CardData card)
        {
            Debug.Log($"Added Card: {card.info.name}");
            CurrentCards.Add(card);
            EventBus<UpdateNumberOfCardsEvent>.Raise(new UpdateNumberOfCardsEvent());
        }

        public void RemoveCard(CardData card)
        {
            CurrentCards.Remove(card);
            EventBus<UpdateNumberOfCardsEvent>.Raise(new UpdateNumberOfCardsEvent());
        }

        public void AddRelic(RelicData relic)
        {
            if (CurrentRelics.Add(relic))
            {
                TryActivateRelic(relic);
                EventBus<InvalidateRelicBarEvent>.Raise(new InvalidateRelicBarEvent());
            }
        }
        
        private void TryActivateRelic(RelicData relic)
        {
            foreach (var command in relic.assets.commands)
            {
                if (command is AcquisitionBaseCommand)
                {
                    command.ExecuteImmediate();
                    Debug.Log($"Activate Relic: {relic.info.name}");
                    Debug.Log($"Command {command.GetType().Name} executed");
                }
            }
        }
        
        public void AddGold(int amount)
        {
            Gold += amount;
            EventBus<UpdateGoldEvent>.Raise(new UpdateGoldEvent());
        }

        public void DecreaseGold(int amount)
        {
            Gold -= amount;
            EventBus<UpdateGoldEvent>.Raise(new UpdateGoldEvent());
        }
        
        public void SetCurrentPlayerHealth(int amount)
        {
            CurrentPlayerHealth = Mathf.Clamp(amount,1,CurrentMaxHealth);
            SendHealthUIEvent();
        }
        
        public void AddHealth(int amount)
        {
            CurrentPlayerHealth = Mathf.Clamp(CurrentPlayerHealth + amount, 1, CurrentMaxHealth);
            SendHealthUIEvent();
        }
        
        public void AddMaxHealth(int amount)
        {
            CurrentMaxHealth += amount;
            SendHealthUIEvent();
        }

        private void SendHealthUIEvent()
        {
            EventBus<UpdateHealthEvent>.Raise(new UpdateHealthEvent
            {
                CurrentHealth = CurrentPlayerHealth,
                MaxHealth = CurrentMaxHealth
            });
        }

        public void ActivateMembershipCard() => isMembershipCard = true;
        public int DiscountedPrice(int price) => isMembershipCard ? price/2 : price;
        
        public async UniTask LoadInputConfig()
        {
            if(_inputConfig.isRingconAndGamepadEnabled)
                await SceneManager.LoadSceneAsync("RingconInputs",LoadSceneMode.Additive);
            else
                await SceneManager.LoadSceneAsync("DefaultInputs",LoadSceneMode.Additive);
        }

        public void GoToMainMenu()
        {
            RelicPoolManager.Instance.Destroy();
            Destroy();
            SceneManager.LoadScene("MainMenu");
        }
        
        private void OnDestroy()
        {
            CurrentMap = null;
            CurrentNodeState = null;
            CurrentCards = null;
            CurrentRelics = null;
        }

        //Note: should be called when the game is over to ensure cleanup
        public void Destroy() => Destroy(gameObject);
    }
}