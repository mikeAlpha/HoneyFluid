Shader "Unlit/FluidQuad"
{
    Properties{
     _Color("Color" , Color) = (1,1,1,1)
     _MainTex("Texture", 2D) = "white" {}
     _Edge("Edge Value", Float) = 0.5
     _FresnelColor("Fresnel Color", Color) = (1,1,1,1)
     _FresnelBias("Fresnel Bias", Float) = 0
     _FresnelScale("Fresnel Scale", Float) = 1
     _FresnelPower("Fresnel Power", Float) = 1
    }

        SubShader{
            Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
            LOD 100

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass {
                CGPROGRAM
                    #pragma vertex vert surface surf
                    #pragma fragment frag
                    #pragma target 2.0
                    #pragma multi_compile_fog

                    #include "UnityCG.cginc"

                    struct appdata_t {
                        float4 vertex : POSITION;
                        float2 texcoord : TEXCOORD0;
                        half3 normal : NORMAL;
                        UNITY_VERTEX_INPUT_INSTANCE_ID
                    };

                    struct v2f {
                        float4 vertex : SV_POSITION;
                        float2 texcoord : TEXCOORD0;
                        float fresnel : TEXCOORD1;
                        UNITY_FOG_COORDS(1)
                        UNITY_VERTEX_OUTPUT_STEREO
                    };

                    sampler2D _MainTex;
                    float4 _MainTex_ST;
                    float _Edge;
                    fixed4 _Color;
                    fixed4 _FresnelColor;
                    fixed _FresnelBias;
                    fixed _FresnelScale;
                    fixed _FresnelPower;

                    v2f vert(appdata_t v)
                    {
                        v2f o;
                        UNITY_SETUP_INSTANCE_ID(v);
                        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                        //v.vertex.y += _Time.y;

                        o.vertex = UnityObjectToClipPos(v.vertex);
                        o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                        UNITY_TRANSFER_FOG(o,o.vertex);
                        
                        float3 i = normalize(ObjSpaceViewDir(v.vertex));
                        o.fresnel = _FresnelBias + _FresnelScale * pow(1 + dot(i, v.normal), _FresnelPower);

                        return o;
                    }

                    fixed4 frag(v2f i) : SV_Target
                    {
                        fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;
                        
                        //col.a = alpha;
                        
                        //fixed4 final = col;

                        //col.a = alpha;

                        UNITY_APPLY_FOG(i.fogCoord, col);
                        return lerp(col, _FresnelColor, 1 - i.fresnel);
                    }
                ENDCG
            }
    }
}
