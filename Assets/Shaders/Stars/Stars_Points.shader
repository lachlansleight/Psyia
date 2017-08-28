Shader "Geometry/Points" 
 {		
	Properties 
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	
	SubShader 
	{
		LOD 200
		
		Pass 
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
 
			#pragma only_renderers d3d11
			#pragma target 4.0
			
			#include "UnityCG.cginc"
 
			#pragma vertex   myVertexShader
			#pragma fragment myFragmentShader
				
			#define TAM 36

			struct data {
				float4 pos;
				float4 velocity;
				float4 scale;
				float4 color;
				float4 randomSeed;
				float4 anchor;
			};

			StructuredBuffer<data> inputBuffer;
												
			struct vIn // Into the vertex shader
			{
				float4 vertex : POSITION;
				float4 color  : COLOR0;
			};
			
			struct v2f // OUT geometry shader, IN fragment shader 
			{
				float4 pos		 : SV_POSITION;
				float4 col : COLOR0;
			};
			
			float4	_Color;		
			// ----------------------------------------------------
			v2f myVertexShader(uint id : SV_VertexID)
			{
				v2f o; // Out here, into geometry shader
				// Passing on color to next shader (using .r/.g there as tile coordinate)
				o.col = inputBuffer[id].color;
				// Passing on center vertex (tile to be built by geometry shader from it later)
				o.pos = mul(UNITY_MATRIX_VP, float4(inputBuffer[id].pos.xyz, 1.0));
				//if (inputBuffer[id].x <= 0) o.pos = mul(UNITY_MATRIX_VP, float4(1000, 0, 0, 1.0));
  
				return o;
			}
			
			// ----------------------------------------------------
			float4 myFragmentShader(v2f IN) : COLOR
			{
				//return float4(1.0,0.0,0.0,1.0);
				return IN.col * _Color;
			}
 
			ENDCG
		}
	} 
 }