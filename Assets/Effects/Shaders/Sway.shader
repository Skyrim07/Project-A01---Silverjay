Shader "A01/Sway"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
         _WindDir("Wind Direction", Range(-1,1)) = 1
        _Speed ("Speed", float) = 1
        _Amplitude ("Amplitude", float) = 1
        _Power ("Power", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Speed, _Amplitude, _Power, _WindDir;


            float4 CubicSmooth(float4 x) {
                return x * x * (3.0 - 2.0 * x);
            }

            float4 TriangleWave(float4 x) {
                return abs(frac(x + 182.2 + 0.5) * 2.0 - 1.0);
            }

            float4 SineApproximation(float4 x) {
                return CubicSmooth(TriangleWave(x));
            }

            v2f vert (appdata v)
            {
                v2f o;

                float3 vPos = v.vertex;
                float fLength = length(vPos);
                float fBF = vPos.y * (_Amplitude) * ((SineApproximation(_Time[3] * (_Speed)) + 0.5) * 0.5);
                // Smooth bending factor and increase its nearby height limit.
                fBF += 1.0;
                fBF *= fBF;
                fBF = fBF * fBF - fBF;
                // Displace position
                float3 vNewPos = vPos;

                vNewPos.x += _WindDir * fBF * saturate(vPos.y);
                // Rescale
                vPos.xy = normalize(vNewPos.xy) * fLength;
                v.vertex.xy = vPos;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }
            ENDCG
        }
    }
}
