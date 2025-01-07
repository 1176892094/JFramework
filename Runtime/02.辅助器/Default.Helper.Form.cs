// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-24 03:12:30
// # Recently: 2024-12-24 03:12:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework
{
    internal sealed partial class DefaultHelper : IFormHelper
    {
        string IFormHelper.Path(string objectText, FileAccess fileAccess)
        {
            switch (fileAccess)
            {
                case FileAccess.Write when objectText == "Assembly":
                    return GlobalSetting.ScriptPath + "/HotUpdate.Data.asmdef";
                case FileAccess.Write when objectText == "Enum":
                    return GlobalSetting.ScriptPath + "/03.枚举类/{0}.cs";
                case FileAccess.Write when objectText == "Table":
                    return GlobalSetting.ScriptPath + "/01.数据表/{0}DataTable.cs";
                case FileAccess.Write when objectText == "Struct":
                    return GlobalSetting.ScriptPath + "/02.结构体/{0}.cs";
                case FileAccess.Write when objectText == "Data":
                    return GlobalSetting.DataTablePath + "/{0}DataTable.asset";
                case FileAccess.Read when objectText == "Assembly":
                    return Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[0].text;
                case FileAccess.Read when objectText == "Enum":
                    return Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[1].text;
                case FileAccess.Read when objectText == "Struct":
                    return Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[2].text;
                case FileAccess.Read when objectText == "Table":
                    return Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[3].text;
            }

            return string.Empty;
        }

        async Task<IDataTable> IFormHelper.Instantiate(string assetPath)
        {
            return (IDataTable)await Service.Asset.Load<ScriptableObject>(assetPath);
        }

        IDataTable IFormHelper.CreateInstance(string assetPath)
        {
            return (IDataTable)ScriptableObject.CreateInstance(assetPath);
        }

        void IFormHelper.CreateAsset(IDataTable assetData, string assetPath)
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset((ScriptableObject)assetData, assetPath);
#endif
        }

        void IFormHelper.CreateProgress(string assetPath, float progress)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayProgressBar(assetPath, "", progress);
#endif
        }
    }
}