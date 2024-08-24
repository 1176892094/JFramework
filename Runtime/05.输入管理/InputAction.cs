// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-25  04:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework.Core
{
    public static partial class InputManager
    {
        public static float vertical;
        public static float horizontal;

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