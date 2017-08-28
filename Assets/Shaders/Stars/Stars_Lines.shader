Shader "Geometry/Lines" 
 {		
	Properties 
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_LineLength ("Line Length", float) = 1
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
			#pragma geometry myGeometryShader
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
			
			struct gIn // OUT vertex shader, IN geometry shader
			{
				float4 pos : SV_POSITION;
				float4 col : COLOR0;
				float4 posB : TANGENT;
			};
			
			struct v2f // OUT geometry shader, IN fragment shader 
			{
				float4 pos		 : SV_POSITION;
				float4 col : COLOR0;
			};
			
			float4	_Color;		
			float _LineLength;
			// ----------------------------------------------------
			gIn myVertexShader(uint id : SV_VertexID)
			{
				gIn o; // Out here, into geometry shader
				// Passing on color to next shader (using .r/.g there as tile coordinate)
				o.col = inputBuffer[id].color;
				// Passing on center vertex (tile to be built by geometry shader from it later)
				o.pos = mul(UNITY_MATRIX_VP, float4(inputBuffer[id].pos.xyz, 1.0));
				float3 velNorm = normalize(inputBuffer[id].velocity.xyz);
				float len = length(inputBuffer[id].velocity.xyz);
				if (len < 0.000001) {
					velNorm = float3(0, 1, 0);
				}
				float3 finalVel = inputBuffer[id].velocity.xyz;
				o.posB = mul(UNITY_MATRIX_VP, float4(inputBuffer[id].pos.xyz - velNorm * (len * _LineLength + 0.001), 1.0));
				//if(len < 0.001) { finalVel = velNorm * 0.01; }
				
				//debug spring constant
				//o.col = lerp(float4(1.0, 0.0, 0.0, 1.0), float4(0.0, 1.0, 0.0, 1.0), inputBuffer[id].anchor.w / 0.001);
  
				return o;
			}
			
			// ----------------------------------------------------
			
			[maxvertexcount(TAM)] 
			// ----------------------------------------------------
			// Using "point" type as input, not "triangle"
			void myGeometryShader(point gIn vert[1], inout LineStream<v2f> lineStream)
			{							
				float4 positions[2] = {	vert[0].posB, vert[0].pos };
				float4 colors[2] = { float4(vert[0].col.xyz, 0.0), vert[0].col };

				v2f linePoints[2];
				linePoints[0].pos = positions[0];
				linePoints[0].col = colors[0];
				linePoints[1].pos = positions[1];
				linePoints[1].col = colors[1];

				lineStream.Append(linePoints[0]);
				lineStream.Append(linePoints[1]);
				lineStream.RestartStrip();
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