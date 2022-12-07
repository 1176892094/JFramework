using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework.Basic
{
    public class LocalValue<T> : ILocalValue<T>
    {
        private Action<T> OnValueChanged;

        private T value;

        public T Value
        {
            get => value;
            set
            {
                if (value == null && this.value == null) return;
                if (value != null && value.Equals(this.value)) return;
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        
        public LocalValue(T value = default)
        {
            this.value = value;
        }

        public IEvent Listen(Action<T> onValueChanged)
        {
            OnValueChanged += onValueChanged;
            return new LocalValueTrigger<T>()
            {
                LocalValue = this,
                OnValueChanged = onValueChanged
            };
        }

        public IEvent Register(Action<T> onValueChanged)
        {
            onValueChanged(value);
            return Listen(onValueChanged);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public void Remove(Action<T> onValueChanged)
        {
            OnValueChanged -= onValueChanged;
        }
        
        public static implicit operator T(LocalValue<T> property)
        {
            return property.Value;
        }
    }
    
    public interface ILocalValue<T>
    {
        T Value { get; set; }
        IEvent Register(Action<T> action);
        void Remove(Action<T> onValueChanged);
        IEvent Listen(Action<T> onValueChanged);
    }

    public class LocalValueTrigger<T> : IEvent
    {
        public LocalValue<T> LocalValue { get; set; }

        public Action<T> OnValueChanged { get; set; }

        public void Dispose()
        {
            LocalValue.Remove(OnValueChanged);
            LocalValue = null;
            OnValueChanged = null;
        }
    }
    
    public class EventObject : MonoBehaviour
    {
        private readonly HashSet<IEvent> eventList = new HashSet<IEvent>();
        public void Register(IEvent e) => eventList.Add(e);

        private void OnDestroy()
        {
            foreach (var e in eventList)
            {
                e.Dispose();
            }

            eventList.Clear();
        }
    }

    public interface IEvent
    {
        void Dispose();
    }
}