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
        private static readonly Dictionary<InputType, Dictionary<InputMode, Action<InputData>>> actions = new();
        private static readonly Dictionary<Type, InputData> inputs = new();
        public static float vertical;
        public static float horizontal;

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
            actions.Add(InputType.Key, keyActions);
            actions.Add(InputType.Axis, axisActions);
            actions.Add(InputType.Mouse, mouseActions);
            actions.Add(InputType.Button, buttonActions);
        }

        private static void OnUpdate()
        {
            foreach (var type in inputs.Keys)
            {
                if (!inputs.TryGetValue(type, out var input))
                {
                    continue;
                }

                if (!actions.TryGetValue(input.type, out var action))
                {
                    continue;
                }

                if (action.TryGetValue(input.mode, out var method))
                {
                    method.Invoke(input);
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

            input.mode = mode;
            input.keyCode = key;
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
            actions.Clear();
        }
    }

    public static partial class InputManager
    {
        private static void GetAxisX(InputData input)
        {
            horizontal = Input.GetAxis(input.button);
        }

        private static void GetAxisY(InputData input)
        {
            vertical = Input.GetAxis(input.button);
        }

        private static void GetAxisRawX(InputData input)
        {
            horizontal = Input.GetAxisRaw(input.button);
        }

        private static void GetAxisRawY(InputData input)
        {
            vertical = Input.GetAxisRaw(input.button);
        }

        private static void GetKeyDown(InputData input)
        {
            if (Input.GetKeyDown(input.keyCode))
            {
                input.Invoke();
            }
        }

        private static void GetKeyUp(InputData input)
        {
            if (Input.GetKeyUp(input.keyCode))
            {
                input.Invoke();
            }
        }

        private static void GetKey(InputData input)
        {
            if (Input.GetKey(input.keyCode))
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