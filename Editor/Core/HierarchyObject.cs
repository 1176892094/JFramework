using UnityEditor;
using UnityEngine;

namespace JFramework.Editor
{
    internal class HierarchyObject
    {
        public int ID = int.MinValue;
        public Rect rect = Rect.zero;
        public Rect nameRect = Rect.zero;
        public GameObject gameObject;

        public void Dispose()
        {
            ID = int.MinValue;
            rect = Rect.zero;
            nameRect = Rect.zero;
            gameObject = null;
        }

        public void Enable()
        {
            Vector2 localPos = Event.current.mousePosition;
            bool isHover = localPos.x >= 0 && localPos.x <= rect.xMax + 16 && localPos.y >= rect.y && localPos.y < rect.yMax;
            if (!isHover) return;
            Rect localRect = new Rect(32.5f, rect.y, 16,rect.height);
            bool isEnable = EditorGUI.Toggle(localRect, GUIContent.none, gameObject.activeSelf);
            gameObject.SetActive(isEnable);
        }
    }
}