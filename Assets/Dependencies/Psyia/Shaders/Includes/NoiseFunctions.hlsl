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



#include "../../AssetPackages/ShaderNoise/HLSL/SimplexNoise3D.hlsl"

float3 GetPotential(float3 p, float time) {
	float3 pot = float3(0,0,0);

	float L = 20;

	pot += L * float3(
		snoise(float4(p.x, p.y,       p.z,      time) / L),
		snoise(float4(p.x, p.y + 43,  p.z,      time) / L),
		snoise(float4(p.x, p.y,       p.z + 43, time) / L)
	);

	return pot;
}

float3 GetNoiseVelocity(float3 p, float scale, float time) {
	p /= scale;

	float epsilon = 0.0001;
	float3 pot = GetPotential(p, time);
	float dp3_dy = (pot.z - GetPotential(float3(p.x, p.y + epsilon, p.z), time)).z / epsilon;
	float dp2_dz = (pot.y - GetPotential(float3(p.x, p.y, p.z + epsilon), time)).y / epsilon;
	float dp1_dz = (pot.x - GetPotential(float3(p.x, p.y, p.z + epsilon), time)).x / epsilon;
	float dp3_dx = (pot.z - GetPotential(float3(p.x + epsilon, p.y, p.z), time)).z / epsilon;
	float dp2_dx = (pot.y - GetPotential(float3(p.x + epsilon, p.y, p.z), time)).y / epsilon;
	float dp1_dy = (pot.x - GetPotential(float3(p.x, p.y + epsilon, p.z), time)).x / epsilon;

	return float3(dp3_dy - dp2_dz, dp1_dz - dp3_dx, dp2_dx - dp1_dy);
}

float3 GetNoiseVelocitySum(float3 p, float scale, int octaves, float time) {
	float3 Sum = float3(0,0,0);
	float3 CurrentP = p;
	float CurrentScale = scale;
	for(int i = 0; i < octaves; i++) {
		Sum += GetNoiseVelocity(CurrentP, CurrentScale, time);
		CurrentScale *= 0.5;
	}
	return Sum / octaves;
}