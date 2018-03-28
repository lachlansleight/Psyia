Shader "Psyia/ParticleLines"
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
#include "Assets/Psyia/ComputeShaders/Includes/Structs.hlsl"

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