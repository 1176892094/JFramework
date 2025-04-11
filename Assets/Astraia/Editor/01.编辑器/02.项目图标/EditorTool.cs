// // *********************************************************************************
// // # Project: Astraia
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 19:04:50
// // # Recently: 2025-04-09 19:04:50
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Astraia
{
    internal static class EditorTool
    {
        public static readonly Dictionary<string, Icon> icons = new Dictionary<string, Icon>
        {
            { "Animations", Icon.Animations },
            { "Resources", Icon.Resources },
            { "Scenes", Icon.Scenes },
            { "Scripts", Icon.Scripts },
            { "Plugins", Icon.Plugins },
            { "Materials", Icon.Materials },
            { "Extensions", Icon.Editor },
            { "Audios", Icon.Audios },
            { "Prefabs", Icon.Prefabs },
            { "Models", Icon.Meshes },
            { "Settings", Icon.Project },
            { "Shaders", Icon.Shaders },
            { "Fonts", Icon.Fonts },
            { "Textures", Icon.Textures },
            { "StreamingAssets", Icon.Resources },
            { "Physics", Icon.Physics },
            { "Terrains", Icon.Terrains },
            { "Tilemaps", Icon.Terrains },
            { "Lights", Icon.Lights },
            { "Process", Icon.Lights },
            { "Editor", Icon.Editor },
            { "Android", Icon.Android },
            { "iOS", Icon.IPhone },
            { "Windows", Icon.Windows },
            { "MacOS", Icon.MacOS },
            { "WebGL", Icon.WebGL },
            { "DataTable", Icon.Project },
            { "Atlas", Icon.Meshes },
            { "Icons", Icon.Textures },
            { "HotUpdate", Icon.Scripts },
            { "Template", Icon.Resources },
        };

        public static Texture2D GetIcon<T>(T value, Dictionary<T, Lazy<string>> strings, Dictionary<T, Texture2D> textures) where T : Enum
        {
            if (textures.TryGetValue(value, out var texture))
            {
                return texture;
            }

            if (!strings.TryGetValue(value, out var result))
            {
                texture = Texture2D.grayTexture;
                textures.Add(value, texture);
                return texture;
            }

            texture = new Texture2D(4, 4, TextureFormat.DXT5, false)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                hideFlags = HideFlags.HideAndDontSave
            };
            texture.LoadImage(Convert.FromBase64String(result.Value));
            textures.Add(value, texture);
            return texture;
        }
    }
}