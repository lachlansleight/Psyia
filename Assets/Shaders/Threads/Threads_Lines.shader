Shader "Geometry/ThreadLines" 
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
			#pragma geometry myGeometryShader
			#pragma fragment myFragmentShader
				
			#define TAM 36

			struct data {
				float3 centerPos;
				float3 pos;
				float4 col;
				float3 size;
				float3 velocity;
				float charge;
			};

			StructuredBuffer<data> bufferPoints;
												
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
				//float2 posCa : TEXCOORD0;
				//float2 posCb : TEXCOORD1;
			};
			
			struct v2f // OUT geometry shader, IN fragment shader 
			{
				float4 pos		 : SV_POSITION;
				float4 col : COLOR0;
			};
			
			float4	_Color;		
			// ----------------------------------------------------
			gIn myVertexShader(uint id : SV_VertexID)
			{
				gIn o; // Out here, into geometry shader
				// Passing on color to next shader (using .r/.g there as tile coordinate)
				o.col = bufferPoints[id].col;				
				// Passing on center vertex (tile to be built by geometry shader from it later)
				o.pos = mul(UNITY_MATRIX_VP, float4(bufferPoints[id].pos, 1.0));
				//float3 velNorm = normalize(bufferPoints[id].velocity);
				//float len = length(bufferPoints[id].velocity);
				//float3 finalVel = bufferPoints[id].velocity;
				//if(len < 0.01) { finalVel = velNorm * 0.01; }
				//o.posB = mul(UNITY_MATRIX_VP, float4(bufferPoints[id].pos - finalVel * 0.1, 1.0));
				float4 anchorPoint = float4(bufferPoints[id].centerPos.x, 0, bufferPoints[id].centerPos.z, 2.0);
				o.posB = anchorPoint;
				//o.posB = mul(UNITY_MATRIX_VP, float4(bufferPoints[id].centerPos.x, 0.0, bufferPoints[id].centerPos.z, 1.0));
				//float4 posC = mul(UNITY_MATRIX_VP, float4(bufferPoints[id].centerPos.x, 3.0, bufferPoints[id].centerPos.z, 1.0));
				//o.posCa = posC.xy;
				//o.posCb = posC.zw;

				return o;
			}
			
			// ----------------------------------------------------
			
			[maxvertexcount(TAM)] 
			// ----------------------------------------------------
			// Using "point" type as input, not "triangle"
			void myGeometryShader(point gIn vert[1], inout LineStream<v2f> lineStream)
			{
				float4 anchorPosA = mul(UNITY_MATRIX_VP, float4(vert[0].posB.xyz, 1.0));//float4(vert[0].posB);//float4(vert[0].posB.x, 3.0, vert[0].posB.z, 1.0);
				float4 anchorPosB = mul(UNITY_MATRIX_VP, float4(vert[0].posB.xwz, 1.0));//float4(vert[0].posCa, vert[0].posCb);//float4(vert[0].posB.x, 0.0, vert[0].posB.z, 1.0);							
				float4 positions[3] = {	anchorPosA, vert[0].pos, anchorPosB };
				float4 colors[3] = { vert[0].col, vert[0].col, vert[0].col };

				v2f linePoints[3];

				linePoints[0].pos = positions[0];
				linePoints[0].col = float4(colors[1].rgb, 0.0);
				linePoints[1].pos = positions[1];
				linePoints[1].col = colors[0];
				linePoints[2].pos = positions[2];
				linePoints[2].col = float4(colors[1].rgb, 0.0);

				lineStream.Append(linePoints[0]);
				lineStream.Append(linePoints[1]);
				lineStream.Append(linePoints[2]);
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