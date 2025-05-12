using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Main.Utilities.Scripts.EventBus
{
    /// <summary>
    /// Generic EventBus implementation that handles events of type T
    /// </summary>
    public static class EventBus<T> where T : IEvent
    {
        private static readonly List<Action<T>> _listeners = new List<Action<T>>();
        private static readonly List<Action<T>> _listenersToAdd = new List<Action<T>>();
        private static readonly List<Action<T>> _listenersToRemove = new List<Action<T>>();
    
        private static bool _isDispatchingEvent;
    
        /// <summary>
        /// Subscribe to events of type T
        /// </summary>
        public static void Register(Action<T> listener)
        {
            if (_isDispatchingEvent)
            {
                _listenersToAdd.Add(listener);
            }
            else if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        /// <summary>
        /// Unsubscribe from events of type T
        /// </summary>
        public static void Deregister(Action<T> listener)
        {
            if (_isDispatchingEvent)
            {
                _listenersToRemove.Add(listener);
            }
            else
            {
                _listeners.Remove(listener);
            }
        }

        /// <summary>
        /// Publish an event of type T
        /// </summary>
        public static void Raise(T eventData)
        {
            _isDispatchingEvent = true;

            for (int i = 0; i < _listeners.Count; i++)
            {
                try
                {
                    _listeners[i]?.Invoke(eventData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error publishing event {typeof(T).Name}: {e}");
                }
            }

            _isDispatchingEvent = false;
            ProcessPendingSubscriptionChanges();
        }

        // Process any pending subscriber changes
        private static void ProcessPendingSubscriptionChanges()
        {
            for (int i = 0; i < _listenersToRemove.Count; i++)
            {
                _listeners.Remove(_listenersToRemove[i]);
            }
            _listenersToRemove.Clear();

            for (int i = 0; i < _listenersToAdd.Count; i++)
            {
                if (!_listeners.Contains(_listenersToAdd[i]))
                {
                    _listeners.Add(_listenersToAdd[i]);
                }
            }
            _listenersToAdd.Clear();
        }

        /// <summary>
        /// Clear all subscriptions for this event type
        /// </summary>
        internal static void Clear()
        {
            _listeners.Clear();
            _listenersToAdd.Clear();
            _listenersToRemove.Clear();
            _isDispatchingEvent = false;
        }
    }
}



