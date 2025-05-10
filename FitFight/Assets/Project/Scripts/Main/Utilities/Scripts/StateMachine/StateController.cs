using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Main.Utilities.Scripts.StateMachine
{
    public interface IState
    {
        void OnEnter(object args);
        void OnExec();
        void OnExit();
        void OnPause();
        void OnResume();
    }

    public abstract class StateController : MonoBehaviour {

        // Use this for initialization
        internal virtual void Start() {

        }

        // Update is called once per frame
        internal virtual void Update() {
            if (null != _mObj)
            {
                _mObj.OnExec();
            }
        }

        public Type State
        {
            get { return _sState; }
        }

        public IState StateHandler
        {
            get { return _mObj; }
        }

        public void RegisterStateHandler<T>()
        {
            _aryHandlers[typeof(T)] = (IState)Activator.CreateInstance(typeof(T), new object[] { this });
        }

        public void ReleaseStateHandler<T>()
        {
            _aryHandlers[typeof(T)] = null;
        }

        public void ChangeState<T>(object args = null)
        {
            Type sNext = typeof(T);

            if (null != _mObj)
            {
                _mObj.OnExit();
            }

            _mObj = _aryHandlers[sNext];
            _sState = sNext;

            if (null != _mObj)
            {
                _mObj.OnEnter(args);
            }
        }

        internal void OnEnable()
        {
            if (null != _mObj) _mObj.OnResume();
        }

        internal void OnDisable()
        {
            if (null != _mObj) _mObj.OnPause();
        }

        public void Pause()
        {
            Debug.Log("Paused");
            if (null != _mObj) _mObj.OnPause();
            enabled = false;
        }

        public void Resume()
        {
            Debug.Log("Resumed");
            enabled = true;
            if (null != _mObj) _mObj.OnResume();
        }

        private Dictionary<Type, IState> _aryHandlers = new Dictionary<Type, IState>();
        private Type _sState;
        private IState _mObj;
    }
}