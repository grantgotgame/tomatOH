namespace SnapshotShaders.BuiltIn
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [Serializable]
    [PostProcess(typeof(TextAdventureRenderer), PostProcessEvent.BeforeStack, "Snapshot Pro/Text Adventure")]
    public class TextAdventure : PostProcessEffectSettings
    {
        [Range(8, 64), Tooltip("The on-screen size of each character, in pixels.")]
        public IntParameter characterSize = new IntParameter { value = 16 };

        [Tooltip("A texture containing the characters that will replace the image.\n" + 
            "An (nx)-by-y texture, where there are n characters, each of which is x-by-y pixels.")]
        public TextureParameter characterAtlas = new TextureParameter();

        [Tooltip("How many characters are contained in the Character Atlas.")]
        public IntParameter characterCount = new IntParameter { value = 16 };

        [Tooltip("The color of the background.")]
        public ColorParameter backgroundColor = new ColorParameter { value = Color.black };

        [ColorUsage(true, true)]
        [Tooltip("The color of the characters superimposed onto the background.")]
        public ColorParameter characterColor = new ColorParameter { value = Color.green };
    }

    public sealed class TextAdventureRenderer : PostProcessEffectRenderer<TextAdventure>
    {
        public override void Render(PostProcessRenderContext context)
        {
            float size = settings.characterSize;
            float aspect = (float)Screen.height / Screen.width;
            Vector2Int pixelSize = new Vector2Int(Mathf.CeilToInt((Screen.width) / size),
                Mathf.CeilToInt(Screen.height / size));

            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/TextAdventure"));
            sheet.properties.SetTexture("_CharacterAtlas", settings.characterAtlas);
            sheet.properties.SetInteger("_CharacterCount", settings.characterCount);
            sheet.properties.SetVector("_CharacterSize", (Vector2)pixelSize);
            sheet.properties.SetColor("_BackgroundColor", settings.backgroundColor);
            sheet.properties.SetColor("_CharacterColor", settings.characterColor);

            var downsampled = RenderTexture.GetTemporary(pixelSize.x, pixelSize.y, 0);
            downsampled.filterMode = FilterMode.Point;

            context.command.BlitFullscreenTriangle(context.source, downsampled);
            context.command.BlitFullscreenTriangle(downsampled, context.destination, sheet, 0);

            RenderTexture.ReleaseTemporary(downsampled);
        }
    }
}
