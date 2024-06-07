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
    public static partial class InputSystem
    {
        private static readonly Dictionary<Type, InputData> inputs = new();
        private static readonly Dictionary<InputMode, Dictionary<InputState, Action<Type, InputData>>> inputActions = new();

        internal static void Register()
        {
            GlobalManager.OnUpdate += OnUpdate;
            var keyActions = new Dictionary<InputState, Action<Type, InputData>>
            {
                { InputState.Up, GetKeyUp },
                { InputState.Press, GetKey },
                { InputState.Down, GetKeyDown },
            };
            var mouseActions = new Dictionary<InputState, Action<Type, InputData>>
            {
                { InputState.Up, GetMouseUp },
                { InputState.Press, GetMouse },
                { InputState.Down, GetMouseDown },
            };
            var buttonActions = new Dictionary<InputState, Action<Type, InputData>>
            {
                { InputState.Up, GetButtonUp },
                { InputState.Press, GetButton },
                { InputState.Down, GetButtonDown },
            };
            inputActions.Add(InputMode.Key, keyActions);
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
                        method.Invoke(type, input);
                    }
                }
            }
        }

        public static void Add<T>(KeyCode key, InputState state)
        {
            if (!inputs.TryGetValue(typeof(T), out var input))
            {
                input = new InputData();
                inputs.Add(typeof(T), input);
            }

            input.key = key;
            input.state = state;
            input.mode = InputMode.Key;
        }

        public static void Add<T>(int mouse, InputState state)
        {
            if (!inputs.TryGetValue(typeof(T), out var input))
            {
                input = new InputData();
                inputs.Add(typeof(T), input);
            }

            input.state = state;
            input.mouse = mouse;
            input.mode = InputMode.Mouse;
        }

        public static void Add<T>(string button, InputState state)
        {
            if (!inputs.TryGetValue(typeof(T), out var input))
            {
                input = new InputData();
                inputs.Add(typeof(T), input);
            }

            input.state = state;
            input.button = button;
            input.mode = InputMode.Button;
        }

        internal static void UnRegister()
        {
            inputs.Clear();
        }

        [Serializable]
        private class InputData
        {
            public int mouse;
            public KeyCode key;
            public string button;
            public InputMode mode;
            public InputState state;
        }

        [Serializable]
        public struct InputAction : IEvent
        {
            public Type type;
            public InputAction(Type type) => this.type = type;
        }

        private enum InputMode
        {
            Key,
            Mouse,
            Button
        }
    }

    public static partial class InputSystem
    {
        private static void GetKeyDown(Type type, InputData input)
        {
            if (Input.GetKeyDown(input.key))
            {
                EventManager.Invoke(new InputAction(type));
            }
        }

        private static void GetKeyUp(Type type, InputData input)
        {
            if (Input.GetKeyUp(input.key))
            {
                EventManager.Invoke(new InputAction(type));
            }
        }

        private static void GetKey(Type type, InputData input)
        {
            if (Input.GetKey(input.key))
            {
                EventManager.Invoke(new InputAction(type));
            }
        }

        private static void GetMouseDown(Type type, InputData input)
        {
            if (Input.GetMouseButtonDown(input.mouse))
            {
                EventManager.Invoke(new InputAction(type));
            }
        }

        private static void GetMouseUp(Type type, InputData input)
        {
            if (Input.GetMouseButtonUp(input.mouse))
            {
                EventManager.Invoke(new InputAction(type));
            }
        }

        private static void GetMouse(Type type, InputData input)
        {
            if (Input.GetMouseButton(input.mouse))
            {
                EventManager.Invoke(new InputAction(type));
            }
        }

        private static void GetButtonDown(Type type, InputData input)
        {
            if (Input.GetButtonDown(input.button))
            {
                EventManager.Invoke(new InputAction(type));
            }
        }

        private static void GetButtonUp(Type type, InputData input)
        {
            if (Input.GetButtonUp(input.button))
            {
                EventManager.Invoke(new InputAction(type));
            }
        }

        private static void GetButton(Type type, InputData input)
        {
            if (Input.GetButton(input.button))
            {
                EventManager.Invoke(new InputAction(type));
            }
        }
    }
}