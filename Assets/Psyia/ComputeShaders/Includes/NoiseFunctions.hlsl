float3 GetPotential(float3 p, float Time) {
	float3 pot = float3(0,0,0);

	float L = 20;

	pot += L * float3(
		snoise(float4(p.x, p.y,       p.z,      Time * NoiseAnimationSpeed) / L),
		snoise(float4(p.x, p.y + 43,  p.z,      Time * NoiseAnimationSpeed) / L),
		snoise(float4(p.x, p.y,       p.z + 43, Time * NoiseAnimationSpeed) / L)
	);

	return pot;
}

float3 GetNoiseVelocity(float3 p, float scale, float Time) {
	p /= scale;

	float epsilon = 0.0001;
	float3 pot = GetPotential(p);
	float dp3_dy = (pot.z - GetPotential(float3(p.x, p.y + epsilon, p.z), Time)).z / epsilon;
	float dp2_dz = (pot.y - GetPotential(float3(p.x, p.y, p.z + epsilon), Time)).y / epsilon;
	float dp1_dz = (pot.x - GetPotential(float3(p.x, p.y, p.z + epsilon), Time)).x / epsilon;
	float dp3_dx = (pot.z - GetPotential(float3(p.x + epsilon, p.y, p.z), Time)).z / epsilon;
	float dp2_dx = (pot.y - GetPotential(float3(p.x + epsilon, p.y, p.z), Time)).y / epsilon;
	float dp1_dy = (pot.x - GetPotential(float3(p.x, p.y + epsilon, p.z), Time)).x / epsilon;

	return float3(dp3_dy - dp2_dz, dp1_dz - dp3_dx, dp2_dx - dp1_dy);
}

float3 GetNoiseVelocitySum(float3 p, int octaves) {
	float3 Sum = float3(0,0,0);
	float3 CurrentP = p;
	float CurrentScale = 1.;
	for(int i = 0; i < octaves; i++) {
		Sum += GetNoiseVelocity(CurrentP, CurrentScale);
		CurrentScale *= 0.5;
	}
	return Sum / octaves;
}