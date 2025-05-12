using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using UnityEngine;

namespace _Project.Scripts.Main.Game.PlayerTurn.Systems
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Arrow : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        
        [Header("Colors")]
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _grayColor;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            EventBus<ArrowDefaultColorEvent>.Register(DefaultColor);
            EventBus<ArrowGrayColorEvent>.Register(GrayColor);
        }

        private void OnDisable()
        {
            EventBus<ArrowDefaultColorEvent>.Deregister(DefaultColor);
            EventBus<ArrowGrayColorEvent>.Deregister(GrayColor);
            _spriteRenderer.color = _defaultColor;
        }

        private void DefaultColor(ArrowDefaultColorEvent ev) => _spriteRenderer.color = _defaultColor;
        private void GrayColor(ArrowGrayColorEvent ev) => _spriteRenderer.color = _grayColor;
    }
}