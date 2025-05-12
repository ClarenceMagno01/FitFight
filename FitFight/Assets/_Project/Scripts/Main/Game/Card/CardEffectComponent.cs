using System;
using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using SpriteGlow;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Project.Scripts.Main.Game.Card
{
    [RequireComponent(typeof(SpriteGlowEffect))]
    public class CardEffectComponent : MonoBehaviour
    {
        [Header("Glow Effect colors")]
        [SerializeField] private Color _highlightColor;
        [SerializeField] private Color _selectedColor;
        
        private SpriteGlowEffect _glowEffect;
        private bool _isSelected = false;
        private bool _isEnable = false;

        private GameObject _parent;
        private SortingGroup _sortingGroup;
        private Vector3 _prevPosition;
        private Vector3 _destPosition;
        private Quaternion _prevRotation;

        private bool _isCachePos;
        private bool _isCacheSortOrder;
        private int _prevSortOrder;
        private const int FrontSortOrder = 999;
        public bool IsHovered { get; private set; }
        
        private void Awake()
        {
            _parent = transform.parent.gameObject;
            _sortingGroup = _parent.GetComponent<SortingGroup>();
            _glowEffect = GetComponent<SpriteGlowEffect>();
        }

        private void Start()
        {
            EventBus<CardEffectEvent>.Register(EnableEffect);
        }

        private void OnDestroy()
        {
            EventBus<CardEffectEvent>.Deregister(EnableEffect);
        }

        private void OnEnable()
        {
            //EventBus<CardEffectEvent>.Register(EnableEffect);
            Reset();
        }
        
        private void OnDisable()
        {
            //EventBus<CardEffectEvent>.Deregister(EnableEffect);
            _isEnable = false;
        }

        private void AddGlow()
        {
            _glowEffect.GlowBrightness = 2.25f;
            _glowEffect.OutlineWidth = 1;
        }
        
        private void RemoveGlow()
        {
            _glowEffect.GlowBrightness = 1;
            _glowEffect.OutlineWidth = 0;
        }

        public void Hovered()
        {
            if(!_isEnable)
                return;

            if (!_isSelected)
            {
                AddGlow();
                _glowEffect.GlowColor = _highlightColor;
            }
            
            ScaleUp();
            RotateStraight();
            MovePositionUp();
            MoveFront();
            IsHovered = true;
        }
        
        public void UnHovered()
        {
            if(!_isEnable)
                return;
            
            if(!_isSelected)
                 RemoveGlow();
            
            ScaleNormal();
            RotateBack();
            MovePositionDown();
            MoveBack();
            IsHovered = false;
        }
        

        public void Select()
        {
            if(!_isEnable)
                return;
            
            AddGlow();
            _isSelected = true;
            _glowEffect.GlowColor = _selectedColor;
        }

        public void UnSelect()
        {
            if(!_isEnable)
                return;
            
            _isSelected = false;
            _glowEffect.GlowColor = _highlightColor;
        }

        private void Reset()
        {
            RemoveGlow();
            ScaleNormal();
            MoveBack();
            _prevRotation = Quaternion.identity;
            _isCachePos = false;
            _isCacheSortOrder = false;
            IsHovered = false;
            _isSelected = false;
        }

        public void ResetToArrange()
        {
            if(!_isSelected)
                RemoveGlow();
            
            ScaleNormal();
            //MoveBack();
            _isCachePos = false;
            _isCacheSortOrder = false;
            IsHovered = false;
        }

        #region Extra Effects

        private void ScaleUp() =>  _parent.transform.localScale =  Vector2.one * 1.2f;
        private void ScaleNormal() =>  _parent.transform.localScale =  Vector2.one * 1f;
        
        private void RotateStraight()
        {
            if (_parent.transform.rotation != Quaternion.identity)
            {
                _prevRotation = _parent.transform.rotation;
                _parent.transform.rotation = Quaternion.identity;
            }
        }

        private void RotateBack()
        {
            _parent.transform.rotation = _prevRotation;
        }
        
        private void MovePositionUp()
        {
            if (!_isCachePos)
            {
                Vector2 localPos = _parent.transform.localPosition;
                _prevPosition =  localPos;
                _destPosition =  new Vector2(localPos.x, 0.55f);
                _isCachePos = true;
            }
        
            if (_parent.transform.localPosition != _destPosition)
                _parent.transform.localPosition = _destPosition;
        }
        
        private void MovePositionDown() => _parent.transform.localPosition = _prevPosition;

        private void MoveFront()
        {
            if (!_isCacheSortOrder)
            {
                _prevSortOrder = _sortingGroup.sortingOrder;
                _isCacheSortOrder = true;
            }
            
            _sortingGroup.sortingOrder = FrontSortOrder;
        }

        private void MoveBack() => _sortingGroup.sortingOrder = _prevSortOrder;

        #endregion
        
        private void EnableEffect(CardEffectEvent ev)
        {
            _isEnable = ev.IsEnable;
        }
        
    }
}