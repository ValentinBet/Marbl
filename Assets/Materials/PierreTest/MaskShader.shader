Shader "Custom/Marble"
{
	Properties
	{
		[Header(Texture)]
		 _MarbleColor("_MarbleColor", Color) = (0, 1, 1, 1)
		[NoScaleOffset] _Motif("Motif", 2D) = "black" {}
		// _Color1("Second Mix", Color) = (0, 1, 1, 1)

		//[NoScaleOffset] _TexMask("Mask", 2D) = "black" {}

		 [NoScaleOffset] _Metallic("Metallic", 2D) = "black" {}

		_TilingMultiplier("TilingMultiplier", Float) = 1
		_TilingOffset("TilingOffset",Float) = 1
		_TilingMultiplierMask("TilingMultiplierMask",Float) = 1
	}

		SubShader
		{
			Tags
			{
				"RenderType" = "Opaque"
				"LightMode" = "ForwardBase"	
			}
	
		Pass
		{
			Blend One OneMinusSrcAlpha
			Cull Off
			Lighting Off
			ZWrite On
			ZTest LEqual
	
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc" //UnityObjectToWorldNormal
			#include "UnityLightingCommon.cginc" // _LightColor0
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			uniform float4 _MarbleColor;
			uniform sampler2D _Motif;
			//uniform float4 _Color1;
			//uniform sampler2D _TexMask;
			uniform float _TilingMultiplier;
			uniform float _TilingOffset;
			uniform float _TilingMultiplierMask;
	
			/*struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};*/
	
			struct v2f
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				SHADOW_COORDS(1)
				fixed3 diff : COLOR0; //LightingColor
				fixed3 ambient : COLOR1;
			};
	
			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//half4 emission = _MarbleColor;
				o.texcoord = _TilingMultiplier * float2(v.texcoord.x, (1 - v.texcoord.y));
				// o.rgb += emission.rgb;
				
				
				//Lighting
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = nl * _LightColor0.rgb;
				o.ambient = ShadeSH9(half4(worldNormal, 1));
				//Compute Shadow
				TRANSFER_SHADOW(o)
				 return o;
				
			}
	
			float4 frag(v2f IN) : SV_Target
			{
				float4 mask = tex2D(_Motif, IN.texcoord *_TilingMultiplierMask);
	
				fixed shadow = SHADOW_ATTENUATION(IN);
				fixed3 lighting = IN.diff * shadow + IN.ambient;
				//float4 result1 = tex2D(, IN.texcoord + _TilingOffset)*(_Color0);
				float4 result2 = tex2D(_Motif, IN.texcoord + _TilingOffset);
	
				float4 result = result2* _MarbleColor;

				result.rgb *= lighting;
				result.a = 1;

				return result;
			}
			ENDCG
		}
		Pass
		{
			Tags {"LightMode" = "ShadowCaster"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f {
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
}
