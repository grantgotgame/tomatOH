namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(DrawingRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Drawing")]
    public sealed class Drawing : PostProcessEffectSettings
    {
        [Tooltip("Drawing overlay texture.")]
        public TextureParameter drawingTex = new TextureParameter();

        [Range(0.0f, 5.0f), Tooltip("Time taken (in seconds) per animation cycle. Set to zero for no animation.")]
        public FloatParameter animCycleTime = new FloatParameter { value = 1.0f };

        [Range(0.0f, 1.0f), Tooltip("Strength of the effect.")]
        public FloatParameter strength = new FloatParameter { value = 0.5f };

        [Range(1.0f, 50.0f), Tooltip("Number of times the drawing texture is tiled.")]
        public FloatParameter tiling = new FloatParameter { value = 25.0f };

        [Range(0.0f, 5.0f), Tooltip("Amount of UV smudging based on drawing texture colour values.")]
        public FloatParameter smudge = new FloatParameter { value = 0.001f };

        [Range(0.0f, 1.01f), Tooltip("Pixels past this depth threshold will not be 'drawn on'.")]
        public FloatParameter depthThreshold = new FloatParameter { value = 0.99f };
    }

    public sealed class DrawingRenderer : PostProcessEffectRenderer<Drawing>
    {
        public override void Render(PostProcessRenderContext context)
        {
            bool isOffset = false;

            if (settings.animCycleTime > 0.0f)
            {
                isOffset = (Time.time % settings.animCycleTime) < (settings.animCycleTime / 2.0f);
            }

            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Drawing"));

            if (settings.drawingTex != null)
            {
                sheet.properties.SetTexture("_DrawingTex", settings.drawingTex);
            }
            else
            {
                sheet.properties.SetTexture("_DrawingTex", Texture2D.whiteTexture);
            }

            sheet.properties.SetFloat("_OverlayOffset", isOffset ? 0.5f : 0.0f);
            sheet.properties.SetFloat("_Strength", settings.strength);
            sheet.properties.SetFloat("_Tiling", settings.tiling);
            sheet.properties.SetFloat("_Smudge", settings.smudge);
            sheet.properties.SetFloat("_DepthThreshold", settings.depthThreshold);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
