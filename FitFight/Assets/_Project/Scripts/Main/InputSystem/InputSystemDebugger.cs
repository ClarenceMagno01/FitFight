using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Main.InputSystem
{
    public class InputSystemDebugger : MonoBehaviour
    {
        [SerializeField, Required] private InputReader _inputReader;

        private void OnEnable()
        {
            _inputReader.PointEvent += OnPoint;
            _inputReader.LookEvent += OnLook;
            _inputReader.RingconRotateEvent += OnRingconRotate;
            _inputReader.RingconLightPressEvent += OnRingconLightPressEvent;
            _inputReader.RingconLightPressUpEvent += OnRingconLightPressUpEvent;
            _inputReader.RingconLightPullEvent += OnRingconLightPullEvent;
            _inputReader.RingconHeavyPressEvent += OnRingconHeavyPressEvent;
            _inputReader.RingconHeavyPullEvent += OnRingconHeavyPullEvent;
            _inputReader.RunEvent += OnRun;
            _inputReader.SprintEvent += OnSprint;
            _inputReader.StrapconLeftEvent += OnStrapconLeft;
            _inputReader.StrapconRightEvent += OnStrapconRight;
            _inputReader.StrapconDownEvent += OnStrapconDown;
            _inputReader.StrapconUpEvent += OnStrapconUp;
            _inputReader.PressXboxEvent += OnPressXbox;
        }

        private void OnDisable()
        {
            _inputReader.PointEvent -= OnPoint;
            _inputReader.LookEvent -= OnLook;
            _inputReader.RingconRotateEvent -= OnRingconRotate;
            _inputReader.RingconLightPressEvent -= OnRingconLightPressEvent;
            _inputReader.RingconLightPressUpEvent -= OnRingconLightPressUpEvent;
            _inputReader.RingconLightPullEvent -= OnRingconLightPullEvent;
            _inputReader.RingconHeavyPressEvent -= OnRingconHeavyPressEvent;
            _inputReader.RingconHeavyPullEvent -= OnRingconHeavyPullEvent;
            _inputReader.RunEvent -= OnRun;
            _inputReader.SprintEvent -= OnSprint;
            _inputReader.StrapconLeftEvent -= OnStrapconLeft;
            _inputReader.StrapconRightEvent -= OnStrapconRight;
            _inputReader.StrapconDownEvent -= OnStrapconDown;
            _inputReader.StrapconUpEvent -= OnStrapconUp;
            _inputReader.PressXboxEvent -= OnPressXbox;
        }

        private void OnPoint(Vector2 input)
        {
            //Debug.Log($"Point: {input}");
        }

        private void OnLook(Vector2 input)
        {
            //Debug.Log($"Look: {input}");
        }

        private void OnRingconRotate(Vector2 value)
        {
//            Debug.Log($"Ringcon Rotate: {value}");
        }

        private void OnRingconLightPressEvent()
        {
            Debug.Log("Ringcon Light Press");
        }

        private void OnRingconLightPressUpEvent()
        {
            Debug.Log("Ringcon Light Press Up");
        }

        private void OnRingconLightPullEvent()
        {
            Debug.Log("Ringcon Light Pull");
        }

        private void OnRingconHeavyPressEvent()
        {
            Debug.Log("Ringcon Heavy Press");
        }

        private void OnRingconHeavyPullEvent()
        {
            Debug.Log("Ringcon Heavy Pull");
        }

        private void OnRun()
        {
            Debug.Log("Run");
        }

        private void OnSprint()
        {
            Debug.Log("Sprint");
        }

        private void OnStrapconLeft()
        {
            Debug.Log("Strapcon Left");
        }

        private void OnStrapconRight()
        {
            Debug.Log("Strapcon Right");
        }

        private void OnStrapconDown()
        {
            Debug.Log("Strapcon Down");
        }

        private void OnStrapconUp()
        {
            Debug.Log("Strapcon Up");
        }

        private void OnPressXbox()
        {
            Debug.Log("PressXbox");
        }
    }
}