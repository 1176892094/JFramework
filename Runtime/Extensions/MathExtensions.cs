// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-06-07  20:06
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework
{
    public static partial class Extensions
    {
        public static readonly Collider2D[] results = new Collider2D[16];

        public static void Rotation(this Transform transform, Transform target)
        {
            transform.Rotation(target.position);
        }

        public static void Rotation(this Transform transform, Vector3 target)
        {
            transform.localScale = new Vector3(transform.position.x > target.x ? -1 : 1, 1, 1);
        }

        public static void FindTarget<T>(this Transform transform, Vector2 direction, float distance, int layer, Action<T> action) where T : Component
        {
            transform.position.FindTarget(direction, distance, layer, action);
        }

        public static void FindTarget<T>(this Transform transform, float radius, int layer, Action<T> action) where T : Component
        {
            transform.position.FindTarget(radius, layer, action);
        }

        public static void FindTarget<T>(this Transform transform, Vector2 size, int layer, Action<T> action) where T : Component
        {
            transform.position.FindTarget(size, layer, action);
        }

        public static void FindTargets<T>(this Transform transform, float radius, ContactFilter2D filter, Action<T> action) where T : Component
        {
            transform.position.FindTargets(radius, filter, action);
        }

        public static void FindTargets<T>(this Transform transform, Vector2 size, float angle, ContactFilter2D filter,  Action<T> action) where T : Component
        {
            transform.position.FindTargets(size, angle, filter, action);
        }

        public static void FindTargets<T>(this Transform transform, float radius, ContactFilter2D filter, Func<T, bool> func) where T : Component
        {
            transform.position.FindTargets(radius, filter, func);
        }

        public static void FindTargets<T>(this Transform transform, Vector2 size, float angle, ContactFilter2D filter, Func<T, bool> func) where T : Component
        {
            transform.position.FindTargets(size, angle, filter, func);
        }

        public static void FindTarget<T>(this Vector3 position, Vector2 direction, float distance, int layer, Action<T> action) where T : Component
        {
            var collider = Physics2D.Raycast(position, direction, distance, layer).collider;
            if (collider && collider.TryGetComponent(out T component))
            {
                action.Invoke(component);
            }
        }
        public static void FindTarget<T>(this Vector3 position, float radius, int layer, Action<T> action) where T : Component
        {
            var collider = Physics2D.OverlapCircle(position, radius, layer);
            if (collider && collider.TryGetComponent(out T component))
            {
                action.Invoke(component);
            }
        }
        
        public static void FindTarget<T>(this Vector3 position, Vector2 size, int layer, Action<T> action) where T : Component
        {
            var collider = Physics2D.OverlapBox(position, size, layer);
            if (collider && collider.TryGetComponent(out T component))
            {
                action.Invoke(component);
            }
        }

        public static void FindTargets<T>(this Vector3 position, float radius, ContactFilter2D filter, Action<T> action) where T : Component
        {
            var result = Physics2D.OverlapCircle(position, radius, filter, results);
            for (int i = 0; i < result; i++)
            {
                var collider = results[i];
                if (collider && collider.TryGetComponent(out T component))
                {
                    action.Invoke(component);
                }
            }
        }

        public static void FindTargets<T>(this Vector3 position, Vector2 size, float angle, ContactFilter2D filter, Action<T> action) where T : Component
        {
            var result = Physics2D.OverlapBox(position, size, angle, filter, results);
            for (int i = 0; i < result; i++)
            {
                var collider = results[i];
                if (collider && collider.TryGetComponent(out T component))
                {
                    action.Invoke(component);
                }
            }
        }

        public static void FindTargets<T>(this Vector3 position, float radius, ContactFilter2D filter, Func<T, bool> func) where T : Component
        {
            var result = Physics2D.OverlapCircle(position, radius, filter, results);
            for (int i = 0; i < result; i++)
            {
                var collider = results[i];
                if (collider && collider.TryGetComponent(out T component))
                {
                    if (func.Invoke(component))
                    {
                        break;
                    }
                }
            }
        }

        public static void FindTargets<T>(this Vector3 position, Vector2 size, float angle, ContactFilter2D filter, Func<T, bool> func) where T : Component
        {
            var result = Physics2D.OverlapBox(position, size, angle, filter, results);
            for (int i = 0; i < result; i++)
            {
                var collider = results[i];
                if (collider && collider.TryGetComponent(out T component))
                {
                    if (func.Invoke(component))
                    {
                        break;
                    }
                }
            }
        }
    }
}