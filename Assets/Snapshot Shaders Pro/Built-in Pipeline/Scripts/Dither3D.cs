namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(Dither3DRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Dither3D")]
    public sealed class Dither3D : PostProcessEffectSettings
    {
        [Tooltip("Noise texture to use for dither thresholding.")]
        public TextureParameter noiseTex = new TextureParameter();

        [Range(0.1f, 100.0f), Tooltip("Size of the noise texture.")]
        public FloatParameter noiseSize = new FloatParameter { value = 1.0f };

        [Range(-0.5f, 0.5f), Tooltip("Offset used when calculating luminance threshold.")]
        public FloatParameter thresholdOffset = new FloatParameter { value = 0.0f };

        [Range(0.0f, 1.0f), Tooltip("Amount of blending between the three cardinal directions.")]
        public FloatParameter blendAmount = new FloatParameter { value = 1.0f };

        [Tooltip("Color to use for dark sections of the image.")]
        public ColorParameter darkColor = new ColorParameter { value = Color.black };

        [Tooltip("Color to use for light sections of the image.")]
        public ColorParameter lightColor = new ColorParameter { value = Color.white };
    }

    public sealed class Dither3DRenderer : PostProcessEffectRenderer<Dither3D>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Dither3D"));

            var cam = context.camera;
            cam.depthTextureMode = DepthTextureMode.DepthNormals;

            var p = GL.GetGPUProjectionMatrix(cam.projectionMatrix, false);
            p[2, 3] = p[3, 2] = 0.0f;
            p[3, 3] = 1.0f;
            var clipToWorld = Matrix4x4.Inverse(p * cam.worldToCameraMatrix) * Matrix4x4.TRS(new Vector3(0, 0, -p[2, 2]), Quaternion.identity, Vector3.one);
            sheet.properties.SetMatrix("clipToWorld", clipToWorld);

            if (settings.noiseTex != null)
            {
                sheet.properties.SetTexture("_NoiseTex", settings.noiseTex);
            }
            else
            {
                sheet.properties.SetTexture("_NoiseTex", Texture2D.whiteTexture);
            }

            sheet.properties.SetFloat("_NoiseSize", settings.noiseSize);
            sheet.properties.SetFloat("_ThresholdOffset", settings.thresholdOffset);
            sheet.properties.SetFloat("_Blend", settings.blendAmount);
            sheet.properties.SetColor("_DarkColor", settings.darkColor);
            sheet.properties.SetColor("_LightColor", settings.lightColor);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
