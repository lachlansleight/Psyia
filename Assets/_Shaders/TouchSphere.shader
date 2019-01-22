Shader "Custom/TouchSphere"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        [HDR] _Emission("Color", Color) = (0, 0, 0, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _SpeedX ("Speed X", Float) = 1
        _SpeedY ("Speed Y", Float) = 1
        _FrequencyX ("Frequency X", Float) = 1
        _FrequencyY ("Frequency Y", Float) = 1
        _Displacement ("Displacement", Float) = 1
        _CustomTimeX ("Custom Time X", Float) = 0
        _CustomTimeY ("Custom Time Y", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        
        #include "/Assets/Dependencies/Psyia/AssetPackages/ShaderNoise/HLSL/SimplexNoise2D.hlsl"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 normal;
            float disp;
        };
        
        float _SpeedX;
        float _SpeedY;
        float _FrequencyX;
        float _FrequencyY;
        float _Displacement;
        float _CustomTimeX;
        float _CustomTimeY;
        
        float3 getVertexPos(float3 input, float2 uv) {
            float3 displacedPoint = input;

            //don't displace top and bottom to avoid holes
            float displacementVerticalFactor = saturate(-cos(uv.y * 2.0 * 3.1415926535897) * 0.7 + 0.5);
            float displacementHorizontalFactor = saturate(-cos(uv.x * 2.0 * 3.1415926535897) * 0.7 + 0.5);
            
            
            
            float displacementAmount = snoise(float2((uv.x + _CustomTimeX) * _FrequencyX, (uv.y + _CustomTimeY) * _FrequencyY));

            displacedPoint += normalize(input) * _Displacement * displacementAmount * displacementVerticalFactor * displacementHorizontalFactor;
            return displacedPoint;
        }
        
        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
        
            float4x4 modelMatrix = unity_ObjectToWorld;
            float4x4 modelMatrixInverse = unity_WorldToObject; 

            float3 pos = getVertexPos(v.vertex, v.texcoord);
            float3 posTangent = getVertexPos(v.vertex + v.tangent * 0.01, v.texcoord + float2(1,1) * 0.01);
            float3 bitangent = cross(v.normal, v.tangent);
            float3 posBitangent = getVertexPos(v.vertex + bitangent * 0.01, v.texcoord + float2(-1,-1) * 0.01);
                  
            o.normal = cross(posTangent - pos, posBitangent - pos);
            
            o.disp = length(pos - v.vertex);

            v.vertex = float4(pos, v.vertex.w);
        }

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float4 _Emission;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            o.Normal = IN.normal;
            o.Emission = lerp(float4(0,0,0,1), _Emission, saturate(IN.disp / _Displacement));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
