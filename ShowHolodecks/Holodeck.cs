using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow.Interaction;
using Il2CppSLZ.Marrow.Zones;
using Il2CppSystem.Collections.Generic;
using Il2CppUltEvents;
using MelonLoader;
using System;
using UnityEngine;

namespace ShowHolodecks
{
    [RegisterTypeInIl2Cpp]
    internal class Holodeck : MonoBehaviour
    {
        public Holodeck(IntPtr intPtr) : base(intPtr) { }

        public List<TweenBlendshape> tweenBlendshapes = new List<TweenBlendshape>();

        public void Setup(ZoneEvents zoneEvents)
        {
            UltEvent<MarrowEntity> zoneEnter = zoneEvents.onZoneEnter.Cast<UltEvent<MarrowEntity>>();
            Action<MarrowEntity> activate = (MarrowEntity marrowEntity) =>
            {
                Activate();
            };
            UltEvent<MarrowEntity>.AddDynamicCall(ref zoneEnter, activate);

            UltEvent<MarrowEntity> zoneExit = zoneEvents.onZoneExit.Cast<UltEvent<MarrowEntity>>();
            Action<MarrowEntity> deactivate = (MarrowEntity marrowEntity) =>
            {
                Deactivate();
            };
            UltEvent<MarrowEntity>.AddDynamicCall(ref zoneExit, deactivate);
        }

        public void Activate()
        {
            foreach (var tween in tweenBlendshapes)
            {
                tween.TweenOn(0.5f);
            }
        }

        public void Deactivate()
        {
            foreach (var tween in tweenBlendshapes)
            {
                tween.TweenOff(0.5f);
            }
        }
    }
}