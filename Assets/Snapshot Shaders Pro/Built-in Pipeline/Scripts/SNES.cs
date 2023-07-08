namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(SNESRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/SNES")]
    public class SNES : PostProcessEffectSettings
    {
        [Range(3, 16), Tooltip("Number of quantisation bands (per channel).")]
        public IntParameter bandingLevels = new IntParameter { value = 6 };

        [Range(0.0f, 4.0f), Tooltip("Modify the input colors via a power ramp. 1 = original mapping, " +
            "higher = favors darker output, lower = favors lighter output.")]
        public FloatParameter powerRamp = new FloatParameter { value = 1.0f };
    }

    public sealed class SNESRenderer : PostProcessEffectRenderer<SNES>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/SNES"));
            sheet.properties.SetInt("_BandingLevels", settings.bandingLevels);
            sheet.properties.SetFloat("_PowerRamp", settings.powerRamp);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
