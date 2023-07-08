namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(ScanlinesRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Scanlines")]
    public class Scanlines : PostProcessEffectSettings
    {
        [Tooltip("Scanlines Texture.")]
        public TextureParameter scanlineTex = new TextureParameter();

        [Range(0.0f, 1.0f), Tooltip("Strength of the effect.")]
        public FloatParameter strength = new FloatParameter { value = 0.0f };

        [Range(1, 64), Tooltip("Pixel size of the scanlines.")]
        public IntParameter size = new IntParameter { value = 8 };

        [Range(0.0f, 10.0f), Tooltip("Scroll speed of scanlines vertically.")]
        public FloatParameter scrollSpeed = new FloatParameter { value = 0.0f };
    }

    public sealed class ScanlinesRenderer : PostProcessEffectRenderer<Scanlines>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Scanlines"));

            if (settings.scanlineTex != null)
            {
                sheet.properties.SetTexture("_ScanlineTex", settings.scanlineTex);
            }
            else
            {
                sheet.properties.SetTexture("_ScanlineTex", Texture2D.whiteTexture);
            }

            sheet.properties.SetFloat("_Strength", settings.strength);
            sheet.properties.SetInt("_Size", settings.size);
            sheet.properties.SetFloat("_ScrollSpeed", settings.scrollSpeed);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
