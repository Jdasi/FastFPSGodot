using EventData;
using System;
using System.Collections.Generic;

public static class Events
{
    private interface IEventHandler { }

    private class EventHandler<T> : IEventHandler where T : IEventData
    {
        private Action<T> _callbacks;

        public bool HasSubscribers() => _callbacks != null;
        public void Subscribe(Action<T> callback) => _callbacks += callback;
        public void Unsubscribe(Action<T> callback) => _callbacks -= callback;
        public void Invoke(T data) => _callbacks?.Invoke(data);
    }

    private static readonly Dictionary<Type, IEventHandler> _handlers = [];

    public static void Subscribe<T>(Action<T> callback) where T : IEventData
    {
        var type = typeof(T);

        if (!_handlers.TryGetValue(type, out var rawHandler))
        {
            rawHandler = new EventHandler<T>();
            _handlers[type] = rawHandler;
        }

        ((EventHandler<T>)rawHandler).Subscribe(callback);
    }

    public static void Unsubscribe<T>(Action<T> callback) where T : IEventData
    {
        var type = typeof(T);

        if (!_handlers.TryGetValue(type, out var rawHandler))
        {
            return;
        }

        var handler = (EventHandler<T>)rawHandler;
        handler.Unsubscribe(callback);

        if (!handler.HasSubscribers())
        {
            _handlers.Remove(type);
        }
    }

    public static void Invoke<T>(T data) where T : IEventData
    {
        if (_handlers.TryGetValue(typeof(T), out var rawHandler))
        {
            ((EventHandler<T>)rawHandler).Invoke(data);
        }
    }
}
