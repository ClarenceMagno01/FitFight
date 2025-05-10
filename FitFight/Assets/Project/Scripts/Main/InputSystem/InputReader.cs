using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Main.InputSystem
{
    [CreateAssetMenu(fileName = "GameplayInputReader", menuName = "InputReader/GameplayInputReader")]
    public class InputReader : ScriptableObject, GameplayInput.IRingconActions
    {
        private GameplayInput _gameplayInput;

        public event UnityAction<Vector2> RingconRotateEvent;
        public event UnityAction RingconLightPressEvent;
        public event UnityAction RingconLightPressUpEvent;
        public event UnityAction RingconLightPullEvent;
        public event UnityAction RingconHeavyPressEvent;
        public event UnityAction RingconHeavyPullEvent;
        public event UnityAction RunEvent;
        public event UnityAction SprintEvent;
        public event UnityAction StrapconLeftEvent;
        public event UnityAction StrapconRightEvent;
        public event UnityAction StrapconDownEvent;
        public event UnityAction StrapconUpEvent;
        public event UnityAction<Vector2> LookEvent;
        public event UnityAction<Vector2> PointEvent;
        public event UnityAction PressXboxEvent;

        private void OnEnable()
        {
            _gameplayInput ??= new GameplayInput();
            _gameplayInput.Ringcon.SetCallbacks(this);
            _gameplayInput.Ringcon.Enable();
        }

        private void OnDisable()
        {
            if (_gameplayInput != null)
            {
                _gameplayInput.Ringcon.RemoveCallbacks(this);
                _gameplayInput.Ringcon.Disable();
            }
        }

        public void OnRingconRotate(InputAction.CallbackContext context)
        {
            if (context.performed)
                RingconRotateEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnRingconLightPress(InputAction.CallbackContext context)
        {
            if (context.performed)
                RingconLightPressEvent?.Invoke();
            else if(context.canceled)
                RingconLightPressUpEvent?.Invoke();
        }

        public void OnRingconLightPull(InputAction.CallbackContext context)
        {
            if (context.performed)
                RingconLightPullEvent?.Invoke();
        }

        public void OnRingconHeavyPress(InputAction.CallbackContext context)
        {
            if (context.performed)
                RingconHeavyPressEvent?.Invoke();
        }

        public void OnRingconHeavyPull(InputAction.CallbackContext context)
        {
            if (context.performed)
                RingconHeavyPullEvent?.Invoke();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.performed)
                RunEvent?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
                SprintEvent?.Invoke();
        }

        public void OnStrapconLeft(InputAction.CallbackContext context)
        {
            if (context.performed)
                StrapconLeftEvent?.Invoke();
        }

        public void OnStrapconRight(InputAction.CallbackContext context)
        {
            if (context.performed)
                StrapconRightEvent?.Invoke();
        }

        public void OnStrapconDown(InputAction.CallbackContext context)
        {
            if (context.performed)
                StrapconDownEvent?.Invoke();
        }

        public void OnStrapconUp(InputAction.CallbackContext context)
        {
            if (context.performed)
                StrapconUpEvent?.Invoke();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            PointEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnXboxButton(InputAction.CallbackContext context)
        {
            if (context.performed)
                PressXboxEvent?.Invoke();
        }
        
    }
}
