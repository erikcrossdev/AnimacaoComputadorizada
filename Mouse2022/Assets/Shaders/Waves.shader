Shader "Unlit/Waves"
{
	Properties
	{
		_MainTex("Diffuse", 2D) = "white" {}
		_Tint("Color Tint", Color)= (1,1,1,1)
		_Freq("Frequency", Range(0,5))= 3 //cycles
		_Speed("Speed", Range(0,100))=10
		_Amp("Amplitude", Range(0,1))=0.5 //wave height
	}
		SubShader
	{
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert

		struct Input {
			float2 uv_MainTex;
			float3 vertColor;
		};

		float4 _Tint;
		float _Freq;
		float _Speed;
		float _Amp;

		struct appdata {
			float4 vertex: POSITION;
			float3 normal: NORMAL;
			float4 texcoord: TEXCOORD0;
			float4 texcoord1: TEXCOORD1;
			float4 texcoord2: TEXCOORD2;
		};

		void vert(inout appdata v, out Input o) { //modify vertex colors
			UNITY_INITIALIZE_OUTPUT(Input,o); 
			float t = _Time * _Speed;//time of the wave, _Time is similar to delta time
			float waveHeight = sin(t + v.vertex.x * _Freq) *_Amp+ 
						   sin(t*2 + v.vertex.x * _Freq*2 ) *_Amp; //the  height of the wave using amplitude, frquency and X position of the vertex
			v.vertex.y = v.vertex.y + waveHeight;//the wave y plus de waveHeight
			v.normal = normalize(float3(v.normal.x + waveHeight, v.normal.y, v.normal.z)); //Update de normals to reflect the normals 
			o.vertColor = waveHeight + 2; //change the color based on the height 
		}

		sampler2D _MainTex;
		void surf(Input IN, inout SurfaceOutput o) {
			float4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c * IN.vertColor.rgb;//the texture * the vertex color on the vert function
		}
		ENDCG
	}	
}
