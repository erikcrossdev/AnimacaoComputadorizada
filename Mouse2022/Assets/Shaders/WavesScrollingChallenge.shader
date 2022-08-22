Shader "Unlit/WavesScrollingChallenge"
{
	Properties
	{
		_MainTex("Diffuse", 2D) = "white" {}
		_Tint("Color Tint", Color)= (1,1,1,1)
		_Freq("Frequency", Range(0,5))= 3 //cycles
		_Speed("Speed", Range(0,100))=10
		_Amp("Amplitude", Range(0,1))=0.5 //wave height

		_FoamTex("Foam", 2D) = "white" {}
		_ScrollX("Scroll X", Range(-5,5)) = 1
		_ScrollY("Scroll Y", Range(-5,5)) = 1
	}
		SubShader
	{
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert

		float _ScrollX;
		float _ScrollY;


		sampler2D _FoamTex;//With lambert, specular wont work
		sampler2D _MainTex;//With lambert, specular wont work
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

		void surf(Input IN, inout SurfaceOutput o) {

			_ScrollX *= _Time;
			_ScrollY *= _Time;

			float3 water = (tex2D(_MainTex, IN.uv_MainTex + float2(_ScrollX, _ScrollY))).rgb;
			float3 foam = (tex2D(_FoamTex, IN.uv_MainTex + float2(_ScrollX / 2.0, _ScrollY / 2.0))).rgb;


			//o.Albedo = (water + foam) / 2.0;

			float3 c = (water + foam) / 2.0;
			o.Albedo = c * IN.vertColor.rgb;//the texture * the vertex color on the vert function
		}
		ENDCG
	}	
}
