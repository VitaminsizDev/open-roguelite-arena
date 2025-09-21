using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    /// Simple generic object pool for non-Unity objects.
    /// </summary>
    public class Pool<T> where T : class, new()
    {
        private readonly Stack<T> _stack = new();

        public T Get()
        {
            return _stack.Count > 0 ? _stack.Pop() : new T();
        }

        public void Release(T obj)
        {
            if (obj != null) _stack.Push(obj);
        }
    }
}
