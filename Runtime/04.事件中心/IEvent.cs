// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework.Interface
{
    public interface IEvent
    {
    }

    public interface IEvent<in T> : IEvent where T : struct, IEvent
    {
        void Execute(T message);
    }
}