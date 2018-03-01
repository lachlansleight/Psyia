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

	struct data {
		float4 color;
	};

	StructuredBuffer<data> inputBuffer;


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
		o.col = inputBuffer[id].color;

		return o;
	}

	// ----------------------------------------------------

	[maxvertexcount(TAM)]
	// ----------------------------------------------------
	// Using "point" type as input, not "triangle"
	void myGeometryShader(point gIn vert[1], inout TriangleStream<v2f> lineStream)
	{
		float height = _ValueSpread * (1. + (0.9 * vert[0].col.x));
		float width = _IndexSpread / _IndexCount;

		float4 LineLengthOffset = float4(0, height, 0, 0);
		float LineWidthOffset = float4(width, 0, 0, 0);

		float4 positions[4] = { 
			vert[0].pos, 
			vert[0].pos + LineLengthOffset,
			vert[0].pos + LineWidthOffset + LineLengthOffset,
			vert[0].pos + LineWidthOffset
		};

		positions[0] = UnityObjectToClipPos(positions[0]);
		positions[1] = UnityObjectToClipPos(positions[1]);
		positions[2] = UnityObjectToClipPos(positions[2]);
		positions[3] = UnityObjectToClipPos(positions[3]);

		float4 colors[4] = { 
			vert[0].col, vert[0].col,
			vert[0].col, vert[0].col
		};

		v2f linePoints[6];
		linePoints[0].pos = positions[0];
		linePoints[0].col = colors[0];
		linePoints[1].pos = positions[1];
		linePoints[1].col = colors[1];
		linePoints[2].pos = positions[2];
		linePoints[2].col = colors[2];

		linePoints[3].pos = positions[0];
		linePoints[3].col = colors[0];
		linePoints[4].pos = positions[2];
		linePoints[4].col = colors[2];
		linePoints[5].pos = positions[3];
		linePoints[5].col = colors[3];

		lineStream.Append(linePoints[0]);
		lineStream.Append(linePoints[1]);
		lineStream.Append(linePoints[2]);
		lineStream.RestartStrip();

		lineStream.Append(linePoints[3]);
		lineStream.Append(linePoints[4]);
		lineStream.Append(linePoints[5]);
		lineStream.RestartStrip();
	}

	// ----------------------------------------------------
	float4 myFragmentShader(v2f IN) : COLOR
	{
		//return float4(1.0,0.0,0.0,1.0);
		return float4(IN.col.xyz, 1.0);
	}

		ENDCG
	}
	}
}