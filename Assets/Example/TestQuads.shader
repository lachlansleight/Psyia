Shader "Geometry/Quads"
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

#pragma vertex   myVertexShader
#pragma geometry myGeometryShader
#pragma fragment myFragmentShader

#define TAM 4

	struct data {
		float3 pos;
		float3 velocity;
		float4 col;
		int isAlive;
		float age;
	};

	StructuredBuffer<data> inputBuffer;

	float4 RotatePoint(float4 p, float3 offsetVector, float3 sideVector, float3 upVector) {
		float3 finalPos = p.xyz;
		finalPos += offsetVector.x * sideVector;
		finalPos += offsetVector.y * upVector;

		return float4(finalPos,1);
	}

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
		o.col = inputBuffer[id].col;
		o.pos = float4(inputBuffer[id].pos, 1.0);
		o.nor = float3(1, 1, 1) * _PointSize;
		return o;
	}

	//gIn myVertexShader(appdata_base v) {
	//gIn o;
	//o.pos = mul(_Object2World, v.vertex);
	//o.nor = float3(1.0, 1.0, 1.0);
	//o.col = float4(1.0, 0.0, 0.0, 1.0);
	//return o;
	//}



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


		const float4 vc[TAM] = {
			RotatePoint(vert[0].pos, float3(-sz.x, -sz.y,0),right,up),
			RotatePoint(vert[0].pos, float3(-sz.x, sz.y,0),right,up),
			RotatePoint(vert[0].pos, float3(sz.x, -sz.y,0),right,up),
			RotatePoint(vert[0].pos, float3(sz.x, sz.y,0),right,up)
		};

		v2f pIn;

		pIn.pos = mul(UNITY_MATRIX_VP, vc[0]);
		pIn.uv = float2(0.0, 0.0);
		pIn.col = vert[0].col;
		triStream.Append(pIn);

		pIn.pos = mul(UNITY_MATRIX_VP, vc[1]);
		pIn.uv = float2(0.0, 1.0);
		pIn.col = vert[0].col;
		triStream.Append(pIn);

		pIn.pos = mul(UNITY_MATRIX_VP, vc[2]);
		pIn.uv = float2(1.0, 0.0);
		pIn.col = vert[0].col;
		triStream.Append(pIn);

		pIn.pos = mul(UNITY_MATRIX_VP, vc[3]);
		pIn.uv = float2(1.0, 1.0);
		pIn.col = vert[0].col;
		triStream.Append(pIn);
	}

	// ----------------------------------------------------
	float4 myFragmentShader(v2f IN) : COLOR
	{
		//return float4(1.0,0.0,0.0,1.0);
		return IN.col * _Image.Sample(sampler_Image, IN.uv) * _Color;
	}

		ENDCG
	}
	}
}