namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(VortexRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Vortex")]
    public sealed class Vortex : PostProcessEffectSettings
    {
        [Tooltip("The vortex will swirl around this normalized position.")]
        public Vector2Parameter center = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };

        [Range(0.0f, 100.0f), Tooltip("How strongly the effect will twirl pixels around the center.")]
        public FloatParameter strength = new FloatParameter { value = 0.0f };

        [Tooltip("How far the image is offset before twirling.")]
        public Vector2Parameter offset = new Vector2Parameter { value = Vector2.zero };
    }

    public sealed class VortexRenderer : PostProcessEffectRenderer<Vortex>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Vortex"));

            sheet.properties.SetVector("_Center", settings.center);
            sheet.properties.SetFloat("_Strength", settings.strength);
            sheet.properties.SetVector("_Offset", settings.offset);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
