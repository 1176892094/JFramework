using System.Threading;
using UnityEngine;

namespace JYJFramework.Async
{
    public static class UnitySynchronize
    {
        public static SynchronizationContext synchronizationContext;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            synchronizationContext = SynchronizationContext.Current;
        }
    }
    
    public class WaitForUpdate : CustomYieldInstruction
    {
        public override bool keepWaiting => false;
    }
}
