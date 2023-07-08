namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(MosaicRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Mosaic")]
    public class Mosaic : PostProcessEffectSettings
    {
        [Tooltip("Texture to overlay onto each mosaic tile.")]
        public TextureParameter overlayTexture = new TextureParameter();

        [Tooltip("Colour of texture overlay.")]
        public ColorParameter overlayColor = new ColorParameter { value = Color.white };

        [Range(5, 500), Tooltip("Number of tiles on the x-axis.")]
        public IntParameter xTileCount = new IntParameter { value = 100 };

        [Tooltip("Use sharper point filtering when downsampling?")]
        public BoolParameter usePointFiltering = new BoolParameter { value = true };
    }

    public sealed class MosaicRenderer : PostProcessEffectRenderer<Mosaic>
    {
        public override void Render(PostProcessRenderContext context)
        {
            int xTileCount = settings.xTileCount;
            int yTileCount = Mathf.RoundToInt((float)Screen.height / Screen.width * xTileCount);

            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Mosaic"));
            if (settings.overlayTexture.value != null)
            {
                sheet.properties.SetTexture("_OverlayTex", settings.overlayTexture);
            }
            else
            {
                sheet.properties.SetTexture("_OverlayTex", Texture2D.whiteTexture);
            }
            sheet.properties.SetColor("_OverlayColor", settings.overlayColor);
            sheet.properties.SetInt("_XTileCount", xTileCount);
            sheet.properties.SetInt("_YTileCount", yTileCount);

            var baseSheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Base"));

            var tmp = RenderTexture.GetTemporary(xTileCount, yTileCount, 0);

            tmp.filterMode = (settings.usePointFiltering) ? FilterMode.Point : FilterMode.Bilinear;

            context.command.BlitFullscreenTriangle(context.source, tmp, baseSheet, 0);
            context.command.BlitFullscreenTriangle(tmp, context.destination, sheet, 0);

            RenderTexture.ReleaseTemporary(tmp);
        }
    }
}
