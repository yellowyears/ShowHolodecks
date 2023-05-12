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
        public const string Version = "1.0.2"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = "https://bonelab.thunderstore.io/package/yellowyears/ShowHolodecks/"; // Download Link for the Mod.  (Set as null if none)
    }

    internal class ShowHolodecks : MelonMod
    {
        // Strings used to identify GameObjects and Scenes
        private const string TargetSceneName = "9845994255863274994b7d033e3bdc76";
        private const string TargetRootName = "//-----ENVIRONMENT";
        private const string TargetObjectName = "HOLODECKS";
        
        
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName != TargetSceneName) return;
            
            MelonLogger.Msg("BONELAB Hub loaded: Activating HoloDecks");

            ActivateHolodecks();
        }
        
       
        private void ActivateHolodecks()
        {
            // Since GameObject.Find() doesn't find inactive objects call GetSceneRootObjects()
            var rootObjects = GetSceneRootGameObjects(TargetSceneName);
            var holodeckParent = new GameObject();

            // Loop through all scene root objects until HOLODECK's parent object (//-----ENVIRONMENT) is found
            foreach (var rootObject in rootObjects)
            {
                rootObject.Cast<GameObject>(); // Cast Il2CppReferenceArray<GameObject> element to GameObject
                if (rootObject.name != TargetRootName) continue;
                
                holodeckParent = GetObjectInChildren(TargetObjectName, rootObject); // Get the HOLODECKS GameObject
                break;
            }

            // Set the parent object to true and apply changes to renderer objects
            holodeckParent.SetActive(true);
            ModifyChildGameObjects(holodeckParent);
        }
        

        // Takes a parent object, finds the SkinnedMeshRenderer components, disables them and adjusts the position of them
        private static void ModifyChildGameObjects(GameObject targetObject)
        {
            var renderers = targetObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers)
            {
                renderer.enabled = false;

                // Caching
                var rendererParentTransform = renderer.transform.parent;
                var rendererParentTransformLocalPos = rendererParentTransform.localPosition;
                
                // Reposition the parent object to prevent the holodeck from floating off the ground
                // Special case for Holodeck_walls_05 (Arena module) since it needs to be pushed backwards on the x axis
                rendererParentTransform.localPosition = rendererParentTransform.name == "Holodeck_walls_05"
                    ? new Vector3(-25.1f, -3.1f, rendererParentTransformLocalPos.z)
                    : new Vector3(rendererParentTransform.localPosition.x, -3.1f, rendererParentTransformLocalPos.z);
            }
        }
        
        // Gets each root GameObject in a scene and return them to an Il2CppReferenceArray of GameObjects
        private static Il2CppReferenceArray<GameObject> GetSceneRootGameObjects(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            return scene.GetRootGameObjects();
        }

        
        // Returns a specific GameObject that is the child of a parent GameObject
        private static GameObject GetObjectInChildren(string targetObjectName, GameObject parentGameObject)
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
