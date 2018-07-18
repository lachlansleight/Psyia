struct ForceData {
    float Strength;
    int AttenuationMode;
    float AttenuationDistance;
    int Shape;
    float3 Position;
    float3 Rotation;
    float2 Padding;
};

struct ParticleData {
	float3 Position;
	float3 Velocity;
	float4 Color;
	int IsAlive;
	float Age;
};

struct DistanceData {
	int Index;
	float Distance;
    float2 Padding;
};