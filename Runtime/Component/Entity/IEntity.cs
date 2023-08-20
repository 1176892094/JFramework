using System.Reflection;
using JFramework.Core;
using UnityEngine;
using UnityEngine.UI;

namespace JFramework.Interface
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 对象变换
        /// </summary>
        Transform transform { get; }

        /// <summary>
        /// 游戏对象
        /// </summary>
        GameObject gameObject { get; }
    }

    /// <summary>
    /// 更新接口
    /// </summary>
    public interface IUpdate : IEntity
    {
        /// <summary>
        /// 实体更新
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 侦听实体的更新事件
        /// </summary>
        void Listen() => GlobalManager.Listen(this);

        /// <summary>
        /// 移除实体的更新
        /// </summary>
        void Remove() => GlobalManager.Remove(this);
    }

    /// <summary>
    /// 角色接口
    /// </summary>
    public interface ICharacter : IEntity
    {
    }

    /// <summary>
    /// 注入接口 (根据子物体名称 查找 并注入值)
    /// </summary>
    public interface IInject : IEntity
    {
        /// <summary>
        /// 获取子物体并注入字段
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        void Inject<T>(T obj) where T : Component
        {
            var type = obj.GetType();
            var fields = type.GetFields(Reflection.Instance);

            foreach (var field in fields)
            {
                var inject = field.GetCustomAttribute<InjectAttribute>(true);
                if (inject == null) continue;

                var target = GetChild(obj.transform, inject.find);
                if (target == null) continue;

                var component = target.GetComponent(field.FieldType);
                if (component != null)
                {
                    field.SetValue(obj, component);
                }

                if (!obj.TryGetComponent(out IPanel panel)) continue;
                var method = type.GetMethod(inject.find, Reflection.Instance);
                if (method == null) continue;
                if (target.TryGetComponent(out Button button) && component == button)
                {
                    button.onClick.AddListener(() =>
                    {
                        if (panel.state == UIState.Freeze) return;
                        obj.SendMessage(inject.find);
                    });
                }
                else if (target.TryGetComponent(out Toggle toggle) && component == toggle)
                {
                    toggle.onValueChanged.AddListener(value =>
                    {
                        if (panel.state == UIState.Freeze) return;
                        obj.SendMessage(inject.find, value);
                    });
                }
            }
        }

        /// <summary>
        /// 迭代查找子物体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Transform GetChild(Transform parent, string name)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.name == name)
                {
                    return child;
                }

                var result = GetChild(child, name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}