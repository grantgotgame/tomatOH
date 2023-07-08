namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(UnderwaterRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Underwater")]
    public class Underwater : PostProcessEffectSettings
    {
        [Tooltip("Displacement texture for surface waves.")]
        public TextureParameter bumpMap = new TextureParameter();

        [Range(0.0f, 0.5f), Tooltip("Strength/size of the waves.")]
        public FloatParameter strength = new FloatParameter { value = 0.01f };

        [Tooltip("Tint of the underwater fog.")]
        public ColorParameter waterColor = new ColorParameter { value = Color.white };

        [Range(0.0f, 1.0f), Tooltip("Strength of the underwater fog.")]
        public FloatParameter fogStrength = new FloatParameter { value = 0.1f };
    }

    public sealed class UnderwaterRenderer : PostProcessEffectRenderer<Underwater>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Underwater"));
            if (settings.bumpMap.value != null)
            {
                sheet.properties.SetTexture("_BumpMap", settings.bumpMap);
            }
            else
            {
                sheet.properties.SetTexture("_BumpMap", Texture2D.normalTexture);
            }
            sheet.properties.SetFloat("_Strength", settings.strength);
            sheet.properties.SetColor("_WaterColor", settings.waterColor);
            sheet.properties.SetFloat("_FogStrength", settings.fogStrength);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
