// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-12 21:12:43
// # Recently: 2024-12-22 20:12:39
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    public static partial class Extensions
    {
        public static Tween DOMoveX(this IEntity entity, float endValue, float duration)
        {
            var transform = entity.GetComponent<Transform>();
            var position = transform.position;
            return entity.Tween(duration).Invoke(progress =>
            {
                var positionX = Mathf.Lerp(position.x, endValue, progress);
                transform.position = new Vector3(positionX, position.y, position.z);
            });
        }

        public static Tween DOMoveY(this IEntity entity, float endValue, float duration)
        {
            var transform = entity.GetComponent<Transform>();
            var position = transform.position;
            return entity.Tween(duration).Invoke(progress =>
            {
                var positionY = Mathf.Lerp(position.y, endValue, progress);
                transform.position = new Vector3(position.x, positionY, position.z);
            });
        }

        public static Tween DOMoveZ(this IEntity entity, float endValue, float duration)
        {
            var transform = entity.GetComponent<Transform>();
            var position = transform.position;
            return entity.Tween(duration).Invoke(progress =>
            {
                var positionZ = Mathf.Lerp(position.z, endValue, progress);
                transform.position = new Vector3(position.x, position.y, positionZ);
            });
        }

        public static Tween DOMove(this IEntity entity, Vector3 endValue, float duration)
        {
            var transform = entity.GetComponent<Transform>();
            var position = transform.position;
            return entity.Tween(duration).Invoke(progress =>
            {
                var positionX = Mathf.Lerp(position.x, endValue.x, progress);
                var positionY = Mathf.Lerp(position.y, endValue.y, progress);
                var positionZ = Mathf.Lerp(position.y, endValue.z, progress);
                transform.position = new Vector3(positionX, positionY, positionZ);
            });
        }

        public static Tween DOScaleX(this IEntity entity, float endValue, float duration)
        {
            var transform = entity.GetComponent<Transform>();
            var localScale = transform.localScale;
            return entity.Tween(duration).Invoke(progress =>
            {
                var localScaleX = Mathf.Lerp(localScale.x, endValue, progress);
                transform.localScale = new Vector3(localScaleX, localScale.y, localScale.z);
            });
        }

        public static Tween DOScaleY(this IEntity entity, float endValue, float duration)
        {
            var transform = entity.GetComponent<Transform>();
            var localScale = transform.localScale;
            return entity.Tween(duration).Invoke(progress =>
            {
                var localScaleY = Mathf.Lerp(localScale.y, endValue, progress);
                transform.localScale = new Vector3(localScale.x, localScaleY, localScale.z);
            });
        }

        public static Tween DOScaleZ(this IEntity entity, float endValue, float duration)
        {
            var transform = entity.GetComponent<Transform>();
            var localScale = transform.localScale;
            return entity.Tween(duration).Invoke(progress =>
            {
                var localScaleZ = Mathf.Lerp(localScale.y, endValue, progress);
                transform.localScale = new Vector3(localScale.x, localScale.y, localScaleZ);
            });
        }

        public static Tween DOScale(this IEntity entity, Vector3 endValue, float duration)
        {
            var transform = entity.GetComponent<Transform>();
            var localScale = transform.localScale;
            return entity.Tween(duration).Invoke(progress =>
            {
                var localScaleX = Mathf.Lerp(localScale.x, endValue.x, progress);
                var localScaleY = Mathf.Lerp(localScale.y, endValue.y, progress);
                var localScaleZ = Mathf.Lerp(localScale.y, endValue.z, progress);
                transform.localScale = new Vector3(localScaleX, localScaleY, localScaleZ);
            });
        }
    }
}