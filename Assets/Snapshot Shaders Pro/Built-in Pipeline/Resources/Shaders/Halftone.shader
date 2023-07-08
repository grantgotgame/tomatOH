Shader "Hidden/SnapshotPro/Halftone"
{
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment Frag
			#pragma multi_compile __ USE_SCENE_TEXTURE_ON

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			TEXTURE2D_SAMPLER2D(_HalftoneTex, sampler_HalftoneTex);
			float _Softness;
			float _TextureSize;
			float2 _MinMaxLuminance;

			float4 _DarkColor;
			float4 _LightColor;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float2 halftoneUVs = i.texcoord * _ScreenParams.xy / _TextureSize;

				float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
				float lum = saturate(dot(col.rgb, float3(0.3f, 0.59f, 0.11f)));

				float halftone = SAMPLE_TEXTURE2D(_HalftoneTex, sampler_HalftoneTex, halftoneUVs);
				halftone = lerp(_MinMaxLuminance.x, _MinMaxLuminance.y, halftone);

				float halftoneSmooth = fwidth(halftone) * _Softness;
				halftoneSmooth = smoothstep(halftone - halftoneSmooth, halftone + halftoneSmooth, lum);

#ifdef USE_SCENE_TEXTURE_ON
				col = lerp(_DarkColor, col, halftoneSmooth);
#else
				col = lerp(_DarkColor, _LightColor, halftoneSmooth);
#endif
				return col;
			}

			ENDHLSL
		}
	}
}
