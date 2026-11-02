Shader "Unlit/Shader2_waktu"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1, 1, 1, 1)

        _WarnaA ("Warna A", Color) = (0, 0.5, 1, 1)
        _WarnaB ("Warna B", Color) = (1, 0, 0, 1)
        _Kecepatan ("Kecepatan Perubahan", Float) = 1
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
                float4 _WarnaA;
                float4 _WarnaB;
                float _Kecepatan;
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

                keluar.posisiClip =
                    TransformObjectToHClip(masuk.posisiObjek.xyz);

                keluar.uv =
                    TRANSFORM_TEX(masuk.uv, _MainTex);

                keluar.warnaVertex =
                    masuk.warnaVertex;

                return keluar;
            }

            half4 Frag(KeFragment masuk) : SV_Target
            {
                half4 teks =
                    SAMPLE_TEXTURE2D(
                        _MainTex,
                        sampler_MainTex,
                        masuk.uv
                    );

                float nilai =
                    (sin(_Time.y * _Kecepatan) + 1.0) * 0.5;

                half4 warnaBerubah =
                    lerp(_WarnaA, _WarnaB, nilai);

                return teks
                    * (half4)_Tint
                    * warnaBerubah
                    * masuk.warnaVertex;
            }

            ENDHLSL
        }
    }
}
