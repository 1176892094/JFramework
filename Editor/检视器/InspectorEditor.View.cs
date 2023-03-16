using System.Globalization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JFramework
{
    [InitializeOnLoad]
    internal class InspectorView : InspectorEditor
    {
        private static InspectorView instance;
        private static VisualElement element;

        static InspectorView() => instance = new InspectorView();

        private InspectorView()
        {
            Selection.selectionChanged -= Awake;
            EditorApplication.delayCall -= Awake;
            InspectorEvent.OnMaximizedChanged -= OnMaximizedChanged;
            Selection.selectionChanged += Awake;
            EditorApplication.delayCall += Awake;
            InspectorEvent.OnMaximizedChanged += OnMaximizedChanged;
        }

        protected override bool InitInspector(EditorWindow w, VisualElement element)
        {
            if (element.parent[0].name == Const.Inspector) element.parent.RemoveAt(0);
            if (element.childCount != 0 || float.IsNaN(element.layout.width)) return false;
            InspectorView.element ??= InitInspector();
            element.parent.Insert(0, InspectorView.element);
            return false;
        }

        private VisualElement InitInspector()
        {
            var visualElement = new VisualElement
            {
                name = Const.Inspector
            };

            InitTitle(visualElement);
            InitFramework(visualElement);
            InitSettings(visualElement);
            InitWindows(visualElement);
            return visualElement;
        }

        private void InitTitle(VisualElement visualElement)
        {
            Label box = new Label(Const.Inspector)
            {
                style =
                {
                    height = 25,
                    fontSize = 15,
                    borderBottomWidth = 1f,
                    borderBottomColor = Color.black,
                    backgroundColor = Color.gray * 0.6f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                }
            };
            visualElement.Add(box);
        }

        private void InitFramework(VisualElement parent)
        {
            CreateLabel(parent, "JFramework");
            VisualElement container = CreateContainer(parent);
            CreateButton(container, Const.MenuPath + Const.EditorWindow, Const.EditorWindow);
            CreateButton(container, Const.MenuPath + Const.ExcelToScripts, Const.ExcelToScripts);
            CreateButton(container, Const.MenuPath + Const.ExcelToAssets, Const.ExcelToAssets);
            CreateButton(container, "Window/Asset Management/Addressables/Groups","AddressablesView");
            CreateButton(container, Const.MenuPath + Const.CurrentProjectPath, Const.CurrentProjectPath);
            CreateButton(container, Const.MenuPath + Const.PersistentDataPath, Const.PersistentDataPath);
            CreateButton(container, Const.MenuPath + Const.StreamingAssetsPath, Const.StreamingAssetsPath);
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

        private void InitWindows(VisualElement parent)
        {
            CreateLabel(parent, "Packages");
            VisualElement container = CreateContainer(parent);

            CreateButton(container, "Assets/Import Package/Custom Package...", "Import Custom Package");

            var skip = true;
            var groupName = "";

            foreach (var submenu in Unsupported.GetSubmenus("Window"))
            {
                var textInfo = CultureInfo.InvariantCulture.TextInfo;
                var upper = textInfo.ToUpper(submenu);
                if (skip)
                {
                    if (upper == "WINDOW/PACKAGE MANAGER")
                    {
                        skip = false;
                    }
                    else continue;
                }

                var parts = submenu.Split('/');
                var firstPart = parts[1];

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

        private static void CreateLabel(VisualElement parent, string text)
        {
            Label label = new Label(text)
            {
                style =
                {
                    fontSize = 12,
                    marginTop = 3,
                    marginLeft = 3,
                    marginRight = 3,
                    paddingLeft = 5
                }
            };
            parent.Add(label);
        }

        private static void CreateButton(VisualElement parent, string submenu, string text)
        {
            Button button = new Button(() => EditorApplication.ExecuteMenuItem(submenu))
            {
                text = text,
                style =
                {
                    left = 0,
                    unityTextAlign = TextAnchor.MiddleCenter,
                }
            };
            parent.Add(button);
        }

        private VisualElement CreateContainer(VisualElement parent)
        {
            VisualElement ve = new VisualElement
            {
                style =
                {
                    borderTopWidth = 1f,
                    borderBottomWidth = 1f,
                    borderTopColor = Color.black,
                    borderBottomColor = Color.black,
                    marginTop = 2,
                },
            };
            parent.Add(ve);
            return ve;
        }
    }
}