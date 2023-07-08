namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(FilmBarsRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/FilmBars")]
    public class FilmBars : PostProcessEffectSettings
    {
        [Range(0.1f, 5.0f), Tooltip("Desired aspect ratio (16:9 = 1.777 approx).")]
        public FloatParameter aspect = new FloatParameter { value = 1.777f };
    }

    public sealed class FilmBarsRenderer : PostProcessEffectRenderer<FilmBars>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/FilmBars"));
            sheet.properties.SetFloat("_Aspect", settings.aspect);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
