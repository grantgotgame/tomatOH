Shader "Hidden/SnapshotPro/TextAdventure"
{
	HLSLINCLUDE
	#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
	#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/Colors.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	TEXTURE2D_SAMPLER2D(_CharacterAtlas, sampler_CharacterAtlas);
	float2 _MainTex_TexelSize;
	int _CharacterCount;
	float2 _CharacterSize;
	float4 _BackgroundColor;
	float4 _CharacterColor;

	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Name "TextPass"
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment FragHorizontal

			float4 FragHorizontal(VaryingsDefault i) : SV_Target
			{
				float3 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).rgb;

				// Luminance() expects linear color, so I think Linear->SRGB is 'wrong' here,
				// but it ends up giving nicer results anyway!
				float luminance = saturate(Luminance(LinearToSRGB(col)));
				luminance = saturate(luminance - EPSILON);

				float2 uv = (i.texcoord * _CharacterSize) % 1.0f;
				uv.x = (floor(luminance * _CharacterCount) + uv.x) / _CharacterCount;

				float characterMask = SAMPLE_TEXTURE2D(_CharacterAtlas, sampler_CharacterAtlas, uv).r;
				return lerp(_BackgroundColor, _CharacterColor, characterMask);
			}

			ENDHLSL
		}
	}
}
