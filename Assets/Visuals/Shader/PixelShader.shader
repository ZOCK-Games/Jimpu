Shader "Custom/UI_Perfect_Pixel_Line"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _PixelSize ("Pixel Block Size (px)", Float) = 4
        _PixelMode ("Pixel Mode (0=screen,1=texture)", Float) = 1
        _UseGlobalLookup ("Use Global Lookup (0/1)", Float) = 0
        _GlobalTex ("Global Lookup Texture", 2D) = "white" {}
        _GlobalPixelSize ("Global Pixel Size (px)", Float) = 4

        _OutlineMode ("Outline Mode (0=off,1=holes,2=outline)", Float) = 0
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness (px)", Float) = 1
        _AlphaThreshold ("Alpha Threshold", Float) = 0.01

        _UseTextureAlpha ("Use Texture Alpha for Discard (0/1)", Float) = 0

        _DebugShow ("Debug: show solid color", Float) = 0
        _DebugColor ("Debug Color", Color) = (1,0,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _GlobalTex;
            float4 _GlobalTex_ST;
            fixed4 _Color;
            float _PixelSize;
            float _PixelMode;
            float _UseGlobalLookup;
            float _GlobalPixelSize;
            float _OutlineMode;
            fixed4 _OutlineColor;
            float _OutlineThickness;
            float4 _MainTex_TexelSize;
            float _AlphaThreshold;
            float _UseTextureAlpha;
            float _DebugShow;
            fixed4 _DebugColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.vertex;
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Prepare UVs - fallback to screenUV if mesh UVs are empty
                float2 screenUV = (IN.screenPos.xy / IN.screenPos.w) * 0.5 + 0.5;
                float2 uv = IN.texcoord;
                if (uv.x == 0.0 && uv.y == 0.0)
                    uv = screenUV;
                uv = TRANSFORM_TEX(uv, _MainTex);

                // derivatives used by screen-space mapping
                float2 duvdx = ddx(uv);
                float2 duvdy = ddy(uv);
                float derivLen = length(duvdx) + length(duvdy);

                float2 sampleUv;
                if (_PixelMode < 0.5)
                {
                    // SCREEN-SPACE MODE: quantize by screen pixels
                    float2 pixelPos = screenUV * _ScreenParams.xy;
                    float2 pixelCenter = (floor(pixelPos / _PixelSize) + 0.5) * _PixelSize;
                    float2 deltaPixels = pixelCenter - pixelPos;
                    float2 uvOffset = derivLen < 1e-6 ? float2(0,0) : (deltaPixels.x * duvdx + deltaPixels.y * duvdy);
                    sampleUv = saturate(uv + uvOffset);
                }
                else
                {
                    // TEXTURE-SPACE MODE: quantize in texel units so blocks are consistent per texture
                    // _MainTex_TexelSize: (1/width,1/height,width,height)
                    float2 texSize = float2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);
                    float2 texelCoord = uv * texSize;
                    float2 texelCenter = (floor(texelCoord / _PixelSize) + 0.5) * _PixelSize;
                    sampleUv = saturate(texelCenter / texSize);
                }

                fixed4 texCol = tex2D(_MainTex, sampleUv);

                // If global lookup is enabled, sample the shared global texture using screen-space pixel center
                if (_UseGlobalLookup > 0.5)
                {
                    float2 pixelPos = screenUV * _ScreenParams.xy;
                    float2 gPixelCenter = (floor(pixelPos / _GlobalPixelSize) + 0.5) * _GlobalPixelSize;
                    float2 globalUV = saturate(gPixelCenter / _ScreenParams.xy);
                    texCol = tex2D(_GlobalTex, globalUV);
                }

                // If texture alpha shouldn't control visibility, prefer vertex/material color
                fixed4 color;
                if (_DebugShow > 0.5)
                {
                    // Force a solid debug color (no discard) so we can confirm material is applied
                    return _DebugColor;
                }
                else if (_UseTextureAlpha < 0.5 && texCol.a <= _AlphaThreshold)
                {
                    // Use vertex color (or material color) when texture is blank/transparent
                    color = (IN.color.a <= 0.001) ? _Color : IN.color;
                }
                else
                {
                    color = texCol * ((IN.color.a <= 0.001) ? _Color : IN.color);
                }

                // Only discard when texture alpha is explicitly required
                if (_UseTextureAlpha > 0.5 && color.a <= _AlphaThreshold) discard;

                // outline / holes
                if (_OutlineMode > 0.5)
                {
                    float2 offR = float2(_OutlineThickness, 0);
                    float2 offL = float2(-_OutlineThickness, 0);
                    float2 offU = float2(0, _OutlineThickness);
                    float2 offD = float2(0, -_OutlineThickness);

                    float2 uvR = saturate(uv + (offR.x * duvdx + offR.y * duvdy));
                    float2 uvL = saturate(uv + (offL.x * duvdx + offL.y * duvdy));
                    float2 uvU = saturate(uv + (offU.x * duvdx + offU.y * duvdy));
                    float2 uvD = saturate(uv + (offD.x * duvdx + offD.y * duvdy));

                    float aR = _UseGlobalLookup > 0.5 ? tex2D(_GlobalTex, saturate((floor((screenUV * _ScreenParams.xy + float2(_OutlineThickness,0)) / _GlobalPixelSize) + 0.5) * _GlobalPixelSize / _ScreenParams.xy)).a : tex2D(_MainTex, uvR).a;
                    float aL = _UseGlobalLookup > 0.5 ? tex2D(_GlobalTex, saturate((floor((screenUV * _ScreenParams.xy + float2(-_OutlineThickness,0)) / _GlobalPixelSize) + 0.5) * _GlobalPixelSize / _ScreenParams.xy)).a : tex2D(_MainTex, uvL).a;
                    float aU = _UseGlobalLookup > 0.5 ? tex2D(_GlobalTex, saturate((floor((screenUV * _ScreenParams.xy + float2(0,_OutlineThickness)) / _GlobalPixelSize) + 0.5) * _GlobalPixelSize / _ScreenParams.xy)).a : tex2D(_MainTex, uvU).a;
                    float aD = _UseGlobalLookup > 0.5 ? tex2D(_GlobalTex, saturate((floor((screenUV * _ScreenParams.xy + float2(0,-_OutlineThickness)) / _GlobalPixelSize) + 0.5) * _GlobalPixelSize / _ScreenParams.xy)).a : tex2D(_MainTex, uvD).a;

                    bool neighborTransparent = (aR <= _AlphaThreshold) || (aL <= _AlphaThreshold) || (aU <= _AlphaThreshold) || (aD <= _AlphaThreshold);

                    if (neighborTransparent)
                    {
                        if (_OutlineMode < 1.5)
                        {
                            discard; // hole
                        }
                        else
                        {
                            color = _OutlineColor * IN.color; // outline color
                        }
                    }
                }

                return color;
            }
        ENDCG
        }
    }
    FallBack "UI/Default"
}
