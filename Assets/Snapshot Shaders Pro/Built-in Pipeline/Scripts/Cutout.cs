namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(CutoutRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Cutout")]
    public sealed class Cutout : PostProcessEffectSettings
    {
        [Tooltip("The texture to use for the cutout.")]
        public TextureParameter cutoutTexture = new TextureParameter();

        [Tooltip("The colour of the area outside the cutout.")]
        public ColorParameter borderColor = new ColorParameter { value = Color.white };

        [Tooltip("Should the cutout texture stretch to fit the screen's aspect ratio?")]
        public BoolParameter stretch = new BoolParameter { value = false };

        [Range(0.01f, 10.0f), Tooltip("How zoomed-in the texture is. 1 = unzoomed.")]
        public FloatParameter zoom = new FloatParameter { value = 1.0f };

        [Tooltip("How offset the texture is from the centre of the screen (in UV space).")]
        public Vector2Parameter offset = new Vector2Parameter { value = new Vector2() };

        [Range(0.0f, 360.0f), Tooltip("How much the texture is rotated (anticlockwise, in degrees).")]
        public FloatParameter rotation = new FloatParameter { value = 0.0f };
    }

    public sealed class CutoutRenderer : PostProcessEffectRenderer<Cutout>
    {
        public override void Render(PostProcessRenderContext context)
        {
            Matrix4x4 rotationMatrix = Matrix4x4.identity;
            rotationMatrix[0, 0] = rotationMatrix[1, 1] = Mathf.Cos(settings.rotation * Mathf.Deg2Rad);
            rotationMatrix[0, 1] = Mathf.Sin(settings.rotation * Mathf.Deg2Rad);
            rotationMatrix[1, 0] = -rotationMatrix[0, 1];

            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Cutout"));
            sheet.properties.SetTexture("_CutoutTex", settings.cutoutTexture);
            sheet.properties.SetColor("_BorderColor", settings.borderColor);
            sheet.properties.SetInt("_Stretch", settings.stretch ? 1 : 0);
            sheet.properties.SetFloat("_Zoom", settings.zoom);
            sheet.properties.SetVector("_Offset", settings.offset);
            sheet.properties.SetMatrix("_Rotation", rotationMatrix);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
