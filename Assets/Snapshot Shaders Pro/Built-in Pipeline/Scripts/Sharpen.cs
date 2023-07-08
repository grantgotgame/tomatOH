namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(SharpenRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Sharpen")]
    public sealed class Sharpen : PostProcessEffectSettings
    {
        [Range(0f, 1f), Tooltip("Sharpen effect intensity.")]
        public FloatParameter intensity = new FloatParameter { value = 0.25f };
    }

    public sealed class SharpenRenderer : PostProcessEffectRenderer<Sharpen>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Sharpen"));
            sheet.properties.SetFloat("_Intensity", settings.intensity);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
