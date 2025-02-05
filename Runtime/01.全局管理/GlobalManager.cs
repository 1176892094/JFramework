// *********************************************************************************
// # Project: Forest
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-02-05 18:02:19
// # Recently: 2025-02-05 18:02:22
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using AssetData = System.Collections.Generic.KeyValuePair<string, string>;
using EnumTable = System.Collections.Generic.Dictionary<System.Enum, JFramework.Common.IData>;
using ItemTable = System.Collections.Generic.Dictionary<int, JFramework.Common.IData>;
using NameTable = System.Collections.Generic.Dictionary<string, JFramework.Common.IData>;
using AgentData = System.Collections.Generic.Dictionary<System.Type, UnityEngine.ScriptableObject>;

namespace JFramework.Common
{
    internal class GlobalManager : JFramework.GlobalManager
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<Type, ItemTable> m_ItemTable { get; } = new Dictionary<Type, ItemTable>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<Type, NameTable> m_NameTable { get; } = new Dictionary<Type, NameTable>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<Type, EnumTable> m_EnumTable { get; } = new Dictionary<Type, EnumTable>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<string, PackData> m_ClientPacks { get; } = new Dictionary<string, PackData>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<string, PackData> m_ServerPacks { get; } = new Dictionary<string, PackData>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<string, AssetData> m_AssetData { get; } = new Dictionary<string, AssetData>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<string, AssetBundle> m_AssetPack { get; } = new Dictionary<string, AssetBundle>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<string, Task<AssetBundle>> m_AssetTask { get; } = new Dictionary<string, Task<AssetBundle>>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<string, IPool> m_PoolData { get; } = new Dictionary<string, IPool>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<string, GameObject> m_PoolGroup { get; } = new Dictionary<string, GameObject>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<Component, AgentData> m_AgentData { get; } = new Dictionary<Component, AgentData>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<Type, UIPanel> m_PanelData { get; } = new Dictionary<Type, UIPanel>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<string, HashSet<UIPanel>> m_PanelGroup { get; } = new Dictionary<string, HashSet<UIPanel>>();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        protected override Dictionary<int, RectTransform> m_PanelLayer { get; } = new Dictionary<int, RectTransform>();
    }
}