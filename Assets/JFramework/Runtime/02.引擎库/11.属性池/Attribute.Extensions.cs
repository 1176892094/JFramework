// // *********************************************************************************
// // # Project: JFramework
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 22:04:49
// // # Recently: 2025-04-09 22:04:49
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System.Runtime.CompilerServices;
using UnityEngine;

namespace JFramework
{
    public static partial class Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetFloat<TOwner, T>(this Attribute<TOwner, T> attribute, T key) where TOwner : Component
        {
            return attribute.Get(key);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetFloat<TOwner, T>(this Attribute<TOwner, T> attribute, T key, float value) where TOwner : Component
        {
            attribute.Set(key, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddFloat<TOwner, T>(this Attribute<TOwner, T> attribute, T key, float value) where TOwner : Component
        {
            attribute.SetFloat(key, attribute.GetFloat(key) + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubFloat<TOwner, T>(this Attribute<TOwner, T> attribute, T key, float value) where TOwner : Component
        {
            attribute.SetFloat(key, attribute.GetFloat(key) - value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetInt<TOwner, T>(this Attribute<TOwner, T> attribute, T key) where TOwner : Component
        {
            return (int)attribute.Get(key);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetInt<TOwner, T>(this Attribute<TOwner, T> attribute, T key, float value) where TOwner : Component
        {
            attribute.Set(key, (int)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddInt<TOwner, T>(this Attribute<TOwner, T> attribute, T key, float value) where TOwner : Component
        {
            attribute.SetInt(key, attribute.GetInt(key) + (int)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubInt<TOwner, T>(this Attribute<TOwner, T> attribute, T key, float value) where TOwner : Component
        {
            attribute.SetInt(key, attribute.GetInt(key) - (int)value);
        }
    }
}