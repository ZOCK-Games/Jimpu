Shader "Custom/PixelAudio_RealResponse"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Line Color", Color) = (0, 1, 0, 1)
        _BackgroundColor ("Background Color", Color) = (0, 0, 0, 0)
        
        _Loudness ("Loudness", Range(0, 1)) = 0.5
        _Sensitivity ("Sensitivity", Range(0, 20)) = 5.0
        
        _Resolution ("Pixel Resolution", Range(8, 512)) = 64
        _Speed ("Wave Speed", Range(0, 30)) = 10
        _Thickness ("Line Thickness", Range(0.01, 0.5)) = 0.05
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "PreviewType"="Plane" }
        Cull Off Lighting Off ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _BackgroundColor;
            float _Loudness;
            float _Sensitivity;
            float _Resolution;
            float _Speed;
            float _Thickness;

            // Pseudo-Random Funktion für die Zacken
            float hash(float n) { 
                return frac(sin(n) * 43758.5453123); 
            }

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                // 1. Pixel-Grid berechnen
                float2 uv = floor(i.uv * _Resolution) / _Resolution;
                
                // 2. Zeit für die Bewegung
                float t = _Time.y * _Speed;

                // 3. ECHTE REAKTION: 
                // Wir nehmen einen Noise-Wert (-0.5 bis 0.5) und multiplizieren ihn 
                // direkt mit Loudness. Bei Loudness 0 wird das Ergebnis 0.
                float noise = hash(floor(uv.x * _Resolution + t)) - 0.5;
                
                // Der Ausschlag (Amplitude)
                float wave = noise * (_Loudness * _Sensitivity);
                
                // Auf Pixel einrasten und in die Mitte setzen (0.5)
                wave = floor(wave * _Resolution) / _Resolution;
                float finalY = uv.y - (0.5 + wave);

                // 4. Linie zeichnen
                fixed4 col = (abs(finalY) < _Thickness) ? _Color : _BackgroundColor;
                
                // Sprite-Transparenz Support
                col.rgb *= col.a;
                return col;
            }
            ENDCG
        }
    }
}