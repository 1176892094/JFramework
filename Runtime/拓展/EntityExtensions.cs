using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    public static partial class Extensions
    {
        /// <summary>
        /// 继承ICharacter后可以使用 GetOrAddCtrl
        /// </summary>
        /// <param name="character"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrAddCtrl<T>(this ICharacter character) where T : ScriptableObject, IController
        {
            return ControllerManager.GetOrAddCtrl<T>(character.Id);
        }

        /// <summary>
        /// 继承ICharacter后可以使用 Dispose
        /// </summary>
        /// <param name="character"></param>
        public static void Destroy(this ICharacter character)
        {
            ControllerManager.Destroy(character.Id);
        }
    }
}