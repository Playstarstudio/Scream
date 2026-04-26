Shader "Fullscreen/DitherLight"
{
    Properties
    {
        _DitherScale ("Dither Cell Size", Range(1, 8)) = 2.0
        _DitherStart ("Falloff Start", Range(0, 1)) = 0.5
        _DitherEnd ("Falloff End", Range(0, 1)) = 0.05
        [IntRange] _StencilRef ("Stencil Skip Value", Range(0, 255)) = 1
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            ZTest Always ZWrite Off Cull Off

            // Skip pixels where stencil == _StencilRef
            Stencil
            {
                Ref [_StencilRef]
                Comp NotEqual
                Pass Keep
            }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _DitherScale;
            float _DitherStart;
            float _DitherEnd;

            static const float Bayer8x8[64] = {
                 0.0/63.0, 32.0/63.0,  8.0/63.0, 40.0/63.0,  2.0/63.0, 34.0/63.0, 10.0/63.0, 42.0/63.0,
                48.0/63.0, 16.0/63.0, 56.0/63.0, 24.0/63.0, 50.0/63.0, 18.0/63.0, 58.0/63.0, 26.0/63.0,
                12.0/63.0, 44.0/63.0,  4.0/63.0, 36.0/63.0, 14.0/63.0, 46.0/63.0,  6.0/63.0, 38.0/63.0,
                60.0/63.0, 28.0/63.0, 52.0/63.0, 20.0/63.0, 62.0/63.0, 30.0/63.0, 54.0/63.0, 22.0/63.0,
                 3.0/63.0, 35.0/63.0, 11.0/63.0, 43.0/63.0,  1.0/63.0, 33.0/63.0,  9.0/63.0, 41.0/63.0,
                51.0/63.0, 19.0/63.0, 59.0/63.0, 27.0/63.0, 49.0/63.0, 17.0/63.0, 57.0/63.0, 25.0/63.0,
                15.0/63.0, 47.0/63.0,  7.0/63.0, 39.0/63.0, 13.0/63.0, 45.0/63.0,  5.0/63.0, 37.0/63.0,
                63.0/63.0, 31.0/63.0, 55.0/63.0, 23.0/63.0, 61.0/63.0, 29.0/63.0, 53.0/63.0, 21.0/63.0
            };

            half4 Frag(Varyings input) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord);
                float lum = dot(color.rgb, float3(0.299, 0.587, 0.114));

                // Above _DitherStart: fully lit, no dithering
                // Between _DitherStart and _DitherEnd: dither dissolve zone
                // Below _DitherEnd: fully black

                // How far into the dither zone are we? 1 = bright edge, 0 = dark edge
                float fade = saturate((lum - _DitherEnd) / max(_DitherStart - _DitherEnd, 0.001));

                // Above the start threshold, pass through untouched
                if (lum >= _DitherStart)
                    return color;

                // Below the end threshold, pure black
                if (lum <= _DitherEnd)
                    return half4(0, 0, 0, color.a);

                // In the dither zone: compare fade against the Bayer threshold
                // Pixels where fade > threshold stay lit, others go black
                uint2 p = uint2(fmod(abs(input.positionCS.xy / _DitherScale), 8.0));
                float threshold = Bayer8x8[p.y * 8 + p.x];

                if (fade > threshold)
                    return color;
                else
                    return half4(0, 0, 0, color.a);
            }
            ENDHLSL
        }
    }
}
