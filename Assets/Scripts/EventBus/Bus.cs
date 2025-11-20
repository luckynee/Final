using System.Collections.Generic;

namespace EventBus
{
    public static class Bus<T> where T : IEvent
    {
        static readonly HashSet<IEvenetBindings<T>> bindings = new HashSet<IEvenetBindings<T>>();
        
        public static void Register(EventBindings<T> binding) => bindings.Add(binding);
        public static void Unregister(EventBindings<T> binding) => bindings.Remove(binding);
        public static void Raise(T @event)
        {
            foreach (var binding in bindings)
            {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }
    }
}
