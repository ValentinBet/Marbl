Shader "Custom/Marble"
{
	Properties
	{
		[Header(Texture)]
		 _MarbleColor("_MarbleColor", Color) = (0, 1, 1, 1)
		[NoScaleOffset] _Motif("Motif", 2D) = "black" {}
		// _Color1("Second Mix", Color) = (0, 1, 1, 1)

		//[NoScaleOffset] _TexMask("Mask", 2D) = "black" {}


		_TilingMultiplier("TilingMultiplier", Float) = 1
		_TilingOffset("TilingOffset",Float) = 1
		_TilingMultiplierMask("TilingMultiplierMask",Float) = 1
	}

		SubShader
		{
			Tags
			{
				"RenderType" = "Transparent"
				"Queue" = "Transparent+0"

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

				uniform float4 _MarbleColor;
				uniform sampler2D _Motif;
				//uniform float4 _Color1;
				//uniform sampler2D _TexMask;
				uniform float _TilingMultiplier;
				uniform float _TilingOffset;
				uniform float _TilingMultiplierMask;

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : POSITION;

					float2 texoord : TEX0;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);

					o.texoord = _TilingMultiplier * float2(v.uv.x,(1 - v.uv.y));
					return o;
				}

				float4 frag(v2f IN) : COLOR
				{
					float4 mask = tex2D(_Motif, IN.texoord *_TilingMultiplierMask);

					//float4 result1 = tex2D(, IN.texoord + _TilingOffset)*(_Color0);
					float4 result2 = tex2D(_Motif, IN.texoord + _TilingOffset);

					float4 result = result2* _MarbleColor;

					return result;
				}
				ENDCG
			}
		}
}
