using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System.Linq;
using System.IO;

namespace Refsa.ReInput.Editor
{
    [InitializeOnLoad]
    public static class ReInputPackageHandler
    {
        static SearchRequest searchRequest;
        static Request packageRequest;
        static ListRequest listRequest;

        static PackRequest packRequest;
        static AddRequest addRequest;

        static string[] wantedPackages = new string[]
        {
            "com.unity.inputsystem"
        };

        static int currentPackage = 0;

        static ReInputPackageHandler()
        {
            currentPackage = 0;

            listRequest = Client.List();
            EditorApplication.update += FindPackages;
        }

        static void FindPackages()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    foreach (var wantedPackage in wantedPackages)
                    {
                        if (!listRequest.Result.Any(e => e.name == wantedPackage))
                        {
                            addRequest = Client.Add(wantedPackage);
                            EditorApplication.update += WaitForPackageInstall;
                        }
                    }
                }
                else if (listRequest.Status == StatusCode.Failure)
                {
                    Debug.Log("ReInput: " + listRequest.Error.message);
                }

                EditorApplication.update -= FindPackages;
            }
        }

        static void WaitForPackageInstall()
        {
            if (packRequest.IsCompleted)
            {
                if (packRequest.Status == StatusCode.Failure)
                {
                    Debug.Log(packRequest.Error.message);
                }
                else if (packRequest.Status == StatusCode.Success)
                {
                    ReplaceInputManagerSettings();
                }
            }

            EditorApplication.update -= WaitForPackageInstall;
        }

        [MenuItem("Tools/ReInput/Setup")]
        static void Setup()
        {
            FindPackages();
            ReplaceInputManagerSettings();
        }

        static void ReplaceInputManagerSettings()
        {
            var settingsPath = LocateFile(Application.dataPath + "/", "InputManager.asset");
            var isPath = Application.dataPath.Replace("/Assets", "/ProjectSettings/InputManager.asset");

            File.Replace(settingsPath, isPath, "InputManager.asset.old");
            File.Copy(isPath, settingsPath);
        }
        
        static string LocateFile(string path, string fileName)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                if (Path.GetFileName(file) == fileName)
                {
                    return file;
                }
            }

            foreach (var folder in Directory.GetDirectories(path))
            {
                var file = LocateFile(folder, fileName);
                if (!string.IsNullOrEmpty(file))
                {
                    return file;
                }
            }

            return "";
        }

#if ReInputDev
        [MenuItem ("Tools/ReInput/Create ReInput Package")]
#endif
        static void CreatePackage()
        {
            packRequest = Client.Pack("Assets/ReInput/", "Assets/Release/");

            EditorApplication.update += WaitForPacking;
        }

        static void WaitForPacking()
        {
            if (packRequest.IsCompleted)
            {
                if (packRequest.Status == StatusCode.Failure)
                {
                    Debug.Log(packRequest.Error.message);
                }
                else if (packRequest.Status == StatusCode.Success)
                {
                    Debug.Log(packRequest.Result.tarballPath);
                    AssetDatabase.Refresh();
                }

                EditorApplication.update -= WaitForPacking;
            }
        }
    }
}
