Shader "Game/Prototype/WorldTiledGround"
{
    Properties
    {
        _MainTex ("Grid Texture", 2D) = "white" {}
        _RepeatPerMeter ("Repeats Per Meter", Float) = 1.0
        _UVOffset ("UV Offset", Vector) = (0, 0, 0, 0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 worldPos   : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float _RepeatPerMeter;
            float4 _UVOffset;

            Varyings vert(Attributes input)
            {
                Varyings o;
                float3 worldPos = TransformObjectToWorld(input.positionOS);
                o.worldPos = worldPos;
                o.positionCS = TransformWorldToHClip(worldPos);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 uv = i.worldPos.xz * _RepeatPerMeter;
                uv += _UVOffset.xy;
                uv = frac(uv);
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                return col;
            }
            ENDHLSL
        }
    }

    FallBack Off
}
