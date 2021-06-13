
Shader "MyShader/WaterFlow"
{
	Properties{
		_MainTex("MainTex",2D) = "white"{}//store stickers
		 _Color("Color Tint",Color) = (1,1,1,1)//Control the overall color
		 _Magnitude("Magnitude",Float) = 0.1//Control the fluctuation frequency
		 _Frequency("Frequency",Float) = 0.5//Control the fluctuation amplitude, refer to the frequency amplitude of the sine wave to understand
		 _Speed("Speed", Float) = 0.01//Control the flow speed
	}

		SubShader{
			 //Specify the transparency mixing rendering queue, the Shader is not affected by the projector, the Shader should use transparency mixing, and the vertex animation cannot be batch
			Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "DisableBatching" = "True"}

			Pass{
			 //Specify the forward rendering mode
			Tags{"LightMode" = "ForwardBase"}

			 ZWrite Off//Close deep reading and writing
			 Blend SrcAlpha OneMinusSrcAlpha//Turn on blending mode
			 Cull Off//Close the rejection function

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			 //Define the variables in Properties
			sampler2D _MainTex;
			 float4 _MainTex_ST;//The scaling and offset value of the texture, TRANSFORM_TEX will call
			fixed4 _Color;
			float _Magnitude;
			float _Frequency;
			float _Speed;

			struct a2v {
				float4 vertex:POSITION;
				float2 texcoord:TEXCOORD0;
			};

			struct v2f {
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
			};

			v2f vert(a2v v) {
				v2f o;
				 float4 offset = float4(0, 0, 0, 0);//vertex offset
				 offset.y = sin(_Frequency * _Time.y + v.vertex.x + v.vertex.y + v.vertex.z) * _Magnitude;//The top line Y coordinate is offset with time
				 o.pos = UnityObjectToClipPos(v.vertex + offset);//Vertex from model space to clipping space
				 o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);//Transfer UV coordinates
				 o.uv += float2(0, _Time.y * _Speed);//Texture animation, movement in the horizontal direction
				return o;
			}

			fixed4 frag(v2f i) :SV_Target{
				 fixed4 c = tex2D(_MainTex,i.uv);//Texture sampling according to UV coordinates
				c.rgb *= _Color.rgb;
				return c;
			}

			ENDCG
			 }
		 }
			 FallBack "Transparent/VertexLit"
}