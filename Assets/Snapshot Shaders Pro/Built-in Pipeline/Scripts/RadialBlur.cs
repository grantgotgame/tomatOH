namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(RadialBlurRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/RadialBlur")]
    public class RadialBlur : PostProcessEffectSettings
    {
        [Range(3, 50), Tooltip("Blur Strength")]
        public IntParameter strength = new IntParameter { value = 5 };

        [Range(0.0f, 1.0f), Tooltip("Proportion of the screen which is unblurred.")]
        public FloatParameter focalSize = new FloatParameter { value = 0.25f };
    }

    public sealed class RadialBlurRenderer : PostProcessEffectRenderer<RadialBlur>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/RadialBlur"));
            sheet.properties.SetInt("_KernelSize", settings.strength);
            sheet.properties.SetFloat("_Spread", settings.strength / 7.5f);
            sheet.properties.SetFloat("_FocalSize", settings.focalSize);

            var tmp = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);

            context.command.BlitFullscreenTriangle(context.source, tmp, sheet, 0);
            context.command.BlitFullscreenTriangle(tmp, context.destination, sheet, 1);

            RenderTexture.ReleaseTemporary(tmp);
        }
    }
}
