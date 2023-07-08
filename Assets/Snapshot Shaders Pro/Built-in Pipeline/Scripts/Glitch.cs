namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(GlitchRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Glitch")]
    public sealed class Glitch : PostProcessEffectSettings
    {
        [Tooltip("Texture which controls the strength of the glitch offset based on y-coordinate.")]
        public TextureParameter offsetTexture = new TextureParameter();

        [Range(0f, 2.0f), Tooltip("Glitch effect intensity.")]
        public FloatParameter offsetStrength = new FloatParameter { value = 0.05f };

        [Range(0.0f, 25.0f), Tooltip("Controls how many times the glitch texture repeats vertically.")]
        public FloatParameter verticalTiling = new FloatParameter { value = 5.0f };
    }

    public sealed class GlitchRenderer : PostProcessEffectRenderer<Glitch>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Glitch"));
            sheet.properties.SetTexture("_OffsetTex", settings.offsetTexture);
            sheet.properties.SetFloat("_OffsetStrength", settings.offsetStrength);
            sheet.properties.SetFloat("_VerticalTiling", settings.verticalTiling);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
