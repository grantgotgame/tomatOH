namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(WorldScanRenderer), PostProcessEvent.BeforeStack, "Snapshot Pro/World Scan")]
    public sealed class WorldScan : PostProcessEffectSettings
    {
        [Tooltip("The world space origin point of the scan.")]
        public Vector3Parameter scanOrigin = new Vector3Parameter { value = Vector3.zero };

        [Tooltip("How quickly, in units per second, the scan propogates.")]
        public FloatParameter scanSpeed = new FloatParameter { value = 1.0f };

        [Tooltip("How far, in Unity units, the scan has travelled from the origin.")]
        public FloatParameter scanDistance = new FloatParameter { value = 0.0f };

        [Tooltip("The distance, in Unity units, the scan texture gets applied over.")]
        public FloatParameter scanWidth = new FloatParameter { value = 1.0f };

        [Tooltip("An x-by-1 ramp texture representing the scan color.")]
        public TextureParameter overlayRampTex = new TextureParameter();

        [ColorUsage(true, true)]
        [Tooltip("An additional HDR color tint applied to the scan.")]
        public ColorParameter overlayColor = new ColorParameter { value = Color.white };
    }
    
    public sealed class WorldScanRenderer : PostProcessEffectRenderer<WorldScan>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/WorldScan"));
            
            Matrix4x4 view = context.camera.worldToCameraMatrix;
            Matrix4x4 proj = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, true);

            Matrix4x4 clipToWorld = Matrix4x4.Inverse(proj * view);

            sheet.properties.SetMatrix("_ClipToWorld", clipToWorld);
            sheet.properties.SetVector("_ScanOrigin", settings.scanOrigin);
            sheet.properties.SetFloat("_ScanDistance", settings.scanDistance);
            sheet.properties.SetFloat("_ScanWidth", settings.scanWidth);
            sheet.properties.SetTexture("_OverlayRampTex", settings.overlayRampTex);
            sheet.properties.SetColor("_OverlayColor", settings.overlayColor);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
