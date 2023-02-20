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

        private readonly string _targetSceneName = "9845994255863274994b7d033e3bdc76";
        private readonly string _targetRootName = "//-----ENVIRONMENT";
        private readonly string _targetObjectName = "HOLODECKS";

        private GameObject _holodeckParent = null;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == _targetSceneName)
            {
                MelonLogger.Msg("BONELAB Hub loaded: Activating HoloDecks");

                ActivateHolodeckParent();
                ModifyChildGameObjects(_holodeckParent);
            }
        }

        private void ActivateHolodeckParent()
        {
            var rootObjects = GetSceneRootObjects(_targetSceneName);

            foreach (var rootObject in rootObjects)
            {
                rootObject.Cast<GameObject>();
                if (rootObject.name == _targetRootName)
                {
                    _holodeckParent = GetObjectInChildren(_targetObjectName, rootObject);
                    break;
                }
            }

            _holodeckParent.SetActive(true);
        }

        private void ModifyChildGameObjects(GameObject targetObject)
        {
            var renderers = targetObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers)
            {
                renderer.enabled = false;

                var rendererParentTransform = renderer.transform.parent;
                if (rendererParentTransform.name == "Holodeck_walls_05")
                {
                    rendererParentTransform.localPosition = new Vector3(-25.1f, -3.1f, rendererParentTransform.localPosition.z);
                }
                else
                {
                    rendererParentTransform.localPosition = new Vector3(rendererParentTransform.localPosition.x, -3.1f, rendererParentTransform.localPosition.z);
                }
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
