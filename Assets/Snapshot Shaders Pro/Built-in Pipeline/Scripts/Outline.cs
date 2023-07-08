namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(OutlineRenderer), PostProcessEvent.BeforeStack, "Snapshot Pro/Outline")]
    public class Outline : PostProcessEffectSettings
    {
        [ColorUsage(true, true)]
        [Tooltip("Color of the outlines.")]
        public ColorParameter outlineColor = new ColorParameter { value = Color.white };

        [Range(0.0f, 1.0f), Tooltip("Threshold for colour-based edge detection.")]
        public FloatParameter colorSensitivity = new FloatParameter { value = 0.1f };

        [Range(0.0f, 1.0f), Tooltip("Strength of colour-based edges.")]
        public FloatParameter colorStrength = new FloatParameter { value = 1.0f };

        [Range(0.0f, 1.0f), Tooltip("Threshold for depth-based edge detection.")]
        public FloatParameter depthSensitivity = new FloatParameter { value = 0.01f };

        [Range(0.0f, 1.0f), Tooltip("Strength of depth-based edges.")]
        public FloatParameter depthStrength = new FloatParameter { value = 1.0f };

        [Range(0.0f, 1.0f), Tooltip("Threshold for normal-based edge detection.")]
        public FloatParameter normalSensitivity = new FloatParameter { value = 0.1f };

        [Range(0.0f, 1.0f), Tooltip("Strength of normal-based edges.")]
        public FloatParameter normalStrength = new FloatParameter { value = 1.0f };

        [Range(0.0f, 1.0f), Tooltip("Pixels past this depth threshold will not be edge-detected.")]
        public FloatParameter depthThreshold = new FloatParameter { value = 0.99f };

        [Tooltip("Color of the background if Use Scene Color is turned off.")]
        public ColorParameter backgroundColor = new ColorParameter { value = Color.black };

        [Tooltip("Use the Scene Color instead of Background Color?")]
        public BoolParameter useSceneColor = new BoolParameter { value = false };
    }

    public sealed class OutlineRenderer : PostProcessEffectRenderer<Outline>
    {
        public override void Render(PostProcessRenderContext context)
        {
            context.camera.depthTextureMode = DepthTextureMode.DepthNormals;
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Outline"));

            if (settings.useSceneColor.value)
            {
                sheet.EnableKeyword("USE_SCENE_TEXTURE_ON");
            }
            else
            {
                sheet.DisableKeyword("USE_SCENE_TEXTURE_ON");
                sheet.properties.SetColor("_BackgroundColor", settings.backgroundColor);
            }
            
            sheet.properties.SetColor("_OutlineColor", settings.outlineColor);
            sheet.properties.SetFloat("_ColorSensitivity", settings.colorSensitivity);
            sheet.properties.SetFloat("_ColorStrength", settings.colorStrength);
            sheet.properties.SetFloat("_DepthSensitivity", settings.depthSensitivity);
            sheet.properties.SetFloat("_DepthStrength", settings.depthStrength);
            sheet.properties.SetFloat("_NormalsSensitivity", settings.normalSensitivity);
            sheet.properties.SetFloat("_NormalsStrength", settings.normalStrength);
            sheet.properties.SetFloat("_DepthThreshold", settings.depthThreshold);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
