using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[InitializeOnLoad]
public static class OmegaInputPackageHandler
{
    static SearchRequest searchRequest;
    static Request packageRequest;
    static ListRequest listRequest;

    static string[ ] wantedPackages = new string[ ]
    {
        "com.unity.inputsystem"
    };

    static int currentPackage = 0;

    static OmegaInputPackageHandler ( )
    {
        currentPackage = 0;

        listRequest = Client.List ( );

        EditorApplication.update += FindPackages;
    }

    static void FindPackages ( )
    {
        if (listRequest.IsCompleted)
        {
            if (listRequest.Status == StatusCode.Success)
            {
                foreach (var package in listRequest.Result) { }
            }
            else if (listRequest.Status == StatusCode.Failure)
            {
                Debug.Log (listRequest.Error.message);
            }

            EditorApplication.update -= FindPackages;
        }
    }
}
