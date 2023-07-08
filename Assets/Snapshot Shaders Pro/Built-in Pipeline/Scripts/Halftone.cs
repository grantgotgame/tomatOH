namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(HalftoneRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Halftone")]
    public sealed class Halftone : PostProcessEffectSettings
    {
        [Tooltip("The texture used to determine the shape of the halftone 'dots'.")]
        public TextureParameter halftoneTexture = new TextureParameter();

        [Tooltip("How soft the transition between light and dark is.")]
        public FloatParameter softness = new FloatParameter { value = 0.5f };

        [Tooltip("Size of the halftone 'dots' on the screen.")]
        public FloatParameter textureSize = new FloatParameter { value = 4.0f };

        [Tooltip("Use this vector to remap the minimum and maximum luminance values used in calculations. Default is (0, 1).")]
        public Vector2Parameter minMaxLuminance = new Vector2Parameter { value = new Vector2(0.0f, 1.0f) };

        [Tooltip("Color to use for dark sections of the image.")]
        public ColorParameter darkColor = new ColorParameter { value = Color.black };

        [Tooltip("Color to use for light sections of the image.")]
        public ColorParameter lightColor = new ColorParameter { value = Color.white };

        [Tooltip("Use the Scene Color instead of Light Color?")]
        public BoolParameter useSceneColor = new BoolParameter { value = false };
    }

    public sealed class HalftoneRenderer : PostProcessEffectRenderer<Halftone>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Halftone"));

            if (settings.useSceneColor)
            {
                sheet.EnableKeyword("USE_SCENE_TEXTURE_ON");
            }
            else
            {
                sheet.DisableKeyword("USE_SCENE_TEXTURE_ON");
            }

            if (settings.halftoneTexture != null)
            {
                sheet.properties.SetTexture("_HalftoneTex", settings.halftoneTexture);
            }
            else
            {
                sheet.properties.SetTexture("_HalftoneTexture", Texture2D.whiteTexture);
            }

            sheet.properties.SetFloat("_Softness", settings.softness);
            sheet.properties.SetFloat("_TextureSize", settings.textureSize);
            sheet.properties.SetVector("_MinMaxLuminance", settings.minMaxLuminance);
            sheet.properties.SetColor("_DarkColor", settings.darkColor);
            sheet.properties.SetColor("_LightColor", settings.lightColor);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
