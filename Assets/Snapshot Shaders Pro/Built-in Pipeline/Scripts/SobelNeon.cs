namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(SobelNeonRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/SobelNeon")]
    public class SobelNeon : PostProcessEffectSettings
    {
        [Range(0.0f, 1.0f), Tooltip("Saturation values lower than this will be clamped to this.")]
        public FloatParameter saturationFloor = new FloatParameter { value = 1.0f };

        [Range(0.0f, 1.0f), Tooltip("Lightness/value values lower than this will be clamped to this.")]
        public FloatParameter lightnessFloor = new FloatParameter { value = 1.0f };
    }

    public sealed class SobelNeonRenderer : PostProcessEffectRenderer<SobelNeon>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/SobelNeon"));

            sheet.properties.SetFloat("_SaturationFloor", settings.saturationFloor);
            sheet.properties.SetFloat("_LightnessFloor", settings.lightnessFloor);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
