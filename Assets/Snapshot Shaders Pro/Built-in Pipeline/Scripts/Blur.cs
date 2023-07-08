namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(BlurRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Blur")]
    public class Blur : PostProcessEffectSettings
    {
        [Range(3, 27), Tooltip("Blur Strength")]
        public IntParameter strength = new IntParameter { value = 5 };
    }

    public sealed class BlurRenderer : PostProcessEffectRenderer<Blur>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Blur"));
            sheet.properties.SetInt("_KernelSize", settings.strength);
            sheet.properties.SetFloat("_Spread", settings.strength / 7.5f);

            var tmp = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);

            context.command.BlitFullscreenTriangle(context.source, tmp, sheet, 0);
            context.command.BlitFullscreenTriangle(tmp, context.destination, sheet, 1);

            RenderTexture.ReleaseTemporary(tmp);
        }
    }
}
