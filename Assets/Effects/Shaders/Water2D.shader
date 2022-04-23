Shader "Unlit/Water2D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RenderTex ("Render Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
            _Speed("Speed", float) = 1
            _Magnitude ("Magnitude", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _RenderTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;

            fixed4 _Color;
            float _Speed, _Magnitude;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 disp = tex2D(_NoiseTex, i.uv + frac(_Time.y * _Speed * 0.05)-0.5).rg;
                disp = ((disp * 2) - 1) * _Magnitude;
                i.uv.y = 1 - i.uv.y;
                fixed4 col = tex2D(_RenderTex, i.uv+disp);

                col *= _Color;

                return col;
            }
            ENDCG
        }
    }
}
