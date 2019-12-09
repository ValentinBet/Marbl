Shader "Custom/Transparent"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Alpha("Alpha", Range(0, 1)) = 1
	}
		SubShader
		{
			Tags
			{
				"RenderType" = "Transparent"
				"IgnoreProjector" = "True"
				"Queue" = "Transparent"
			}

			LOD 200

			Pass { ColorMask 0 }
			ZWrite Off
			Alphatest Greater 0
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB

			CGPROGRAM

			#pragma surface surf Standard addshadow novertexlights noforwardadd alpha:fade

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			struct Input
			{
				float2 uv_MainTex;
				float3 worldPos;
				float4 color : COLOR;
			};

			fixed4 _Color;
			sampler2D _MainTex;
			half _Glossiness;
			float _Alpha;

			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color * _Color;
				o.Albedo = c.rgb;
				o.Smoothness = _Glossiness;

				// Fade alpha depending on camera position
				float dist = distance(IN.worldPos, _WorldSpaceCameraPos);
				c.a = _Alpha;

				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
