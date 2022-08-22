// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
Shader "Relic/WaterScroll" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_ScrollX("Scroll X", Range(-5,5)) = 1
		_ScrollY("Scroll Y", Range(-5,5)) = 1
		_Velocity("Velocity", Range(1,10)) = 1

	}
		SubShader{
			Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			LOD 200
		CGPROGRAM
		#pragma surface surf Lambert alpha:fade
		sampler2D _MainTex;
		fixed4 _Color;
		float _ScrollX;
		float _ScrollY;
		float _Velocity;

		float4 _RimColor;
		float _RimPower;
		struct Input {
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutput o) {

			_ScrollX *= _Time;
			_ScrollY *= _Time;

			fixed4 water = (tex2D(_MainTex, IN.uv_MainTex + float2(_ScrollX, _ScrollY)*_Velocity)).rgba* _Color;

			o.Albedo = water;

			//fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			//o.Albedo = c.rgb;
			o.Alpha = water.a;
		}
		ENDCG
	}
		Fallback "Legacy Shaders/Transparent/VertexLit"
}