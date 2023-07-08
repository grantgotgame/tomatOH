namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(GameBoyRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Game Boy")]
    public class GameBoy : PostProcessEffectSettings
    {
        [Tooltip("Darkest colour.")]
        public ColorParameter darkest = new ColorParameter { value = new Color(0.11f, 0.21f, 0.08f) };

        [Tooltip("Second darkest colour.")]
        public ColorParameter dark = new ColorParameter { value = new Color(0.24f, 0.38f, 0.21f) };

        [Tooltip("Second lightest colour.")]
        public ColorParameter light = new ColorParameter { value = new Color(0.57f, 0.67f, 0.21f) };

        [Tooltip("Lightest colour.")]
        public ColorParameter lightest = new ColorParameter { value = new Color(0.75f, 0.82f, 0.46f) };

        [Range(0.0f, 4.0f), Tooltip("Modify the input colors via a power ramp. 1 = original mapping, " +
            "higher = favors darker output, lower = favors lighter output.")]
        public FloatParameter powerRamp = new FloatParameter { value = 1.0f };
    }

    public sealed class GameBoyRenderer : PostProcessEffectRenderer<GameBoy>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/GameBoy"));
            sheet.properties.SetColor("_GBDarkest", settings.darkest);
            sheet.properties.SetColor("_GBDark", settings.dark);
            sheet.properties.SetColor("_GBLight", settings.light);
            sheet.properties.SetColor("_GBLightest", settings.lightest);
            sheet.properties.SetFloat("_PowerRamp", settings.powerRamp);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
