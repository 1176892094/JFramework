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

namespace JFramework
{
    public static partial class Extensions
    {
        public static Timer Wait(this IEntity entity, float waitTime)
        {
            return TimerManager.Pop(entity.gameObject, waitTime);
        }

        // public static Tween DOScaleX(this Transform transform, float endValue, float duration)
        // {
        //     var localScale = transform.localScale;
        //     return TweenManager.Tween(transform.gameObject, duration).Invoke(timer =>
        //     {
        //         var localScaleX = Mathf.Lerp(localScale.x, endValue, timer / duration);
        //         transform.localScale = new Vector3(localScaleX, localScale.y, localScale.z);
        //     });
        // }
        //
        // public static Tween DOScaleY(this Transform transform, float endValue, float duration)
        // {
        //     var localScale = transform.localScale;
        //     return TweenManager.Tween(transform.gameObject, duration).Invoke(timer =>
        //     {
        //         var localScaleY = Mathf.Lerp(localScale.y, endValue, timer / duration);
        //         transform.localScale = new Vector3(localScale.x, localScaleY, localScale.z);
        //     });
        // }
        //
        // public static Tween DOScaleZ(this Transform transform, float endValue, float duration)
        // {
        //     var localScale = transform.localScale;
        //     return TweenManager.Tween(transform.gameObject, duration).Invoke(timer =>
        //     {
        //         var localScaleZ = Mathf.Lerp(localScale.y, endValue, timer / duration);
        //         transform.localScale = new Vector3(localScale.x, localScale.y, localScaleZ);
        //     });
        // }

        public static Tween DOScale(this Transform transform, Vector3 endValue, float duration)
        {
            var localScale = transform.localScale;
            return TweenManager.Tween(transform.gameObject, duration).Invoke(timer =>
            {
                var localScaleX = Mathf.Lerp(localScale.x, endValue.x, timer / duration);
                var localScaleY = Mathf.Lerp(localScale.y, endValue.y, timer / duration);
                var localScaleZ = Mathf.Lerp(localScale.y, endValue.z, timer / duration);
                transform.localScale = new Vector3(localScaleX, localScaleY, localScaleZ);
            });
        }
        
        public static Tween DOMove(this Transform transform, Vector3 endValue, float duration)
        {
            var position = transform.position;
            return TweenManager.Tween(transform.gameObject, duration).Invoke(timer =>
            {
                var positionX = Mathf.Lerp(position.x, endValue.x, timer / duration);
                var positionY = Mathf.Lerp(position.y, endValue.y, timer / duration);
                var positionZ = Mathf.Lerp(position.y, endValue.z, timer / duration);
                transform.position = new Vector3(positionX, positionY, positionZ);
            });
        }
    }
}