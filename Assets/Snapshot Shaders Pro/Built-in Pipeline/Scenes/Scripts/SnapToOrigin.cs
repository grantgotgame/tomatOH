namespace SnapshotShaders.BuiltIn
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    public class SnapToOrigin : MonoBehaviour
    {
        [SerializeField]
        private PostProcessVolume volume;

        private WorldScan worldScanEffect = null;

        private void Start()
        {
            volume.profile.TryGetSettings(out worldScanEffect);
        }

        private void Update()
        {
            if(worldScanEffect != null)
            {
                transform.position = worldScanEffect.scanOrigin;
            }
        }
    }
}
