using System;
using System.Collections.Generic;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Game.Commands;
using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Game.Entity;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video; // <-- Required for VideoPlayer
using static _Project.Scripts.Main.Game.Events.GameCardEvents;

namespace _Project.Scripts.Main.Game.Card
{
    public class GameCard : MonoBehaviour, ICard
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _txtReps;
        [SerializeField] private TMP_Text _txtName;
        [SerializeField] private TMP_Text _txtDescription;
        [SerializeField] private VideoPlayer _videoPlayer; // <-- Added VideoPlayer reference

        [Header("Activate Feedback")]
        [SerializeField] private MMF_Player _activateFeedback;
        
        [Space]
        [Header("Data")]
        public int drawID;
        
        public int targetIndex = -1;
        private CardEffectComponent _effect;
        private SortingGroup _sortingGroup;
        [ShowInInspector] public CardData Data { get; set; }
        [ShowInInspector] public CardType Type => Data?.info.type ?? CardType.Attack;
        [ShowInInspector] public bool CanPickTarget => Data?.canPickTarget ?? false;
        [ShowInInspector] public bool IsSelected { get; private set; }

        public static bool IsCardActivating = false;

        private void Awake()
        {
            _sortingGroup = GetComponent<SortingGroup>();
            _effect = GetComponentInChildren<CardEffectComponent>();
        }

        private void Start()
        {
            IsCardActivating = false;
            EventBus<EnableColliderEvent>.Register(EnableCollider);
        }

        private void OnDisable()
        {
            Cleanup();
        }

        private void OnDestroy()
        {
            EventBus<EnableColliderEvent>.Deregister(EnableCollider);
        }

        public void InvalidateUI()
        {
            if (Data == null)
                throw new NullReferenceException("Card data is null");

            _txtReps.text = Data.info.requiredReps.ToString();
            _txtName.text = Data.info.name;
            _txtDescription.text = Data.info.description;

            // Handle video playback
            if (_videoPlayer != null)
            {
                if (Data.videoClip != null)
                {
                    _videoPlayer.clip = Data.videoClip;
                    _videoPlayer.Play();
                }
                else
                {
                    _videoPlayer.Stop();
                    _videoPlayer.clip = null;
                }
            }
        }

        public void Activate(Action onActivated = null)
        {
            IsCardActivating = true;
            AsyncActivate(onActivated).Forget();
        }

        private async UniTaskVoid AsyncActivate(Action onActivated)
        {
            Player player = EntityManager.Instance.PlayerEntity;
            List<Enemy> targets = EntityManager.Instance.Enemies;

            await PlayActivateFeedback();
            foreach (var command in Data.assets.commands)
            {
                if (command is LateCommand lateCommand)
                {
                    if (lateCommand.isPersistent)
                        CommandTriggerManager.Instance.AddPersistentCommand(lateCommand);
                    else
                        CommandTriggerManager.Instance.AddOneShotCommand(lateCommand);
                }
                else
                {
                    command.SetPlayer(player);
                    if (command is EnemyTargetBaseCommand targetCommand)
                    {
                        targetCommand.SetPriority(targetIndex);
                        targetCommand.SetTargets(targets);
                    }
                    if (command is CardBaseCommand cardCommand)
                        cardCommand.SetCardData(Data);

                    await command.ExecuteImmediate();
                    Debug.Log($"Command {command.GetType().Name} executed");
                }
            }

            Debug.Log($"Card: {Data?.info.name} Activated");
            onActivated?.Invoke();
            IsCardActivating = false;
        }

        private async UniTask PlayActivateFeedback()
        {
            _activateFeedback.PlayFeedbacks();
            while (_activateFeedback.IsPlaying)
                await UniTask.Yield();
        }

        public void SetSortingOrder(int order)
        {
            _sortingGroup.sortingOrder = order;
        }

        public void Select()
        {
            IsSelected = true;
            _effect.Select();
        }

        public void UnSelect()
        {
            targetIndex = -1;
            IsSelected = false;
            _effect.UnSelect();
        }

        public void UnHovered() => _effect.UnHovered();
        public bool IsHovered => _effect.IsHovered;
        public void ResetToArrange() => _effect.ResetToArrange();

        private void EnableCollider(EnableColliderEvent ev)
        {
            GetComponent<BoxCollider2D>().enabled = ev.IsEnable;
        }

        private void Cleanup()
        {
            _sortingGroup.sortingOrder = 0;
            targetIndex = -1;
            IsSelected = false;
            Data = null;

            // Optionally stop video when card is reset
            if (_videoPlayer != null)
            {
                _videoPlayer.Stop();
                _videoPlayer.clip = null;
            }
        }
    }
}
