Shader "Relic/Water" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "black" {}
		_ScrollX("Scroll X", Range(-5,5)) = 1
		_ScrollY("Scroll Y", Range(-5,5)) = 1
		_Velocity("Velocity", Range(1,10))=1
	}

		SubShader{
			Blend One One 

			CGPROGRAM
			#define BLINK
			#pragma surface surf Lambert  

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		
		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		float _ScrollX;
		float _ScrollY;
		float _Velocity;

		void surf( Input IN, inout SurfaceOutput o )
		{
			_ScrollX *= _Time;
			_ScrollY *= _Time;

			float3 water = (tex2D(_MainTex, IN.uv_MainTex + float2(_ScrollX, _ScrollY)*_Velocity)).rgb* _Color;
			
			o.Albedo = water;
		}


		ENDCG
	}
		
}