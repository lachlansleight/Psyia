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