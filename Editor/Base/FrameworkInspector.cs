using System.Globalization;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JFramework.Editor
{
    internal class FrameworkInspector : InspectorEditor
    {
        private const string JFramework = "JFrameworkInspector";
        private const string MenuPath = "Tools/JFramework/";

        private static FrameworkInspector instance;
        private static VisualElement visualElement;

        private FrameworkInspector()
        {
            EditorApplication.delayCall += InitInspector;
            WindowManager.OnMaximizedChanged += OnMaximizedChanged;
            Selection.selectionChanged += InitInspector;
        }

        [InitializeOnLoadMethod]
        private static void Init() => instance = new FrameworkInspector();

        protected override bool OnInject(EditorWindow wnd, VisualElement mainContainer, VisualElement editorsList)
        {
            if (editorsList.parent[0].name == JFramework) editorsList.parent.RemoveAt(0);
            if (editorsList.childCount != 0 || float.IsNaN(editorsList.layout.width)) return false;
            if (visualElement == null) visualElement = InitItems();
            editorsList.parent.Insert(0, visualElement);
            return true;
        }
        
        private VisualElement InitItems()
        {
            VisualElement element = new VisualElement { name = JFramework };

            Label box = new Label("JFramework Tools")
            {
                style =
                {
                    backgroundColor = Color.gray * 0.7f,
                    fontSize = 15,
                    height = 25,
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            };

            element.Add(box);
            InitItems(element);
            InitSettings(element);
            InitWindows(element);
            return element;
        }
        
        private void InitSettings(VisualElement parent)
        {
            CreateLabel(parent, "Settings");
            VisualElement container = CreateContainer(parent);
            CreateButton(container, "Edit/Project Settings...", "Project Settings");
#if UNITY_EDITOR_OSX
            CreateButton(container, "Unity/Preferences...", "Preferences");
#elif UNITY_EDITOR
            CreateButton(container, "Edit/Preferences...", "Preferences");
#endif
        }
        
        private static void CreateLabel(VisualElement parent, string text)
        {
            Label label = new Label(text)
            {
                style =
                {
                    fontSize = 12,
                    marginTop = 5,
                    marginLeft = 3,
                    marginRight = 3,
                    paddingLeft = 5
                }
            };
            parent.Add(label);
        }

        private static void CreateButton(VisualElement parent, string submenu, string text)
        {
            ToolbarButton button = new ToolbarButton(() => EditorApplication.ExecuteMenuItem(submenu))
            {
                text = text,
                style =
                {
                    left = 0,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    borderLeftWidth = 0,
                    borderRightWidth = 0
                }
            };
            parent.Add(button);
        }

        private VisualElement CreateContainer(VisualElement parent)
        {
            VisualElement el = new VisualElement();
            el.style.borderBottomWidth = el.style.borderTopWidth = el.style.borderLeftWidth = el.style.borderRightWidth = 1;
            el.style.borderBottomColor = el.style.borderTopColor = el.style.borderLeftColor = el.style.borderRightColor = Color.gray;
            el.style.marginLeft = 3;
            el.style.marginRight = 5;
            parent.Add(el);
            return el;
        }
        
        private void InitItems(VisualElement parent)
        {
            CreateLabel(parent, "JFramework");
            VisualElement container = CreateContainer(parent);
            CreateButton(container, MenuPath + "JFramework Init", "JFramework Init");
            CreateButton(container, MenuPath + "Excel Setting", "Excel Setting");
            CreateButton(container, MenuPath + "Excel Writer", "Excel Writer");
            CreateButton(container, MenuPath + "Excel Delete", "Excel Delete");
            CreateButton(container, MenuPath + "CurrentProjectPath", "CurrentProjectPath");
            CreateButton(container, MenuPath + "PersistentDataPath", "PersistentDataPath");
            CreateButton(container, MenuPath + "StreamingAssetsPath", "StreamingAssetsPath");
        }

        private void InitWindows(VisualElement parent)
        {
            CreateLabel(parent, "Packages");
            VisualElement container = CreateContainer(parent);

            CreateButton(container, "Assets/Import Package/Custom Package...", "Import Custom Package");

            bool skip = true;
            string groupName = "";

            foreach (string submenu in Unsupported.GetSubmenus("Window"))
            {
                TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
                string upper = textInfo.ToUpper(submenu);
                if (skip)
                {
                    if (upper == "WINDOW/PACKAGE MANAGER")
                    {
                        skip = false;
                    }
                    else continue;
                }

                string[] parts = submenu.Split('/');
                string firstPart = parts[1];

                if (parts.Length == 2)
                {
                    CreateButton(container, submenu, firstPart);
                }
                else if (parts.Length == 3)
                {
                    if (groupName != firstPart)
                    {
                        CreateLabel(parent, firstPart);
                        container = CreateContainer(parent);
                        groupName = firstPart;
                    }

                    CreateButton(container, submenu, parts[2]);
                }
            }
        }
    }
}