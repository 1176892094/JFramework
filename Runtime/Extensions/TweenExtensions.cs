// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-07  00:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using JFramework.Core;
using JFramework.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace JFramework
{
    public static partial class Extensions
    {
        public static T Search<T>(this IEntity entity) where T : ScriptableObject, IComponent
        {
            return (T)EntityManager.GetComponent(entity, typeof(T));
        }

        public static void Destroy(this IEntity entity)
        {
            EntityManager.Destroy(entity);
        }

        public static Timer Wait(this IEntity entity, float waitTime)
        {
            return TimerManager.Pop(entity.gameObject, waitTime);
        }

        public static Tween DOScaleX(this Transform transform, float endValue, float duration)
        {
            var localScale = transform.localScale;
            return TweenManager.Tween(transform.gameObject, duration).Invoke(progress =>
            {
                var localScaleX = Mathf.Lerp(localScale.x, endValue, progress);
                transform.localScale = new Vector3(localScaleX, localScale.y, localScale.z);
            });
        }

        public static Tween DOScaleY(this Transform transform, float endValue, float duration)
        {
            var localScale = transform.localScale;
            return TweenManager.Tween(transform.gameObject, duration).Invoke(progress =>
            {
                var localScaleY = Mathf.Lerp(localScale.y, endValue, progress);
                transform.localScale = new Vector3(localScale.x, localScaleY, localScale.z);
            });
        }

        public static Tween DOScaleZ(this Transform transform, float endValue, float duration)
        {
            var localScale = transform.localScale;
            return TweenManager.Tween(transform.gameObject, duration).Invoke(progress =>
            {
                var localScaleZ = Mathf.Lerp(localScale.y, endValue, progress);
                transform.localScale = new Vector3(localScale.x, localScale.y, localScaleZ);
            });
        }

        public static Tween DOScale(this Transform transform, Vector3 endValue, float duration)
        {
            var localScale = transform.localScale;
            return TweenManager.Tween(transform.gameObject, duration).Invoke(progress =>
            {
                var localScaleX = Mathf.Lerp(localScale.x, endValue.x, progress);
                var localScaleY = Mathf.Lerp(localScale.y, endValue.y, progress);
                var localScaleZ = Mathf.Lerp(localScale.y, endValue.z, progress);
                transform.localScale = new Vector3(localScaleX, localScaleY, localScaleZ);
            });
        }

        public static Tween DOMoveX(this Transform transform, float endValue, float duration)
        {
            var position = transform.position;
            return TweenManager.Tween(transform.gameObject, duration).Invoke(progress =>
            {
                var positionX = Mathf.Lerp(position.x, endValue, progress);
                transform.position = new Vector3(positionX, position.y, position.z);
            });
        }

        public static Tween DOMoveY(this Transform transform, float endValue, float duration)
        {
            var position = transform.position;
            return TweenManager.Tween(transform.gameObject, duration).Invoke(progress =>
            {
                var positionY = Mathf.Lerp(position.y, endValue, progress);
                transform.position = new Vector3(position.x, positionY, position.z);
            });
        }

        public static Tween DOMoveZ(this Transform transform, float endValue, float duration)
        {
            var position = transform.position;
            return TweenManager.Tween(transform.gameObject, duration).Invoke(progress =>
            {
                var positionZ = Mathf.Lerp(position.y, endValue, progress);
                transform.position = new Vector3(position.x, position.y, positionZ);
            });
        }

        public static Tween DOMove(this Transform transform, Vector3 endValue, float duration)
        {
            var position = transform.position;
            return TweenManager.Tween(transform.gameObject, duration).Invoke(progress =>
            {
                var positionX = Mathf.Lerp(position.x, endValue.x, progress);
                var positionY = Mathf.Lerp(position.y, endValue.y, progress);
                var positionZ = Mathf.Lerp(position.y, endValue.z, progress);
                transform.position = new Vector3(positionX, positionY, positionZ);
            });
        }

        public static Tween DOShake(this Camera camera, float strength, float duration)
        {
            var position = camera.transform.localPosition;
            return TweenManager.Tween(camera.gameObject, duration).Invoke(progress =>
            {
                if (progress < 1.0f)
                {
                    var offsetX = Mathf.Lerp(0, Random.Range(-strength, strength), progress);
                    var offsetY = Mathf.Lerp(0, Random.Range(-strength, strength), progress);
                    var offsetZ = Mathf.Lerp(0, Random.Range(-strength, strength), progress);
                    camera.transform.localPosition = position + new Vector3(offsetX, offsetY, offsetZ);
                }
                else
                {
                    camera.transform.localPosition = position;
                }
            });
        }

        public static Tween DOColor(this SpriteRenderer component, Color endValue, float duration)
        {
            var color = component.color;
            return TweenManager.Tween(component.gameObject, duration).Invoke(progress =>
            {
                var colorR = Mathf.Lerp(color.r, endValue.r, progress);
                var colorG = Mathf.Lerp(color.g, endValue.g, progress);
                var colorB = Mathf.Lerp(color.b, endValue.b, progress);
                var colorA = Mathf.Lerp(color.a, endValue.a, progress);
                component.color = new Color(colorR, colorG, colorB, colorA);
            });
        }

        public static Tween DOFade(this SpriteRenderer component, float endValue, float duration)
        {
            var color = component.color;
            return TweenManager.Tween(component.gameObject, duration).Invoke(progress =>
            {
                var colorA = Mathf.Lerp(color.a, endValue, progress);
                component.color = new Color(color.r, color.g, color.b, colorA);
            });
        }

        public static Tween DOColor(this Image component, Color endValue, float duration)
        {
            var color = component.color;
            return TweenManager.Tween(component.gameObject, duration).Invoke(progress =>
            {
                var colorR = Mathf.Lerp(color.r, endValue.r, progress);
                var colorG = Mathf.Lerp(color.g, endValue.g, progress);
                var colorB = Mathf.Lerp(color.b, endValue.b, progress);
                var colorA = Mathf.Lerp(color.a, endValue.a, progress);
                component.color = new Color(colorR, colorG, colorB, colorA);
            });
        }

        public static Tween DOFade(this Image component, float endValue, float duration)
        {
            var color = component.color;
            return TweenManager.Tween(component.gameObject, duration).Invoke(progress =>
            {
                var colorA = Mathf.Lerp(color.a, endValue, progress);
                component.color = new Color(color.r, color.g, color.b, colorA);
            });
        }
    }
}