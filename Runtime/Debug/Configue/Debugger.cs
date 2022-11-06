using UnityEngine;

namespace JYJFramework
{
    public abstract class Debugger
    {
        public Component Target;
        public abstract void OnInit();
        public abstract void OnDebuggerGUI();
    }
}
