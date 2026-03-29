using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Warehouse;
using Il2CppSLZ.Marrow.Zones;
using Il2CppSLZ.Utilities;
using Il2CppSystem.Collections.Generic;
using Il2CppUltEvents;
using MelonLoader;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace ShowHolodecks
{
    public static class BuildInfo
    {
        public const string Name = "ShowHolodecks"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "yellowyears"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.2.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = "https://bonelab.thunderstore.io/package/yellowyears/ShowHolodecks/"; // Download Link for the Mod.  (Set as null if none)
    }

    internal class ShowHolodecks : MelonMod
    {
        // Strings used to identify GameObjects and Scenes
        private const string TargetSceneName = "scene_BoneLab_Hub_Lab_Floor";
        private const string TargetRootName = "//-----ENVIRONMENT";
        private const string TargetObjectName = "HOLODECKS";
                
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName != TargetSceneName) return;
            
            MelonLogger.Msg("BONELAB Hub loaded: Activating HoloDecks");

            FixHolodecks();
        }
        
        private void FixHolodecks()
        {
            // Since GameObject.Find() doesn't find inactive objects call GetSceneRootObjects()
            var rootObjects = GetSceneRootGameObjects(TargetSceneName);
            GameObject holodecksParent = new GameObject();

            // Loop through all scene root objects until HOLODECK's parent object (//-----ENVIRONMENT) is found
            foreach (var rootObject in rootObjects)
            {
                rootObject.Cast<GameObject>(); // Cast Il2CppReferenceArray<GameObject> element to GameObject
                if (rootObject.name != TargetRootName) continue;
                
                holodecksParent = GetObjectInChildren(TargetObjectName, rootObject); // Get the HOLODECKS GameObject
                break;
            }

            // Fix each holodeck
            foreach (var childTransform in holodecksParent.transform)
            {
                Transform child = childTransform.Cast<Transform>();

                FixHolodeck(child.gameObject);
            }

            holodecksParent.SetActive(true);
        }

        private void FixHolodeck(GameObject holodeckObj)
        {
            var tweenBlendshapes = new List<TweenBlendshape>();

            var renderers = holodeckObj.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers)
            {
                var tweenBlendshape = renderer.gameObject.GetComponent<TweenBlendshape>();
                if (tweenBlendshape != null)
                {
                    tweenBlendshapes.Add(tweenBlendshape);
                }

                // Fix any clipping/positioning issues
                switch (holodeckObj.name)
                {
                    case "Holodeck_walls_03":
                        break;
                    case "Holodeck_walls_05":
                        break;
                    default:
                        break;
                }
            }

            Holodeck holodeck = holodeckObj.AddComponent<Holodeck>();
            holodeck.tweenBlendshapes = tweenBlendshapes;

            GenericTrigger trigger = holodeck.GetComponentInChildren<GenericTrigger>();

            // Replace trigger with zone
            Zone zone = trigger.gameObject.AddComponent<Zone>();
            zone.gameObject.layer = LayerMask.NameToLayer("EntityTrigger");

            ZoneEvents zoneEvents = trigger.gameObject.AddComponent<ZoneEvents>();
            zoneEvents._zone = zone;

            // Setup activator tag
            var query = new TagQuery();
            BoneTagReference btRef = new BoneTagReference(MarrowSettings.RuntimeInstance.PlayerTag.Barcode);
            query.BoneTag = btRef;
            zoneEvents.activatorTags.Tags.Add(query);

            // Add listener to zone enter and zone exit
            holodeck.zoneEvents = zoneEvents;
            holodeck.Setup();

            holodeck.Deactivate(); // start deactivated
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