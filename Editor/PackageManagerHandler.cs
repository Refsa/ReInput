using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Refsa.ReInput.Editor
{
    [InitializeOnLoad]
    public static class ReInputPackageHandler
    {
        static SearchRequest searchRequest;
        static Request packageRequest;
        static ListRequest listRequest;

        static PackRequest packRequest;

        static string[ ] wantedPackages = new string[ ]
        {
            "com.unity.inputsystem"
        };

        static int currentPackage = 0;

        static ReInputPackageHandler ( )
        {
            currentPackage = 0;

            //listRequest = Client.List ( );
            //EditorApplication.update += FindPackages;
        }

        static void FindPackages ( )
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    foreach (var package in listRequest.Result)
                    {
                        Debug.Log (package.name);
                    }
                }
                else if (listRequest.Status == StatusCode.Failure)
                {
                    Debug.Log (listRequest.Error.message);
                }

                EditorApplication.update -= FindPackages;
            }
        }

#if ReInputDev
        [MenuItem ("Tools/ReInput/Create ReInput Package")]
#endif
        static void CreatePackage ( )
        {
            packRequest = Client.Pack ("Assets/ReInput/", "Assets/Release/");

            EditorApplication.update += WaitForPacking;
        }

        static void WaitForPacking ( )
        {
            if (packRequest.IsCompleted)
            {
                if (packRequest.Status == StatusCode.Failure)
                {
                    Debug.Log (packRequest.Error.message);
                }
                else if (packRequest.Status == StatusCode.Success)
                {
                    Debug.Log (packRequest.Result.tarballPath);
                    AssetDatabase.Refresh ( );
                }

                EditorApplication.update -= WaitForPacking;
            }
        }
    }
}
