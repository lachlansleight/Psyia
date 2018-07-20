Shader "Psyia/Points"
{

	Properties{
		_Color("Color", Color) = (1, 1, 1, 1)
	}

		SubShader
	{
		LOD 200

		Pass
	{
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

#include "UnityCG.cginc"
#include "../Includes/Structs.hlsl"

#pragma vertex   myVertexShader
#pragma fragment myFragmentShader

	float4 _Color;

	//The buffer that our compute shader is editing
	StructuredBuffer<ParticleData> ParticleBuffer;

	struct v2f {
		float4 pos : SV_POSITION;
		float4 col : COLOR0;
	};

	//We don't take in a struct anymore, we instead take in an index of the inputBuffer.
	//So rather than running per-vertex, this function runs per inputBuffer index
	//We could perform any typical vertex manipulation here if we wanted
	v2f myVertexShader(uint id : SV_VertexID)
	{
		v2f o;
		o.col = ParticleBuffer[id].Color;
		o.pos = mul(UNITY_MATRIX_VP, float4(ParticleBuffer[id].Position, 1.0));

		return o;
	}

	//This is a normal fragment shader.
	float4 myFragmentShader(v2f IN) : COLOR
	{
		return IN.col * _Color;
	}

		ENDCG
	}
	}
}