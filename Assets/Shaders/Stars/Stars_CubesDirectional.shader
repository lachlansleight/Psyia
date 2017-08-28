// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Geometry/CubesDirectional" 
 {		
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_PointSize("Point Size", Float) = 1.0
		_LineLength("Line Length", Float) = 5.0
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
				
			#define TAM 24

			struct data {
				float4 pos;
				float4 velocity;
				float4 scale;
				float4 color;
				float4 randomSeed;
				float4 anchor;
			};

			StructuredBuffer<data> inputBuffer;
			
			struct gIn // OUT vertex shader, IN geometry shader
			{
				float4 pos : SV_POSITION;
				float4 col : COLOR0;
				float3 nor : NORMAL;
				float4 vel : TANGENT;
			};
			
			struct v2f // OUT geometry shader, IN fragment shader 
			{
				float4 pos		 : SV_POSITION;
				float4 col : COLOR0;
			};
			
			float4	_Color;
			float	_PointSize;
			float _LineLength;
			float _ShadowStrength;
			uniform float4 _LightColor0;			
			// ----------------------------------------------------
			gIn myVertexShader(uint id : SV_VertexID)
			{
				gIn o; // Out here, into geometry shader
				// Passing on color to next shader (using .r/.g there as tile coordinate)
				o.col = inputBuffer[id].color;				
				// Passing on center vertex (tile to be built by geometry shader from it later)
				o.pos = float4(inputBuffer[id].pos.xyz, 1.0);
				o.nor = inputBuffer[id].scale.xyz;
				o.vel = inputBuffer[id].velocity;
  
				return o;
			}
			
			// ----------------------------------------------------
			
			[maxvertexcount(TAM)] 
			// ----------------------------------------------------
			// Using "point" type as input, not "triangle"
			void myGeometryShader(point gIn vert[1], inout TriangleStream<v2f> triStream)
			{							
				//float f = _PointSize/20.0f; //half size
				float3 f = float3(vert[0].nor.x / 2.0, vert[0].nor.y / 2.0, vert[0].nor.z / 2.0) * _PointSize;

				float3 velN = normalize(vert[0].vel.xyz);
				if (length(vert[0].vel.xyz) < 0.000001) {
					velN = float3(0, 0, 1);
				}
				
				float3 horizontalVector = normalize(cross(velN, float3(0, 1, 0)));
				float3 verticalVector = normalize(cross(velN, horizontalVector));

				float4 p = float4(vert[0].pos.xyz + velN * f.z, 1.0);
				float4 fw = p + float4(velN * f.z, 0.0);
				float4 bk = p - float4(velN * f.z, 0.0) - vert[0].vel * _LineLength;
				float4 up = p + float4(verticalVector * f.y, 0.0);
				float4 dn = p - float4(verticalVector * f.y, 0.0);
				float4 rt = p + float4(horizontalVector * f.x, 0.0);
				float4 lt = p - float4(horizontalVector * f.x, 0.0);

				//all eight surface normals
				float3 rufN = normalize(cross(rt - fw, up - fw)); //[r]ight, [u]p, [f]ront Normal
				float3 lufN = normalize(cross(fw - up, up - lt));
				float3 rdfN = normalize(cross(fw - dn, rt - fw));
				float3 ldfN = normalize(cross(lt - fw, lt - dn));
				float3 rubN = normalize(cross(up - bk, bk - rt));
				float3 lubN = normalize(cross(up - lt, lt - bk));
				float3 rdbN = normalize(cross(dn - rt, rt - bk)); //[l]eft, [d]own, [b]ack Normal (etc) - note XYZ order
				float3 ldbN = normalize(cross(bk - lt, lt - dn));
				
				const float4 vc[TAM] = { 
					fw, rt, up, fw, up, lt, 
					fw, lt, dn, fw, dn, rt,
					bk, up, rt, bk, rt, dn,
					bk, dn, lt, bk, lt, up
				};

															
				const int TRI_STRIP[TAM]  = {
					0,1,2,3,4,5,
					6,7,8,9,10,11,
					12,13,14,15,16,17,
					18,19,20,21,22,23
				}; 
															
				v2f v[TAM];

				int i;

				//for (i = 0; i<TAM; i++) { v[i].pos = vert[0].pos + vc[i]; v[i].col = vert[0].col * _Color; }
				


				
				// Assign new vertices positions 
				if(_ShadowStrength > 0.0) {
					//flat shading
					const float3 nm[TAM] = {
						rufN, rufN, rufN, lufN, lufN, lufN,
						ldfN, ldfN, ldfN, rdfN, rdfN, rdfN,
						rubN, rubN, rubN, rdbN, rdbN, rdbN,
						ldbN, ldbN, ldbN, lubN, lubN, lubN
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