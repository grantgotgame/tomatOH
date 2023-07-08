namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(NoiseGrainRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Noise Grain")]
    public sealed class NoiseGrain : PostProcessEffectSettings
    {
        [Tooltip("How strongly the screen colors get lightened by noise.")]
        public FloatParameter strength = new FloatParameter { value = 0.1f };

        [Tooltip("How fast the noise grain changes values.")]
        public FloatParameter speed = new FloatParameter { value = 1.0f };

        [Tooltip("The size of the noise texture that gets applied to the screen.")]
        public FloatParameter noiseSize = new FloatParameter { value = 1.0f };

        [Tooltip("Hermite interpolation is faster, while Quintic interpolation will look very slightly nicer.")]
        public NoiseInterpParameter noiseInterpolation = new NoiseInterpParameter { value = NoiseInterpolation.Quintic };
    }

    public sealed class NoiseGrainRenderer : PostProcessEffectRenderer<NoiseGrain>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/NoiseGrain"));
            
            sheet.properties.SetFloat("_Strength", settings.strength);
            sheet.properties.SetFloat("_Speed", settings.speed);
            sheet.properties.SetFloat("_NoiseSize", settings.noiseSize);
            sheet.properties.SetFloat("_AspectRatio", Screen.width / Screen.height);
            
            if(settings.noiseInterpolation == NoiseInterpolation.Quintic)
            {
                sheet.EnableKeyword("USE_QUINTIC_INTERP");
            }
            else
            {
                sheet.DisableKeyword("USE_QUINTIC_INTERP");
            }

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }

    [Serializable]
    public enum NoiseInterpolation
    {
        Hermite, Quintic
    }

    [Serializable]
    public sealed class NoiseInterpParameter : ParameterOverride<NoiseInterpolation> { }
}
