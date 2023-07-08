namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(OilPaintingRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Oil Painting")]
    public sealed class OilPainting : PostProcessEffectSettings
    {
        [Range(1, 17), Tooltip("Oil Painting effect radius.")]
        public IntParameter kernelSize = new IntParameter { value = 5 };
    }

    public sealed class OilPaintingRenderer : PostProcessEffectRenderer<OilPainting>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/OilPainting"));
            sheet.properties.SetInt("_KernelSize", settings.kernelSize);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
