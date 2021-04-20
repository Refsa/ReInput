
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ReInput
{
    public class ReInputBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnSubsystemRegistrationHook()
        {
            SceneManager.sceneLoaded -= SetupRequiredComponents;
            SceneManager.sceneLoaded += SetupRequiredComponents;
        }

        private static void SetupRequiredComponents(Scene arg0, LoadSceneMode arg1)
        {
            var exists = GameObject.FindObjectOfType<ReInputController>();

            if (exists == null)
            {
                var inputGameObject = new GameObject();
                inputGameObject.name = "ReInputManager";
                GameObject.DontDestroyOnLoad(inputGameObject);

                exists = inputGameObject.AddComponent<ReInputController>();
            }
        }
    }
}