Shader "Custom/FullPBR_URP"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _ColorMap ("Color Map", 2D) = "white" {}
        _UseColorMap ("Use Color Map", Float) = 1

        _NormalMap ("Normal Map", 2D) = "bump" {}
        _UseNormalMap ("Use Normal Map", Float) = 1

        _Metallic ("Metallic", Range(0,1)) = 0
        _MetallicMap ("Metallic Map", 2D) = "white" {}
        _UseMetallicMap ("Use Metallic Map", Float) = 0

        _Roughness ("Roughness", Range(0,1)) = 0.5
        _RoughnessMap ("Roughness Map", 2D) = "white" {}
        _UseRoughnessMap ("Use Roughness Map", Float) = 0

        _EmissiveColor ("Emissive Color", Color) = (0,0,0,1)
        _EmissiveMap ("Emissive Map", 2D) = "black" {}
        _UseEmissiveMap ("Use Emissive Map", Float) = 0
        _UseEmissiveColor ("Use Emissive Color", Float) = 1

        _AoMap ("AO Map", 2D) = "white" {}
        _UseAoMap ("Use AO Map", Float) = 0

        _UVOffset ("UV Offset", Vector) = (0,0,0,0)
        _UVScale ("UV Scale", Vector) = (1,1,0,0)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
        LOD 300

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : NORMAL;
                float3 tangentWS : TEXCOORD1;
                float3 bitangentWS : TEXCOORD2;
            };

            // Properties
            float4 _BaseColor;
            TEXTURE2D(_ColorMap); SAMPLER(sampler_ColorMap);
            float _UseColorMap;

            TEXTURE2D(_NormalMap); SAMPLER(sampler_NormalMap);
            float _UseNormalMap;

            float _Metallic;
            TEXTURE2D(_MetallicMap); SAMPLER(sampler_MetallicMap);
            float _UseMetallicMap;

            float _Roughness;
            TEXTURE2D(_RoughnessMap); SAMPLER(sampler_RoughnessMap);
            float _UseRoughnessMap;

            float4 _EmissiveColor;
            TEXTURE2D(_EmissiveMap); SAMPLER(sampler_EmissiveMap);
            float _UseEmissiveMap;
            float _UseEmissiveColor;

            TEXTURE2D(_AoMap); SAMPLER(sampler_AoMap);
            float _UseAoMap;

            float4 _UVOffset;
            float4 _UVScale;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv * _UVScale.xy + _UVOffset.xy;

                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 tangentWS = TransformObjectToWorldDir(IN.tangentOS.xyz);
                float3 bitangentWS = cross(normalWS, tangentWS) * IN.tangentOS.w;

                OUT.normalWS = normalWS;
                OUT.tangentWS = tangentWS;
                OUT.bitangentWS = bitangentWS;

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // Base Color
                float4 colorTex = SAMPLE_TEXTURE2D(_ColorMap, sampler_ColorMap, IN.uv);
                float4 albedo = lerp(_BaseColor, colorTex * _BaseColor, _UseColorMap);

                // Metallic
                float metallicTex = SAMPLE_TEXTURE2D(_MetallicMap, sampler_MetallicMap, IN.uv).r;
                float metallic = lerp(_Metallic, metallicTex, _UseMetallicMap);

                // Roughness
                float roughnessTex = SAMPLE_TEXTURE2D(_RoughnessMap, sampler_RoughnessMap, IN.uv).r;
                float roughness = lerp(_Roughness, roughnessTex, _UseRoughnessMap);

                // Normal
                float3 normalTS = float3(0, 0, 1);
                if (_UseNormalMap > 0.5)
                {
                    normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, IN.uv));
                }
                float3x3 TBN = float3x3(IN.tangentWS, IN.bitangentWS, IN.normalWS);
                float3 normalWS = normalize(mul(normalTS, TBN));

                // Emissive
                float3 emissive = float3(0,0,0);
                if (_UseEmissiveMap > 0.5)
                {
                    float3 emissiveTex = SAMPLE_TEXTURE2D(_EmissiveMap, sampler_EmissiveMap, IN.uv).rgb;
                    emissive += emissiveTex;
                }
                if (_UseEmissiveColor > 0.5)
                {
                    emissive += _EmissiveColor.rgb;
                }

                // AO
                float aoTex = SAMPLE_TEXTURE2D(_AoMap, sampler_AoMap, IN.uv).r;
                float ao = lerp(1.0, aoTex, _UseAoMap);

                // Lighting
                InputData inputData = (InputData)0;
                inputData.positionWS = TransformObjectToWorld(IN.positionHCS);
                inputData.normalWS = normalWS;
                inputData.viewDirectionWS = GetWorldSpaceViewDir(inputData.positionWS);
                inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);

                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = albedo.rgb;
                surfaceData.metallic = metallic;
                surfaceData.smoothness = 1.0 - roughness;
                surfaceData.normalTS = normalTS;
                surfaceData.emission = emissive;
                surfaceData.occlusion = ao;
                surfaceData.alpha = albedo.a;

                return UniversalFragmentPBR(inputData, surfaceData);
            }
            ENDHLSL
        }
    }
}
