// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-08-25  03:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    internal abstract class InputData
    {
        public int mouse;
        public string button;
        public KeyCode key;
        public InputMode mode;
        public InputOption option;
        public abstract void Listen();
        public abstract void Remove();
        public abstract void Invoke();
    }
}