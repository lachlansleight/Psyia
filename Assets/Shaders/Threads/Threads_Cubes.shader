// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Geometry/ThreadCubes" 
 {		
	Properties 
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_PointSize("Point Size", Float) = 1.0
		_ShadowStrength("Shadow Strength", Float) = 0.2
	}
	
	SubShader 
	{
		LOD 200
		
		Pass 
		{
			Tags {"LightMode" = "ForwardBase"} 
			//ZWrite Off
			//Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
 
			#pragma only_renderers d3d11
			#pragma target 4.0
			
			#include "UnityCG.cginc"
 
			#pragma vertex   myVertexShader
			#pragma geometry myGeometryShader
			#pragma fragment myFragmentShader
				
			#define TAM 36

			struct data {
				float3 restpos;
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
				float3 normal : NORMAL;
			};
			
			struct gIn // OUT vertex shader, IN geometry shader
			{
				float4 pos : SV_POSITION;
				float4 col : COLOR0;
				float3 nor : NORMAL;
			};
			
			struct v2f // OUT geometry shader, IN fragment shader 
			{
				float4 pos		 : SV_POSITION;
				float4 col : COLOR0;
			};
			
			float4	_Color;
			float	_PointSize;
			float _ShadowStrength;
			uniform float4 _LightColor0;			
			// ----------------------------------------------------
			gIn myVertexShader(uint id : SV_VertexID)
			{
				gIn o; // Out here, into geometry shader
				// Passing on color to next shader (using .r/.g there as tile coordinate)
				o.col = bufferPoints[id].col;				
				// Passing on center vertex (tile to be built by geometry shader from it later)
				o.pos = float4(bufferPoints[id].pos, 1.0);
				o.nor = bufferPoints[id].size;
  
				return o;
			}
			
			// ----------------------------------------------------
			
			[maxvertexcount(TAM)] 
			// ----------------------------------------------------
			// Using "point" type as input, not "triangle"
			void myGeometryShader(point gIn vert[1], inout TriangleStream<v2f> triStream)
			{							
				//float f = _PointSize/20.0f; //half size
				float3 f = float3(vert[0].nor.x / 2.0, vert[0].nor.y / 2.0, vert[0].nor.z / 2.0);
				
				const float4 vc[TAM] = { float4( -f.x,  f.y,  f.z, 0.0f), float4(  f.x,  f.y,  f.z, 0.0f), float4(  f.x,  f.y, -f.z, 0.0f),	//Top								
										float4(  f.x,  f.y, -f.z, 0.0f), float4( -f.x,  f.y, -f.z, 0.0f), float4( -f.x,  f.y,  f.z, 0.0f),	//Top
										
										float4(  f.x,  f.y, -f.z, 0.0f), float4(  f.x,  f.y,  f.z, 0.0f), float4(  f.x, -f.y,  f.z, 0.0f),	//Right
										float4(  f.x, -f.y,  f.z, 0.0f), float4(  f.x, -f.y, -f.z, 0.0f), float4(  f.x,  f.y, -f.z, 0.0f),	//Right
										
										float4( -f.x,  f.y, -f.z, 0.0f), float4(  f.x,  f.y, -f.z, 0.0f), float4(  f.x, -f.y, -f.z, 0.0f),	//Front
										float4(  f.x, -f.y, -f.z, 0.0f), float4( -f.x, -f.y, -f.z, 0.0f), float4( -f.x,  f.y, -f.z, 0.0f),	//Front
										
										float4( -f.x, -f.y, -f.z, 0.0f), float4(  f.x, -f.y, -f.z, 0.0f), float4(  f.x, -f.y,  f.z, 0.0f),	//Bottom										
										float4(  f.x, -f.y,  f.z, 0.0f), float4( -f.x, -f.y,  f.z, 0.0f), float4( -f.x, -f.y, -f.z, 0.0f),	//Bottom
										
										float4( -f.x,  f.y,  f.z, 0.0f), float4( -f.x,  f.y, -f.z, 0.0f), float4( -f.x, -f.y, -f.z, 0.0f),	//Left
										float4( -f.x, -f.y, -f.z, 0.0f), float4( -f.x, -f.y,  f.z, 0.0f), float4( -f.x,  f.y,  f.z, 0.0f),	//Left
										
										float4( -f.x,  f.y,  f.z, 0.0f), float4( -f.x, -f.y,  f.z, 0.0f), float4(  f.x, -f.y,  f.z, 0.0f),	//Back
										float4(  f.x, -f.y,  f.z, 0.0f), float4(  f.x,  f.y,  f.z, 0.0f), float4( -f.x,  f.y,  f.z, 0.0f)	//Back
										};

															
				const int TRI_STRIP[TAM]  = {  0, 1, 2,  3, 4, 5,
												6, 7, 8,  9,10,11,
											 12,13,14, 15,16,17,
											 18,19,20, 21,22,23,
											 24,25,26, 27,28,29,
											 30,31,32, 33,34,35  
											 }; 
															
				v2f v[TAM];
				int i;
				
				// Assign new vertices positions 
				if(_ShadowStrength > 0.0) {
					const float3 nm[TAM] = {
						float3(0,  1, 0), float3(0,  1, 0), float3(0,  1, 0), float3(0,  1, 0), float3(0,  1, 0), float3(0,  1, 0),
						float3( 1, 0, 0), float3( 1, 0, 0), float3( 1, 0, 0), float3( 1, 0, 0), float3( 1, 0, 0), float3( 1, 0, 0),
						float3(0, 0,  1), float3(0, 0,  1), float3(0, 0,  1), float3(0, 0,  1), float3(0, 0,  1), float3(0, 0,  1),
						float3(0, -1, 0), float3(0, -1, 0), float3(0, -1, 0), float3(0, -1, 0), float3(0, -1, 0), float3(0, -1, 0),
						float3(-1, 0, 0), float3(-1, 0, 0), float3(-1, 0, 0), float3(-1, 0, 0), float3(-1, 0, 0), float3(-1, 0, 0),
						float3(0, 0, -1), float3(0, 0, -1), float3(0, 0, -1), float3(0, 0, -1), float3(0, 0, -1), float3(0, 0, -1)
					};

					for (i=0;i<TAM;i++) { 
						v[i].pos = vert[0].pos + vc[i]; 

						float3 normalDirection = normalize(mul(float4(nm[i], 0.0), unity_WorldToObject).xyz);
                 		float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                 		float3 diffuseReflection = _LightColor0.rgb * vert[0].col.rgb * max(0.0, dot(normalDirection, lightDirection));

                 		v[i].col = float4(lerp(vert[0].col, float4(diffuseReflection, 1.0), clamp(_ShadowStrength, 0.0, 1.0)).rgb, vert[0].col.a); 
					}

				} else {
					for (i=0;i<TAM;i++) { v[i].pos = vert[0].pos + vc[i]; v[i].col = vert[0].col * _Color;	}
				}
				
				// Position in view space
				for (i=0;i<TAM;i++) { v[i].pos = mul(UNITY_MATRIX_MVP, v[i].pos); }
					
				// Build the cube tile by submitting triangle strip vertices
				for (i=0;i<TAM/3;i++)
				{ 
					triStream.Append(v[TRI_STRIP[i*3+0]]);
					triStream.Append(v[TRI_STRIP[i*3+1]]);
					triStream.Append(v[TRI_STRIP[i*3+2]]);	
									
					triStream.RestartStrip();
				}
			}
			
			// ----------------------------------------------------
			float4 myFragmentShader(v2f IN) : COLOR
			{
				//return float4(1.0,0.0,0.0,1.0);
				return IN.col;
			}
 
			ENDCG
		}
	} 
 }