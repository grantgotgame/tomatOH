namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(GreyscaleRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Greyscale")]
    public sealed class Greyscale : PostProcessEffectSettings
    {
        [Range(0f, 1f), Tooltip("Greyscale effect intensity.")]
        public FloatParameter blend = new FloatParameter { value = 0.5f };
    }

    public sealed class GreyscaleRenderer : PostProcessEffectRenderer<Greyscale>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Greyscale"));
            sheet.properties.SetFloat("_Blend", settings.blend);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
