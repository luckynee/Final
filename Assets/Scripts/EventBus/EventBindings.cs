using System;

namespace EventBus
{
    internal interface IEvenetBindings<T>
    {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }

    public class EventBindings<T> : IEvenetBindings<T> where T : IEvent
    {
        private Action<T> onEvent = _ => { };
        private Action onEventNoArgs = () => { };
        
        Action<T> IEvenetBindings<T>.OnEvent
        {
            get => onEvent;
            set => onEvent = value;
        }
        
        Action IEvenetBindings<T>.OnEventNoArgs
        {
            get => onEventNoArgs;
            set => onEventNoArgs = value;
        }
        
        public EventBindings(Action<T> onEvent) => this.onEvent = onEvent;
        public EventBindings(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;
        
        public void Add(Action onEvent) => onEventNoArgs += onEvent;
        public void Remove(Action onEvent) => onEventNoArgs -= onEvent;
        
        public void Add(Action<T> onEvent) => this.onEvent += onEvent;
        public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;
    }
}