// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  00:03
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework.Interface
{
    public interface IPanel : IEntity
    {
        /// <summary>
        /// UI层级
        /// </summary>
        UILayer layer { get; }

        /// <summary>
        /// UI面板状态
        /// </summary>
        UIState state { get; }

        /// <summary>
        /// 显示
        /// </summary>
        void Show();

        /// <summary>
        /// 隐藏
        /// </summary>
        void Hide();
    }
}