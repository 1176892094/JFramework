//-----------------------------------------------------------------------
// <copyright file="OdinUpgrader.cs" company="Sirenix ApS">
// Copyright (c) Sirenix ApS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Sirenix.OdinInspector.Internal
{
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public static class OdinUpgrader
    {
        private static int counter = 0;
        private const int NUM_OF_FRAMES_WITHOUT_RECOMPILE = 10;
        private static bool DEBUG = false;
        private static int numberOfTimesCalled = 0;

        [InitializeOnLoadMethod]
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void Update()
        {
            bool dontDoItYouAreTooYoung =
                AssemblyUtilities.GetTypeByCachedFullName("Sirenix.Internal.UnitTests.Core.Editor.UnitTestEditorUtility") != null ||
                EditorPrefs.HasKey("PREVENT_SIRENIX_FILE_GENERATION");

            // Since SirenixPathLookupScriptableObject is not in a persistent assembly, we'll need to fix its script reference guid.
            if (dontDoItYouAreTooYoung)
            {
                if (DEBUG) Debug.Log(new DirectoryInfo(SirenixAssetPaths.SirenixAssembliesPath).FullName);
                if (DEBUG) Debug.Log("Didn't do it.");
                return;
            }

            if (!File.Exists(SirenixAssetPaths.SirenixPluginPath + "Odin Inspector/OdinUpgrader.cs"))
            {
                if (DEBUG) Debug.Log("The updater doesn't exist, which means the OdinUpgrader was probably just executed and deleted itself, but the program is still in memory.");
                return;
            }

            if (numberOfTimesCalled <= 2)
            {
                // EditorApplication.isCompiling gives an error in 2017 that can't be silenced if called from InitializeOnLoadMethod or DidReloadScripts.
                // We'll just wait a few frames and then call it.
                UnityEditorEventUtility.DelayAction(Update, true);
                numberOfTimesCalled++;
                return;
            }

            if (counter == NUM_OF_FRAMES_WITHOUT_RECOMPILE)
            {
                if (DEBUG) Debug.Log("Upgrading");
                Upgrade();
            }
            else
            {
                bool isCompiling = true;

                try
                {
                    isCompiling = EditorApplication.isCompiling;
                }
                catch
                {
                }
                finally
                {
                    counter = isCompiling ? 0 : counter + 1;
                }

                if (DEBUG) Debug.Log("Counting " + counter);
                UnityEditorEventUtility.DelayAction(Update);
            }
        }

        private static void Upgrade()
        {
            var directoriesToDelete = new List<string>();
            var filesToDelete = new List<string>();

            // Delete old mdb files - pdb files now work fine.
            if (Directory.Exists(SirenixAssetPaths.SirenixAssembliesPath))
            {
                filesToDelete.AddRange(new DirectoryInfo(SirenixAssetPaths.SirenixAssembliesPath).GetFiles("*.mdb", SearchOption.AllDirectories).Select(x => x.FullName));
            }

            // We no longer have Sirenix specific assets (Icon data are now embedded in the code)
            directoriesToDelete.Add(SirenixAssetPaths.SirenixAssetsPath);

            // Demo packages are located directly in the demo folder -> All directories are old unpacked demos that needs to be deleted.
            if (Directory.Exists(SirenixAssetPaths.SirenixPluginPath + "Demos"))
            {
                directoriesToDelete.AddRange(new DirectoryInfo(SirenixAssetPaths.SirenixPluginPath + "Demos").GetDirectories().Select(x => x.FullName));
            }

            // Delete the upgrader itself (this script).
            filesToDelete.Add(SirenixAssetPaths.SirenixPluginPath + "Odin Inspector/Scripts/Editor/OdinUpgrader.cs");
            filesToDelete.Add(SirenixAssetPaths.SirenixPluginPath + "Odin Inspector/OdinUpgrader.cs");

            // The getting started guide is no longer an asset.
            filesToDelete.Add(SirenixAssetPaths.SirenixPluginPath + "Getting Started With Odin.asset");

            // The sample projects guide is no longer an asset.
            filesToDelete.Add(SirenixAssetPaths.SirenixPluginPath + "Odin Inspector/Assets/Sample Projects.asset");

            // Delete the old wizard.
            filesToDelete.Add(SirenixAssetPaths.SirenixPluginPath + "Odin Inspector/Scripts/Editor/OdinGettingStartedWizard.cs");

            // Odin Attributes Overview is renamed to Attributes Overview.
            filesToDelete.Add(SirenixAssetPaths.SirenixPluginPath + "Demos/Odin Attributes Overview.unitypackage");

            // Custom Drawer Examples is renamed to Custom Drawers.
            filesToDelete.Add(SirenixAssetPaths.SirenixPluginPath + "Demos/Custom Drawer Examples.unitypackage");

            // Removal of old compatibility layer
            directoriesToDelete.Add(SirenixAssetPaths.SirenixPluginPath + "Odin Inspector/Scripts");

            AssetDatabase.StartAssetEditing();
            try
            {
                DeleteDirsAndFiles(directoriesToDelete, filesToDelete);

                // Re-enabled editor only mode.
                if (EditorOnlyModeConfig.Instance.IsEditorOnlyModeEnabled())
                {
                    EditorOnlyModeConfig.Instance.EnableEditorOnlyMode(force: true);
                }
            }
            finally
            {
                // Open Getting Started window after recompilation:
                EditorPrefs.SetBool("ODIN_INSPECTOR_SHOW_GETTING_STARTED", true);

                // Open Getting Started window after recompilation:
                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
        }

        private static void DeleteDirsAndFiles(List<string> directoriesToDelete, List<string> filesToDelete)
        {
            foreach (var dir in directoriesToDelete.Select(x => x.Replace('\\', '/')))
            {
                var mdb = dir + ".mdb";
                var dirExist = Directory.Exists(dir);
                var mdbExist = File.Exists(mdb);

                if (DEBUG) Debug.Log("Dir exist: " + (dirExist ? 1 : 0) + " Mdb exist: " + (mdbExist ? 1 : 0) + " Path: " + dir);

                if (dirExist)
                {
                    var paths = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
                    for (int i = 0; i < paths.Length; i++)
                    {
                        var p = paths[i].Replace('\\', '/');
                        DeleteFile(p);
                    };

                    DeleteDirectory(dir);
                }

                DeleteFile(mdb);
            }

            // Delete all files.
            foreach (var file in filesToDelete.Select(x => x.Replace('\\', '/')))
            {
                var mdb = file + ".mdb";
                bool existFile = File.Exists(file);
                bool existMdb = File.Exists(mdb);

                if (DEBUG) Debug.Log("File exist: " + (existFile ? 1 : 0) + " Mdb exist: " + (existMdb ? 1 : 0) + " Path: " + file);
                DeleteFile(file);
                DeleteFile(mdb);
            }
        }

        private static void DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            var metaFile = file + ".meta";
            if (File.Exists(metaFile))
            {
                try
                {
                    File.Delete(metaFile);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        private static void DeleteDirectory(string dir)
        {
            if (Directory.Exists(dir))
            {
                try
                {
                    Directory.Delete(dir, true);

                    if (Directory.Exists(dir))
                        Directory.Delete(dir);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            var metaFile = dir + ".meta";
            if (File.Exists(metaFile))
            {
                try
                {
                    File.Delete(metaFile);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
    }
}
