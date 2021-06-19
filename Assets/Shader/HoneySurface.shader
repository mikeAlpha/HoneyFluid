Shader "Mikealpha/HoneySurface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("MainTexture", 2D) = "white" {}
        [NoScaleOffset] _FlowTex("FlowTexture", 2D) = "white" {}
        _Range ("DistortRange", Range(0,0.5)) = 0.1
    }
    SubShader
    {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            sampler2D _FlowTex;
            float _Range;
            float _Power;
            fixed4 _Color;
            
            struct Input
            {
                float2 uv_MainTex;
            };
                
            void surf (Input IN, inout SurfaceOutput o)
            {
                float2 fv = tex2D(_FlowTex, IN.uv_MainTex).rg;
                float2 uv = IN.uv_MainTex - fv * _Range;
                fixed4 c = tex2D (_MainTex, uv) * _Color;
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
        ENDCG
     }
    FallBack "Diffuse"
}
