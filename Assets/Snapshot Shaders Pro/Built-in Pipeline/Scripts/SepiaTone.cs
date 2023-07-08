namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(SepiaToneRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Sepia Tone")]
    public sealed class SepiaTone : PostProcessEffectSettings
    {
        [Range(0f, 1f), Tooltip("Sepia Tone effect intensity.")]
        public FloatParameter blend = new FloatParameter { value = 0.5f };
    }

    public sealed class SepiaToneRenderer : PostProcessEffectRenderer<SepiaTone>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/SepiaTone"));
            sheet.properties.SetFloat("_Blend", settings.blend);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
