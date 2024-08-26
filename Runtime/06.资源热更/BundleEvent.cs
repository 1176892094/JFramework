// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-26  16:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using JFramework.Interface;

namespace JFramework
{
    public struct OnBundleEntry : IEvent
    {
        public readonly List<long> sizes;

        public OnBundleEntry(List<long> sizes)
        {
            this.sizes = sizes;
        }
    }

    public struct OnBundleUpdate : IEvent
    {
        public readonly string name;
        public readonly float progress;

        public OnBundleUpdate(string name, float progress)
        {
            this.name = name;
            this.progress = progress;
        }
    }

    public struct OnBundleComplete : IEvent
    {
        public readonly bool success;

        public OnBundleComplete(bool success)
        {
            this.success = success;
        }
    }
}