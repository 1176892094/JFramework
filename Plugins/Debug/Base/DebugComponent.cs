using UnityEngine;

namespace JFramework
{
    internal abstract class DebugComponent
    {
        public Component Target;
        public abstract void OnInit();
        public abstract void OnDebugGUI();
    }
}