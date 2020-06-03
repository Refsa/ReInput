using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Refsa.OmegaInput.Editor
{
    [InitializeOnLoad]
    public static class OmegaInputPackageHandler
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

        static OmegaInputPackageHandler ( )
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

        [MenuItem ("OmegaInput/Create OmegaInput Package")]
        static void CreatePackage ( )
        {
            packRequest = Client.Pack ("Assets/OmegaInput/", "Assets/Release/");

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
