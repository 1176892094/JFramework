using UnityEngine;
using UnityEditor;

namespace JFramework.Excel
{
	[CustomEditor(typeof(ExcelSetting))]
	internal class ExcelInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			ExcelStyle.Enable();
			
			GUI.enabled = false;
			base.OnInspectorGUI();
			GUI.enabled = true;

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Open",  GUILayout.Height(20)))
			{
				ExcelEditor.OpenSettingsWindow();
			}
			if (GUILayout.Button("Reset", GUILayout.Height(20)))
			{
				if (EditorUtility.DisplayDialog("JFramework", "Are you sure to reset ExcelSetting?", "Yes", "Cancel"))
				{
					ExcelSetting.Instance.ResetData();
					EditorUtility.SetDirty(ExcelSetting.Instance);
				}
			}
			GUILayout.EndHorizontal();
			
			GUIUtility.ExitGUI();
		}
	}
}