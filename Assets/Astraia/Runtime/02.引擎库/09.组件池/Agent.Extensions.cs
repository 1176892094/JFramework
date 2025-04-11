// // *********************************************************************************
// // # Project: Astraia
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 22:04:56
// // # Recently: 2025-04-09 22:04:56
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System;
using Astraia.Common;
using UnityEngine;

namespace Astraia
{
    public static partial class Extensions
    {
        public static void Show<T>(this Component current, Type type)
        {
            AgentManager.Register<T>(current, type);
        }

        public static T Find<T>(this Component current) where T : IAgent
        {
            return (T)AgentManager.Find<T>(current);
        }

        public static void Hide<T>(this Component current)
        {
            AgentManager.UnRegister<T>(current);
        }
    }
}