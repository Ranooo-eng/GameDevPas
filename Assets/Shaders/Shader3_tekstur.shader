Shader "Unlit/Shader3_tekstur"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Name "Unlit2D"
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Tint;
            CBUFFER_END

            struct Atribut
            {
                float4 posisiObjek : POSITION;
                float2 uv : TEXCOORD0;
                float4 warnaVertex : COLOR;
            };

            struct KeFragment
            {
                float4 posisiClip : SV_POSITION;
                float2 uv : TEXCOORD0;
                half4 warnaVertex : COLOR;
            };

            KeFragment Vert(Atribut masuk)
            {
                KeFragment keluar;
                keluar.posisiClip = TransformObjectToHClip(masuk.posisiObjek.xyz);
                keluar.uv = TRANSFORM_TEX(masuk.uv, _MainTex);
                keluar.warnaVertex = masuk.warnaVertex;
                return keluar;
            }

            half4 Frag(KeFragment masuk) : SV_Target
            {
                half4 teks = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, masuk.uv);
                // Tekstur * tint material * warna Sprite Renderer.
                return teks * (half4)_Tint * masuk.warnaVertex;
            }
            ENDHLSL
        }
    }

}
