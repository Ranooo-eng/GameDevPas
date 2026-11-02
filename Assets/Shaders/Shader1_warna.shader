Shader "Unlit/Shader1_warna"
{
    Properties
    {
        _Warna ("Warna", Color) = (0.145, 0.588, 0.745, 1)
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
            Tags
            {
                "LightMode" = "Universal2D"
            }

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Warna;
            CBUFFER_END

            struct Atribut
            {
                float4 posisiObjek : POSITION;
            };

            struct KeFragment
            {
                float4 posisiClip : SV_POSITION;
            };

            KeFragment Vert(Atribut masuk)
            {
                KeFragment keluar;

                keluar.posisiClip =
                    TransformObjectToHClip(masuk.posisiObjek.xyz);

                return keluar;
            }

            half4 Frag(KeFragment masuk) : SV_Target
            {
                return _Warna;
            }

            ENDHLSL
        }
    }
}