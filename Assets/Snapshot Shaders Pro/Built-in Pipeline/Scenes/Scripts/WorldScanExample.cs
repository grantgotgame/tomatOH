namespace SnapshotShaders.BuiltIn
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    public class WorldScanExample : MonoBehaviour
    {
        [SerializeField] private float scanSpeed;
        [SerializeField] private float scanDuration;

        [SerializeField] private PostProcessVolume worldScanVolume;
        private WorldScan worldScanEffect;

        private void Start()
        {
            if(worldScanVolume == null || worldScanVolume.profile == null)
            {
                return;
            }
            worldScanVolume.profile.TryGetSettings(out worldScanEffect);
        }

        private void Update()
        {
            if (worldScanEffect != null)
            {
                var t = Time.time % scanDuration;
                var distance = t * scanSpeed;

                worldScanEffect.scanDistance.value = distance;
            }
        }
    }
}
