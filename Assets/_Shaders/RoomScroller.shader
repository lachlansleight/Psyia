// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/RoomScroller"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollSpeedA ("Scroll Speed A", Vector) = (0, -0.1, 0, 0)
        _ScrollSpeedB ("Scroll Speed B", Vector) = (0.09, 0, 0, 0)
        _ScrollSpeedC ("Scroll Speed C", Vector) = (-0.07, 0.03, 0, 0)
        _ColorA ("Color A", Color) = (1, 0, 0, 1)
        _ColorB ("Color B", Color) = (0, 1, 0, 1)
        _ColorC ("Color C", Color) = (0, 0, 1, 1)
        _FadeCenter ("Fade Center", Vector) = (0, 0, 0, 0)
        _FadeDistance ("Fade Distance", Range(0.0, 20.0)) = 0.0
        _FadeColor ("Fade Color", Color) = (0, 0, 0, 1)
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
                float3 worldPos : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float2 _ScrollSpeedA;
            float2 _ScrollSpeedB;
            float2 _ScrollSpeedC;
            float4 _ColorA;
            float4 _ColorB;
            float4 _ColorC;
            float3 _FadeCenter;
            float _FadeDistance;
            float4 _FadeColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv + _Time.y * _ScrollSpeedA) * _ColorA;
                col += tex2D(_MainTex, i.uv + _Time.y * _ScrollSpeedB) * _ColorB;
                col += tex2D(_MainTex, i.uv + _Time.y * _ScrollSpeedC) * _ColorC;
                col = saturate(col);
                
                if(_FadeDistance <= 0) {
                    return col;
                }
                
                float distance = saturate(length(i.worldPos - _FadeCenter) / _FadeDistance);
                if(distance < 0.5) {
                    distance = 0.0;
                } else {
                    distance = 1.0;
                }
                
                return lerp(_FadeColor, col, distance);
            }
            ENDCG
        }
    }
}
