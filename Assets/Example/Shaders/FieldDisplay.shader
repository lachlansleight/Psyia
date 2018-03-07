Shader "Geometry/FieldLines"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_LineLength("Line Length", float) = 1
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

		struct FieldStruct {
		float3 pos;
		float3 force;
};

		StructuredBuffer<FieldStruct> inputBuffer;

	float3 FieldCount;
	float3 FieldStartPos;
	float3 FieldEndPos;

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
		float idF = (float)id;
		float3 FieldPos = inputBuffer[id].pos;

		o.pos = UnityObjectToClipPos(FieldPos);
		o.posB = UnityObjectToClipPos(FieldPos + inputBuffer[id].force * _LineLength);
		o.col = _Color;

		return o;
	}

	// ----------------------------------------------------

	[maxvertexcount(TAM)]
	// ----------------------------------------------------
	// Using "point" type as input, not "triangle"
	void myGeometryShader(point gIn vert[1], inout LineStream<v2f> lineStream)
	{
		float4 positions[2] = { vert[0].posB, vert[0].pos };
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