/*

Copyright (c) 2018 Lachlan Sleight
Modified 2018 by Lachlan Sleight for PernicketySplit

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



Shader "Psyia/Petrichor"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_PointSize("Point Size", Float) = 1.0
		_Image("Image", 2D) = "white" {}
		_Y ("Chroma Y", Range(-0.5, 1.5)) = 0.7
		_rIndex ("R Index", Int) = 0
		_gIndex ("G Index", Int) = 1
		_bIndex ("B Index", Int) = 2
	}

		SubShader
	{
		LOD 200

		Pass
	{
		Tags{ "Queue" = "Transparent" }
		ZWrite Off
		Cull Off
		Blend One One

		CGPROGRAM

#pragma only_renderers d3d11
#pragma target 4.0
#pragma multi_compile_instancing

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

	float _Y;
	uint _rIndex;
	uint _gIndex;
	uint _bIndex;
	uniform float4x4 SystemMatrix;
	uniform float4x4 InverseSystemMatrix;

	float3 GetRgbFromYCbCr(float Cb, float Cr) {
		float y = _Y * 255;
		float cb = Cb * 255;
		float cr = Cr * 255;

		float ScaleFactor = 255./129.;

		float r = ((255. / 219.) * (y - 16)) + ((255. / 112.) * 0.701 * (cr - 128));
		float g = ((255. / 219.) * (y - 16)) - ((255. / 112.) * 0.886 * (0.114 / 0.587) * (cb - 128)) - ((255. / 112.) * 0.701 * (0.299 / 0.587) * (cr - 128));
		float b = ((255. / 129.) * (y - 16)) + ((255. / 112.) * 0.866 * (cb - 128));

		float3 final = float3(r, g, b);
		return float3(final[_rIndex % 3], final[_gIndex % 3], final[_bIndex % 3]) / 255;
	}

	float3 GetRgbFromVelocity(float3 velocity) {
		float3 VelocityDirection = normalize(velocity);
		float theta = atan(VelocityDirection.z / VelocityDirection.x);
		float phi = atan(VelocityDirection.y / length(VelocityDirection.xz));

		float ThetaMap = (theta / 1.5707963) % 1.0;
		float PhiMap = (phi / 1.5707963) % 1.0;

		if(ThetaMap < 0) {
			ThetaMap = ThetaMap * -1;
		}
		if(PhiMap < 0) {
			PhiMap = PhiMap * -1;
		}

		return GetRgbFromYCbCr(ThetaMap, PhiMap);
	}

	struct vIn
	{
		uint id : SV_VertexID;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct gIn // OUT vertex shader, IN geometry shader
	{
		float4 pos : SV_POSITION;
		float4 col : COLOR0;
		float3 nor : NORMAL;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f // OUT geometry shader, IN fragment shader 
	{
		float4 pos		 : SV_POSITION;
		float2 uv : TEXCOORD0;
		float4 col : COLOR0;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	float4	_Color;
	float	_PointSize;
	float _ShadowStrength;
	uniform float4 _LightColor0;
	Texture2D _Image;
	SamplerState sampler_Image;

	// ----------------------------------------------------
	gIn myVertexShader(vIn v)
	{
		gIn o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_TRANSFER_INSTANCE_ID(v, o);

		uint vId = DistanceBuffer[v.id].Index;
		//o.col = lerp(float4(1, 0, 0, 1), float4(0, 0, 1, 1), saturate(DistanceBuffer[id].distance));
		o.col = ParticleBuffer[vId].Color * float4(GetRgbFromVelocity(ParticleBuffer[vId].Velocity), 1.0);
		float rotation = (float)vId / 1024.0;
		o.pos = float4(mul(SystemMatrix, float4(ParticleBuffer[vId].Position, 1)).xyz, rotation);
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
		UNITY_SETUP_INSTANCE_ID(vert[0]);

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

		/*
		uv[0] = RotateUV(uv[0], rotation);
		uv[1] = RotateUV(uv[1], rotation);
		uv[2] = RotateUV(uv[2], rotation);
		uv[3] = RotateUV(uv[3], rotation);
		*/

		v2f pIn[TAM];
		int i;

		for(i=0;i<TAM;i++) {
			pIn[i].pos = mul(UNITY_MATRIX_VP, vc[i]);
			pIn[i].uv = uv[i];
			pIn[i].col = vert[0].col;
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(pIn[i]);
			triStream.Append(pIn[i]);
		}
	}

	// ----------------------------------------------------
	float4 myFragmentShader(v2f IN) : COLOR
	{
		float4 texCol = _Image.Sample(sampler_Image, IN.uv);
		float4 col = IN.col * _Color;
		//alpha blended
		//col.a = texCol.r * texCol.a * col.a;

		//additive
		col *= texCol.r * texCol.a * col.a;
		return col;
	}

		ENDCG
	}
	}
}