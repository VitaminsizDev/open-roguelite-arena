using System;
using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    /// Lightweight service locator. Use sparingly.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Register<T>(T instance) where T : class
        {
            _services[typeof(T)] = instance;
        }

        public static T Resolve<T>() where T : class
        {
            return _services.TryGetValue(typeof(T), out var o) ? (T)o : null;
        }

        public static void Reset()
        {
            _services.Clear();
        }
    }
}
