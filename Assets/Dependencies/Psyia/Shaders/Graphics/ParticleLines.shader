/*

Copyright (c) 2018 Lachlan Sleight

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/



Shader "Psyia/Lines"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_LineLength("Point Size", Float) = 1.0
	}

		SubShader
	{
		LOD 200

		Pass
	{
		Tags{ "Queue" = "Transparent" }
		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

#pragma only_renderers d3d11
#pragma target 4.0

#include "UnityCG.cginc"
#include "../Includes/Structs.hlsl"

#pragma vertex   myVertexShader
#pragma geometry myGeometryShader
#pragma fragment myFragmentShader

#define TAM 2

	StructuredBuffer<ParticleData> ParticleBuffer;
	StructuredBuffer<DistanceData> DistanceBuffer;

	float4 RotatePoint(float4 p, float3 offsetVector, float3 sideVector, float3 upVector) {
		float3 finalPos = p.xyz;
		finalPos += offsetVector.x * sideVector;
		finalPos += offsetVector.y * upVector;

		return float4(finalPos,1);
	}

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
	float	_LineLength;

	// ----------------------------------------------------
	gIn myVertexShader(uint id : SV_VertexID)
	{
		gIn o;
		uint vId = DistanceBuffer[id].Index;
		//o.col = lerp(float4(1, 0, 0, 1), float4(0, 0, 1, 1), saturate(DistanceBuffer[id].distance));
		o.col = ParticleBuffer[vId].Color;
		o.pos = float4(ParticleBuffer[vId].Position, 1.);
		o.nor = ParticleBuffer[vId].Velocity;
		return o;
	}

	// ----------------------------------------------------

	[maxvertexcount(10)]
	// ----------------------------------------------------
	// Using "point" type as input, not "triangle"
	void myGeometryShader(point gIn vert[1], inout LineStream<v2f> lineStream)
	{
		const float4 vc[TAM] = {
			vert[0].pos,
			vert[0].pos - (float4(vert[0].nor * _LineLength, 0.)) + float4(0.001, 0, 0, 0)
		};


		v2f pIn;

		pIn.pos = mul(UNITY_MATRIX_VP, vc[0]);
		pIn.col = vert[0].col;
		lineStream.Append(pIn);

		pIn.pos = mul(UNITY_MATRIX_VP, vc[1]);
		pIn.col = vert[0].col * 0.; //fade out line end
		lineStream.Append(pIn);
	}

	// ----------------------------------------------------
	float4 myFragmentShader(v2f IN) : COLOR
	{
		float4 col = IN.col * _Color;
		return col;
	}

		ENDCG
	}
	}
}