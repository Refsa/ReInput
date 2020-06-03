
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Refsa.OmegaInput
{
    public class OmegaInputBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnSubsystemRegistrationHook()
        {
            SceneManager.sceneLoaded -= SetupRequiredComponents;
            SceneManager.sceneLoaded += SetupRequiredComponents;
        }

        private static void SetupRequiredComponents(Scene arg0, LoadSceneMode arg1)
        {
            var exists = GameObject.FindObjectOfType<OmegaInputController>();

            if (exists == null)
            {
                var inputGameObject = new GameObject();
                inputGameObject.name = "OmegaInputManager";

                exists = inputGameObject.AddComponent<OmegaInputController>();
            }

            exists.Setup();
        }
    }
}