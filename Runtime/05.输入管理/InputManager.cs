// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-06-07  05:06
// # Copyright: 2024, Charlotte
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
        private static readonly Dictionary<InputType, Dictionary<InputMode, Action<InputData>>> inputActions = new();

        internal static void Register()
        {
            GlobalManager.OnUpdate += OnUpdate;
            var buttonActions = new Dictionary<InputMode, Action<InputData>>
            {
                { InputMode.Up, GetButtonUp },
                { InputMode.Press, GetButton },
                { InputMode.Down, GetButtonDown },
            };
            var keyActions = new Dictionary<InputMode, Action<InputData>>
            {
                { InputMode.Up, GetKeyUp },
                { InputMode.Press, GetKey },
                { InputMode.Down, GetKeyDown },
            };
            var mouseActions = new Dictionary<InputMode, Action<InputData>>
            {
                { InputMode.Up, GetMouseUp },
                { InputMode.Press, GetMouse },
                { InputMode.Down, GetMouseDown },
            };
            var axisActions = new Dictionary<InputMode, Action<InputData>>
            {
                { InputMode.AxisX, GetAxisX },
                { InputMode.AxisY, GetAxisY },
                { InputMode.AxisRawX, GetAxisRawX },
                { InputMode.AxisRawY, GetAxisRawY },
            };
            inputActions.Add(InputType.Key, keyActions);
            inputActions.Add(InputType.Axis, axisActions);
            inputActions.Add(InputType.Mouse, mouseActions);
            inputActions.Add(InputType.Button, buttonActions);
        }

        private static void OnUpdate()
        {
            foreach (var type in inputs.Keys)
            {
                if (!inputs.TryGetValue(type, out var input))
                {
                    continue;
                }

                if (inputActions.TryGetValue(input.type, out var action))
                {
                    if (action.TryGetValue(input.mode, out var method))
                    {
                        method.Invoke(input);
                    }
                }
            }
        }

        public static void Add<T>(KeyCode key, InputMode mode) where T : struct, IEvent
        {
            if (!inputs.TryGetValue(typeof(T), out var input))
            {
                input = new InputEvent<T>();
                inputs.Add(typeof(T), input);
                input.Listen();
            }

            input.key = key;
            input.mode = mode;
            input.type = InputType.Key;
        }

        public static void Add<T>(string button, InputMode mode) where T : struct, IEvent
        {
            if (!inputs.TryGetValue(typeof(T), out var input))
            {
                input = new InputEvent<T>();
                inputs.Add(typeof(T), input);
                input.Listen();
            }

            input.mode = mode;
            input.button = button;
            input.type = mode > InputMode.Down ? InputType.Axis : InputType.Button;
        }

        public static void Add<T>(int mouse, InputMode mode) where T : struct, IEvent
        {
            if (!inputs.TryGetValue(typeof(T), out var input))
            {
                input = new InputEvent<T>();
                inputs.Add(typeof(T), input);
                input.Listen();
            }

            input.mode = mode;
            input.mouse = mouse;
            input.type = InputType.Mouse;
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
            vertical = 0;
            horizontal = 0;
            inputs.Clear();
            inputActions.Clear();
        }
    }
}