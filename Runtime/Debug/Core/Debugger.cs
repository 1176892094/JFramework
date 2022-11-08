using UnityEngine;

namespace JYJFramework.Logger
{
    public abstract class Debugger
    {
        public Component Target;
        public abstract void OnInit();
        public abstract void OnDebugGUI();
    }
}
