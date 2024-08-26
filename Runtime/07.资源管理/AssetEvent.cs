// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-26  17:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using JFramework.Interface;

namespace JFramework
{
    public struct OnAssetEntry : IEvent
    {
        public readonly string[] names;

        public OnAssetEntry(string[] names)
        {
            this.names = names;
        }
    }

    public struct OnAssetUpdate : IEvent
    {
        public readonly string name;

        public OnAssetUpdate(string name)
        {
            this.name = name;
        }
    }

    public struct OnAssetComplete : IEvent
    {
    }
}