// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-06-07  05:06
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;

namespace JFramework.Core
{
    public static partial class InputManager
    {
        private static readonly Dictionary<Type, InputData> inputs = new();
        private static readonly Dictionary<InputMode, Dictionary<InputState, Action<InputData>>> inputActions = new();
        public static float Vertical;
        public static float Horizontal;

        internal static void Register()
        {
            GlobalManager.OnUpdate += OnUpdate;
            var buttonActions = new Dictionary<InputState, Action<InputData>>
            {
                { InputState.Up, GetButtonUp },
                { InputState.Press, GetButton },
                { InputState.Down, GetButtonDown },
            };
            var keyActions = new Dictionary<InputState, Action<InputData>>
            {
                { InputState.Up, GetKeyUp },
                { InputState.Press, GetKey },
                { InputState.Down, GetKeyDown },
            };
            var mouseActions = new Dictionary<InputState, Action<InputData>>
            {
                { InputState.Up, GetMouseUp },
                { InputState.Press, GetMouse },
                { InputState.Down, GetMouseDown },
            };
            var axisActions = new Dictionary<InputState, Action<InputData>>
            {
                { InputState.AxisX, GetAxisX },
                { InputState.AxisY, GetAxisY },
                { InputState.AxisRawX, GetAxisRawX },
                { InputState.AxisRawY, GetAxisRawY },
            };
            inputActions.Add(InputMode.Key, keyActions);
            inputActions.Add(InputMode.Axis, axisActions);
            inputActions.Add(InputMode.Mouse, mouseActions);
            inputActions.Add(InputMode.Button, buttonActions);
        }

        private static void OnUpdate()
        {
            foreach (var type in inputs.Keys)
            {
                if (!inputs.TryGetValue(type, out var input))
                {
                    continue;
                }

                if (inputActions.TryGetValue(input.mode, out var action))
                {
                    if (action.TryGetValue(input.state, out var method))
                    {
                        method.Invoke(input);
                    }
                }
            }
        }

        public static void Add<T>(KeyCode key, InputState state) where T : struct, IEvent
        {
            if (!inputs.TryGetValue(typeof(T), out var input))
            {
                input = new InputData<T>();
                inputs.Add(typeof(T), input);
                input.Listen();
            }

            input.key = key;
            input.state = state;
            input.mode = InputMode.Key;
        }

        public static void Add<T>(string button, InputState state) where T : struct, IEvent
        {
            if (!inputs.TryGetValue(typeof(T), out var input))
            {
                input = new InputData<T>();
                inputs.Add(typeof(T), input);
                input.Listen();
            }

            input.state = state;
            input.button = button;
            input.mode = state > InputState.Down ? InputMode.Axis : InputMode.Button;
        }

        public static void Add<T>(int mouse, InputState state) where T : struct, IEvent
        {
            if (!inputs.TryGetValue(typeof(T), out var input))
            {
                input = new InputData<T>();
                inputs.Add(typeof(T), input);
                input.Listen();
            }

            input.state = state;
            input.mouse = mouse;
            input.mode = InputMode.Mouse;
        }

        public static void Remove<T>() where T : struct, IEvent
        {
            if (inputs.TryGetValue(typeof(T), out var input))
            {
                input.Remove();
                inputs.Remove(typeof(T));
            }
        }

        internal static void UnRegister()
        {
            inputs.Clear();
            inputActions.Clear();
            Vertical = 0;
            Horizontal = 0;
        }

        internal abstract class InputData
        {
            public int mouse;
            public string button;
            public KeyCode key;
            public InputMode mode;
            public InputState state;
            public abstract void Listen();
            public abstract void Remove();
            public abstract void Invoke();
        }

        [Serializable]
        private class InputData<T> : InputData where T : struct, IEvent
        {
            private event Action<T> OnInput;
            public override void Listen() => OnInput += EventManager.Invoke;
            public override void Remove() => OnInput -= EventManager.Invoke;
            public override void Invoke() => OnInput?.Invoke(default);
        }

        internal enum InputMode
        {
            Key,
            Axis,
            Mouse,
            Button
        }
    }

    public static partial class InputManager
    {
        private static void GetAxisX(InputData input)
        {
            Horizontal = Input.GetAxis(input.button);
        }

        private static void GetAxisY(InputData input)
        {
            Vertical = Input.GetAxis(input.button);
        }

        private static void GetAxisRawX(InputData input)
        {
            Horizontal = Input.GetAxisRaw(input.button);
        }

        private static void GetAxisRawY(InputData input)
        {
            Vertical = Input.GetAxisRaw(input.button);
        }

        private static void GetKeyDown(InputData input)
        {
            if (Input.GetKeyDown(input.key))
            {
                input.Invoke();
            }
        }

        private static void GetKeyUp(InputData input)
        {
            if (Input.GetKeyUp(input.key))
            {
                input.Invoke();
            }
        }

        private static void GetKey(InputData input)
        {
            if (Input.GetKey(input.key))
            {
                input.Invoke();
            }
        }

        private static void GetMouseDown(InputData input)
        {
            if (Input.GetMouseButtonDown(input.mouse))
            {
                input.Invoke();
            }
        }

        private static void GetMouseUp(InputData input)
        {
            if (Input.GetMouseButtonUp(input.mouse))
            {
                input.Invoke();
            }
        }

        private static void GetMouse(InputData input)
        {
            if (Input.GetMouseButton(input.mouse))
            {
                input.Invoke();
            }
        }

        private static void GetButtonDown(InputData input)
        {
            if (Input.GetButtonDown(input.button))
            {
                input.Invoke();
            }
        }

        private static void GetButtonUp(InputData input)
        {
            if (Input.GetButtonUp(input.button))
            {
                input.Invoke();
            }
        }

        private static void GetButton(InputData input)
        {
            if (Input.GetButton(input.button))
            {
                input.Invoke();
            }
        }
    }
}