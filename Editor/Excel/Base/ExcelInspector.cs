using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using Logger = JFramework.Basic.Logger;

namespace JFramework.Excel
{
	[CustomEditor(typeof(ExcelSetting))]
	internal class ExcelInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			ExcelStyle.Enable();

			var prevGUIState = GUI.enabled;
			GUI.enabled = false;
			base.OnInspectorGUI();
			GUI.enabled = prevGUIState;

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
		}
		
		[OnOpenAsset(10)]
		private static bool OnOpenExcelFile(int instanceId, int line)
		{
			try
			{
				var asset = (ExcelSetting)EditorUtility.InstanceIDToObject(instanceId);
				if (asset == null) return false;
				ExcelEditor.OpenSettingsWindow();
				return true;
			}
			catch (Exception e)
			{
				Logger.LogError(e.ToString());
			}

			return false;
		}
	}
}