namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(PixelateRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Pixelate")]
    public class Pixelate : PostProcessEffectSettings
    {
        [Range(1, 128), Tooltip("Size of each new 'pixel' in the image.")]
        public IntParameter pixelSize = new IntParameter { value = 4 };
    }

    public sealed class PixelateRenderer : PostProcessEffectRenderer<Pixelate>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Base"));
            var tmp = RenderTexture.GetTemporary(Screen.width / settings.pixelSize, Screen.height / settings.pixelSize, 0);
            tmp.filterMode = FilterMode.Point;

            context.command.BlitFullscreenTriangle(context.source, tmp, sheet, 0);
            context.command.BlitFullscreenTriangle(tmp, context.destination, sheet, 0);

            RenderTexture.ReleaseTemporary(tmp);
        }
    }
}
