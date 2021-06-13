Shader "Mikealpha/MyShader"{

	Properties{
		_myColour("Color" , Color) = (1,1,1,1)
		_Range("Range", Range(0,5)) = 1
		_Speed("Speed", Range(0,1)) = 0.1
		_BumpAmount("BumpAmount", Range(0,10)) = 1
		_Glossiness("Glossiness", Range(0,10)) = 0.5
		_Metallic("Metallic", Range(0,10)) = 0
		_Texture("MainTexture" , 2D) = "white"{}
	    _Bump("BumpMap" , 2D) = "bump"{}
		_Cube("Cubemap" , CUBE) = ""{}
		_Float("Float" , Float) = 0.5
		_Vector("Vector" , Vector) = (0.5 , 1 , 1 , 1)
	}

	SubShader{

		CGPROGRAM
           #pragma surface surf Standard fullforwardshadows vertex:vert

		   struct Input {
				float2 uv_Texture;
				float2 uv_Bump;
				float3 worldRefl;
           };
	       
	       fixed4 _myColour;
		   half _Range;
		   half _BumpAmount;
		   half _Metallic;
		   half _Glossiness;
		   half _Speed;
		   sampler2D _Texture;
		   sampler2D _Bump;
		   samplerCUBE _Cube;
		   float _Float;
		   float4 _Vector;

		   void vert(inout appdata_full v, out Input o) {


			   v.vertex.y += sin(_Time * 30 + v.vertex.x * 2) * 0.7;

			   UNITY_INITIALIZE_OUTPUT(Input, o);

		   }
		   
		   void surf(Input IN, inout SurfaceOutputStandard o) {
			   float2 uv = IN.uv_Texture + _Time.y * _Speed;
			   float2 uvbump = IN.uv_Bump + _Time.y * _Speed;
			   fixed4 c = tex2D(_Texture, uv) * _myColour;
			   o.Albedo = c.rgb;
			   o.Metallic = _Metallic;
			   o.Smoothness = _Glossiness;
			   o.Alpha = c.a;
			   //o.Emission = texCUBE(_Cube, IN.worldRefl).rgb;
			   o.Normal = UnpackNormal(tex2D(_Bump, uvbump));
			   o.Normal *= float3(_BumpAmount, _BumpAmount, 1);
		   }		
		ENDCG
	}

	Fallback "Diffuse"
}
