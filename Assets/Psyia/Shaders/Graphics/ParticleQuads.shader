Shader "Psyia/Quads"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_PointSize("Point Size", Float) = 1.0
		_Image("Image", 2D) = "white" {}
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

#define TAM 4

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
		float2 uv : TEXCOORD0;
		float4 col : COLOR0;
	};

	float4	_Color;
	float	_PointSize;
	float _ShadowStrength;
	uniform float4 _LightColor0;
	Texture2D _Image;
	SamplerState sampler_Image;

	// ----------------------------------------------------
	gIn myVertexShader(uint id : SV_VertexID)
	{
		gIn o;
		uint vId = DistanceBuffer[id].Index;
		//o.col = lerp(float4(1, 0, 0, 1), float4(0, 0, 1, 1), saturate(DistanceBuffer[id].distance));
		o.col = ParticleBuffer[vId].Color;
		float rotation = (float)vId / 1024.0;
		o.pos = float4(ParticleBuffer[vId].Position, rotation);
		o.nor = float3(1, 1, 1) * _PointSize * ParticleBuffer[vId].Size;
		return o;
	}

	//gIn myVertexShader(appdata_base v) {
	//gIn o;
	//o.pos = mul(_Object2World, v.vertex);
	//o.nor = float3(1.0, 1.0, 1.0);
	//o.col = float4(1.0, 0.0, 0.0, 1.0);
	//return o;
	//}

	float2 RotateUV(float2 uv, float angle) {
		uv -= 0.5;
		float s = sin(angle);
		float c = cos(angle);
		float2x2 rotationMatrix = float2x2(c, -s, s, c);
		rotationMatrix *= 0.5;
		rotationMatrix += 0.5;
		rotationMatrix = rotationMatrix * 2 - 1;
		uv = mul(uv, rotationMatrix);
		uv += 0.5;

		return uv;
	}

	// ----------------------------------------------------

	[maxvertexcount(10)]
	// ----------------------------------------------------
	// Using "point" type as input, not "triangle"
	void myGeometryShader(point gIn vert[1], inout TriangleStream<v2f> triStream)
	{
		float2 sz = float2(0.5 * vert[0].nor.x, 0.5 * vert[0].nor.y) * _PointSize;
		//float2 sz = float2(0.005, 0.005);

		float3 up = float3(0,1,0);
		float3 look = _WorldSpaceCameraPos - vert[0].pos.xyz;
		look = normalize(look);
		float3 right = normalize(cross(up, look));
		up = normalize(cross(right, look));

		//float f = _PointSize/20.0f; //half size
		float rotation = vert[0].pos.w;
		vert[0].pos.w = 1.0;

		const float4 vc[TAM] = {
			RotatePoint(vert[0].pos, float3(-sz.x, -sz.y,0),right,up),
			RotatePoint(vert[0].pos, float3(-sz.x, sz.y,0),right,up),
			RotatePoint(vert[0].pos, float3(sz.x, -sz.y,0),right,up),
			RotatePoint(vert[0].pos, float3(sz.x, sz.y,0),right,up)
		};

		float2 uv[TAM] = {
			float2(0, 0),
			float2(0, 1),
			float2(1, 0),
			float2(1, 1)
		};

		uv[0] = RotateUV(uv[0], rotation);
		uv[1] = RotateUV(uv[1], rotation);
		uv[2] = RotateUV(uv[2], rotation);
		uv[3] = RotateUV(uv[3], rotation);


		v2f pIn;

		pIn.pos = mul(UNITY_MATRIX_VP, vc[0]);
		pIn.uv = uv[0];
		pIn.col = vert[0].col;
		triStream.Append(pIn);

		pIn.pos = mul(UNITY_MATRIX_VP, vc[1]);
		pIn.uv = uv[1];
		pIn.col = vert[0].col;
		triStream.Append(pIn);

		pIn.pos = mul(UNITY_MATRIX_VP, vc[2]);
		pIn.uv = uv[2];
		pIn.col = vert[0].col;
		triStream.Append(pIn);

		pIn.pos = mul(UNITY_MATRIX_VP, vc[3]);
		pIn.uv = uv[3];
		pIn.col = vert[0].col;
		triStream.Append(pIn);
	}

	// ----------------------------------------------------
	float4 myFragmentShader(v2f IN) : COLOR
	{
		float4 texCol = _Image.Sample(sampler_Image, IN.uv);
		float4 col = IN.col * _Color;
		col.a = texCol.r * texCol.a * col.a;
		return col;
	}

		ENDCG
	}
	}
}