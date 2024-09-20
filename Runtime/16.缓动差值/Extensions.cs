// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-27  00:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    public static partial class Extensions
    {
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
    }
}