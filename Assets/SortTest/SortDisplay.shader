// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Geometry/SortDisplay"
{
	Properties
	{
		_IndexSpread("Index Spread", float) = 0.001
		_IndexCount("Index Count", float) = 1024
		_ValueSpread("Value Spread", float) = 1
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

	struct SortData {
		int index;
		float distance;
	};

	StructuredBuffer<SortData> inputBuffer;


	struct gIn // OUT vertex shader, IN geometry shader
	{
		float4 pos : SV_POSITION;
		float4 col : COLOR0;
	};

	struct v2f // OUT geometry shader, IN fragment shader 
	{
		float4 pos		 : SV_POSITION;
		float4 col : COLOR0;
	};

	float _IndexCount;
	float _IndexSpread;
	float _ValueSpread;

	// ----------------------------------------------------
	gIn myVertexShader(uint id : SV_VertexID)
	{
		gIn o; // Out here, into geometry shader
			   // Passing on color to next shader (using .r/.g there as tile coordinate)
		
		// Passing on center vertex (tile to be built by geometry shader from it later)
		o.pos = float4(_IndexSpread * ((float)id / _IndexCount), 0, 0, 1);
		o.col = float4(inputBuffer[id].distance, 1, 1, 1);

		return o;
	}

	// ----------------------------------------------------

	[maxvertexcount(TAM)]
	// ----------------------------------------------------
	// Using "point" type as input, not "triangle"
	void myGeometryShader(point gIn vert[1], inout TriangleStream<v2f> triStream)
		{
		float height = vert[0].col.x > 1000 ? 0 : _ValueSpread * (1. + (0.9 * vert[0].col.x));
		float width = _IndexSpread / _IndexCount;
			
			//float f = _PointSize/20.0f; //half size
		float3 f = float3(width / 2, height, width / 2);

			const float4 vc[TAM] = { float4(-f.x,  f.y,  f.z, 0.0f), float4(f.x,  f.y,  f.z, 0.0f), float4(f.x,  f.y, -f.z, 0.0f),	//Top								
				float4(f.x,  f.y, -f.z, 0.0f), float4(-f.x,  f.y, -f.z, 0.0f), float4(-f.x,  f.y,  f.z, 0.0f),	//Top

				float4(f.x,  f.y, -f.z, 0.0f), float4(f.x,  f.y,  f.z, 0.0f), float4(f.x, 0,  f.z, 0.0f),	//Right
				float4(f.x, 0,  f.z, 0.0f), float4(f.x, 0, -f.z, 0.0f), float4(f.x,  f.y, -f.z, 0.0f),	//Right

				float4(-f.x,  f.y, -f.z, 0.0f), float4(f.x,  f.y, -f.z, 0.0f), float4(f.x, 0, -f.z, 0.0f),	//Front
				float4(f.x, 0, -f.z, 0.0f), float4(-f.x, 0, -f.z, 0.0f), float4(-f.x,  f.y, -f.z, 0.0f),	//Front

				float4(-f.x, 0, -f.z, 0.0f), float4(f.x, 0, -f.z, 0.0f), float4(f.x, 0,  f.z, 0.0f),	//Bottom										
				float4(f.x, 0,  f.z, 0.0f), float4(-f.x, 0,  f.z, 0.0f), float4(-f.x, 0, -f.z, 0.0f),	//Bottom

				float4(-f.x,  f.y,  f.z, 0.0f), float4(-f.x,  f.y, -f.z, 0.0f), float4(-f.x, 0, -f.z, 0.0f),	//Left
				float4(-f.x, 0, -f.z, 0.0f), float4(-f.x, 0,  f.z, 0.0f), float4(-f.x,  f.y,  f.z, 0.0f),	//Left

				float4(-f.x,  f.y,  f.z, 0.0f), float4(-f.x, 0,  f.z, 0.0f), float4(f.x, 0,  f.z, 0.0f),	//Back
				float4(f.x, 0,  f.z, 0.0f), float4(f.x,  f.y,  f.z, 0.0f), float4(-f.x,  f.y,  f.z, 0.0f)	//Back
			};


			const int TRI_STRIP[TAM] = { 0, 1, 2,  3, 4, 5,
				6, 7, 8,  9,10,11,
				12,13,14, 15,16,17,
				18,19,20, 21,22,23,
				24,25,26, 27,28,29,
				30,31,32, 33,34,35
			};

			v2f v[TAM];
			int i;

			// Assign new vertices positions 

			float4 col = vert[0].col;
			for (i = 0; i < TAM; i++) { v[i].pos = vert[0].pos + vc[i]; v[i].col = col; }

			// Position in view space
			for (i = 0; i<TAM; i++) { v[i].pos = UnityObjectToClipPos(v[i].pos); }

			// Build the cube tile by submitting triangle strip vertices
			for (i = 0; i<TAM / 3; i++)
			{
				triStream.Append(v[TRI_STRIP[i * 3 + 0]]);
				triStream.Append(v[TRI_STRIP[i * 3 + 1]]);
				triStream.Append(v[TRI_STRIP[i * 3 + 2]]);

				triStream.RestartStrip();
			}
		}

	// ----------------------------------------------------
	float4 myFragmentShader(v2f IN) : COLOR
	{
		//return float4(1.0,0.0,0.0,1.0);
		return float4(IN.col.x, 1.0 - IN.col.x, 1.0 - IN.col.x, 1.0);
	}

		ENDCG
	}
	}
}