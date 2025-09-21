using System;
using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    /// Minimal type-safe event bus for solo projects.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _map = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            if (_map.TryGetValue(typeof(T), out var d))
                _map[typeof(T)] = Delegate.Combine(d, handler);
            else
                _map[typeof(T)] = handler;
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            if (_map.TryGetValue(typeof(T), out var d))
            {
                var nd = Delegate.Remove(d, handler);
                if (nd == null) _map.Remove(typeof(T));
                else _map[typeof(T)] = nd;
            }
        }

        public static void Publish<T>(T evt)
        {
            if (_map.TryGetValue(typeof(T), out var d) && d is Action<T> a)
                a.Invoke(evt);
        }
    }
}
