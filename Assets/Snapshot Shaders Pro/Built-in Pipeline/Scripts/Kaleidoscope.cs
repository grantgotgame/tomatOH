namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(KaleidoscopeRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Kaleidoscope")]
    public sealed class Kaleidoscope : PostProcessEffectSettings
    {
        [Range(0.0f, 20.0f), Tooltip("The number of radial segments.")]
        public FloatParameter segmentCount = new FloatParameter { value = 6.0f };
    }

    public sealed class KaleidoscopeRenderer : PostProcessEffectRenderer<Kaleidoscope>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Kaleidoscope"));

            sheet.properties.SetFloat("_SegmentCount", settings.segmentCount);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
