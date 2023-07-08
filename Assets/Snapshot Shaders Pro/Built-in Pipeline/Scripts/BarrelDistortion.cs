namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(BarrelDistortionRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/BarrelDistortion")]
    public sealed class BarrelDistortion : PostProcessEffectSettings
    {
        [Tooltip("Color of the background around the 'screen'.")]
        public ColorParameter backgroundColor = new ColorParameter { value = Color.black };

        [Tooltip("Strength of the distortion. Values above zero cause CRT screen-like distortion; values below zero bulge outwards.")]
        public FloatParameter strength = new FloatParameter { value = 0.0f };
    }

    public sealed class BarrelDistortionRenderer : PostProcessEffectRenderer<BarrelDistortion>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/BarrelDistortion"));
            sheet.properties.SetColor("_BackgroundColor", settings.backgroundColor);
            sheet.properties.SetFloat("_Strength", settings.strength);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
