using System;
using _Project.Scripts.Main.VirtualMouse;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.Main.Utilities.Scripts
{

    public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IOnMouseEnterHandler,IOnMouseExitHandler
    {
        [SerializeField] protected bool ignoreInteractable = false;
        [SerializeField] protected UnityEvent onHoverOverAction;
        [SerializeField] protected UnityEvent onHoverExitAction;
        [SerializeField] protected bool executeHoverEnterActionPerFrame = true;

        private Selectable _selectable;
        private bool _isHovering;
    
        private void OnEnable()
        {
            _selectable = this.GetComponent<Selectable>();
        }

        private void OnDisable()
        {
            _isHovering = false;
            onHoverExitAction?.Invoke();
        }

        private void OnDestroy()
        {
            _isHovering = false;
        }
    
        void OnMouseEnter()
        {
            StartHover();
        }

        private void OnMouseExit()
        {
            StopHover();
        }
    
        public void OnMouseEnterVirtual()
        {
            StartHover();
        }

        public void OnMouseExitVirtual()
        {
            StopHover();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!ignoreInteractable && _selectable != null)
            {
                if (!_selectable.IsInteractable())
                {
                    return;
                }
            }

            StartHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!ignoreInteractable && _selectable != null)
            {
                if (!_selectable.IsInteractable())
                {
                    return;
                }
            }

            StopHover();
        }
    
        private void StartHover()
        {
            if (executeHoverEnterActionPerFrame)
            {
                _isHovering = true;
            }
            else
            {
                _isHovering = true;
                onHoverOverAction?.Invoke();
            }
        }
    
        private void StopHover()
        {
            _isHovering = false;
            onHoverExitAction?.Invoke();
        }
    
        public void SetOnHoverOverAction(UnityAction e)
        {
            onHoverOverAction.RemoveAllListeners();
            onHoverOverAction.AddListener(e);
        }

        public void SetOnHoverOutAction(UnityAction e)
        {
            onHoverExitAction.RemoveAllListeners();
            onHoverExitAction.AddListener(e);
        }

        public void AddOnHoverOverAction(UnityAction e)
        {
            onHoverOverAction.AddListener(e);
        }

        public void AddOnHoverOutAction(UnityAction e)
        {
            onHoverExitAction.AddListener(e);
        }

        public void RemoveOnHoverOverAction(UnityAction e)
        {
            onHoverOverAction.RemoveListener(e);
        }

        public void RemoveOnHoverOutAction(UnityAction e)
        {
            onHoverExitAction.RemoveListener(e);
        }

        public void ClearHoverActions()
        {
            onHoverOverAction.RemoveAllListeners();
            onHoverExitAction.RemoveAllListeners();
        }

        public void ExecuteHoverEnterActionPerFrame(bool p_state)
        {
            executeHoverEnterActionPerFrame = p_state;
        }


        void Update()
        {
            if (executeHoverEnterActionPerFrame)
            {
                if (_isHovering)
                {
                    onHoverOverAction?.Invoke();
                }
            }
        }


    }
}