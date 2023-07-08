namespace SnapshotShaders.BuiltIn
{
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    public class ExampleScript : MonoBehaviour
    {
        public PostProcessVolume volume;

        // Update is called once per frame
        void Update()
        {
            OilPainting oilPaintingEffect = null;
            volume.profile.TryGetSettings(out oilPaintingEffect);

            if (oilPaintingEffect != null)
            {
                oilPaintingEffect.kernelSize.value = 100;
            }
        }
    }
}
