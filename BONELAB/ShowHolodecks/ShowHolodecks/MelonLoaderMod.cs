using Il2CppSystem.Collections.Generic;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShowHolodecks
{
    public static class BuildInfo
    {
        public const string Name = "ShowHolodecks"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "yellowyears"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class ShowHolodecks : MelonMod
    {

        private string targetSceneName = "9845994255863274994b7d033e3bdc76";
        private string targetRootName = "//-----ENVIRONMENT";
        private string targetObjectName = "HOLODECKS";

        private GameObject holodeckParent = null;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == targetSceneName)
            {
                MelonLogger.Msg("BONELAB Hub loaded: Activating HoloDecks");

                ActivateHolodeckParent();
                DisableSkinnedMeshRenderers(holodeckParent);
            }
        }

        private void ActivateHolodeckParent()
        {
            var rootObjects = GetSceneRootObjects(targetSceneName);

            foreach (var rootObject in rootObjects)
            {
                rootObject.Cast<GameObject>();
                if (rootObject.name == targetRootName)
                {
                    holodeckParent = GetObjectInChildren(targetObjectName, rootObject);
                    break;
                }
            }

            holodeckParent.SetActive(true);
        }

        private void DisableSkinnedMeshRenderers(GameObject targetObject)
        {
            var renderers = targetObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers)
            {
                renderer.enabled = false;
            }
        }

        private Il2CppReferenceArray<GameObject> GetSceneRootObjects(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            return scene.GetRootGameObjects();
        }

        private GameObject GetObjectInChildren(string targetObjectName, GameObject parentGameObject)
        {
            for (var i = 0; i < parentGameObject.transform.GetChildCount(); i++)
            {
                var childGameObject = parentGameObject.transform.GetChild(i);
                if (childGameObject.name == targetObjectName)
                {
                    return childGameObject.gameObject;
                }
            }
            return null;
        }

    }
}
