using System;

namespace GatherBuddy.Utility
{
    public static class Service<T> where T : class
    {
        private static T? _object;

        public static void Dispose()
        {
            var o = _object as IDisposable;
            o?.Dispose();
            _object = null;
        }

        public static void Set(T obj)
            => _object = obj;

        public static T Set(params dynamic[] args)
        {
            _object = (T) Activator.CreateInstance(typeof(T), args);
            return _object;
        }

        public static T Get()
        {
            if (_object == null)
                throw new InvalidOperationException($"{nameof(T)} has not been registered.");

            return _object;
        }
    }
}
