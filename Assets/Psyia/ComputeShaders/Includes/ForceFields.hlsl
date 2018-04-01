float3 RotateVector(float3 input, float3 eulers) {
    eulers /= 57.29577951;
	float3x3 rotX = float3x3(
		1, 0, 0,
		0, cos(eulers.x), -sin(eulers.x),
		0, sin(eulers.x), cos(eulers.x)
		);
	float3x3 rotY = float3x3(
		cos(eulers.y), 0, sin(eulers.y),
		0, 1, 0,
		-sin(eulers.y), 0, cos(eulers.y)
		);
	float3x3 rotZ = float3x3(
		cos(eulers.z), -sin(eulers.z), 0,
		sin(eulers.z), cos(eulers.z), 0,
		0, 0, 1
		);

	float3x3 rotXYZ = mul(rotX, mul(rotY, rotZ));
	return mul(rotXYZ, input);
}

float GetAttenuation(int AttenuationMode, float AttenuationDistance, float Distance) {
    if(AttenuationMode == 0) { //Constant
        return 1.;
    }
    if(AttenuationMode == 1) { //Linear
        return saturate((-Distance / AttenuationDistance) + 1.);
    }
    if(AttenuationMode == 2) { //Hyperbolic
        return saturate((2. / ((Distance / AttenuationDistance) + 1.) - 1.));
    }
    if(AttenuationMode == 3) { //Hyperbolic Squared
        float PreCalcDistance = (Distance / AttenuationDistance) + 1;
        return saturate((1. / 3.) * (4. / (PreCalcDistance * PreCalcDistance) - 1.));
    }
    if(AttenuationMode == 4) { //sine
        return sin(6.2831853 * (Distance / AttenuationDistance) + 1.570796);
    }

    return 0;
}

float3 GetDirectionAtDisplacement(int ForceShape, float3 ForceRotation, float3 Displacement) {
    if(ForceShape == 0) { //Radial
        return normalize(Displacement);
    } else if(ForceShape == 1) { //Vortex
        float3 Forward = RotateVector(float3(0, 1., 0), ForceRotation);
        float3 VortexVector = cross(Forward, Displacement);

        return VortexVector;
    } else if(ForceShape == 2) { //Linear
        float3 LinearForce = float3(0, 0, 1);

        LinearForce = RotateVector(LinearForce, ForceRotation);
        return LinearForce;
    } else if(ForceShape == 3) { //Dipole
        //Displacement = float3(Displacement.x, Displacement.z, Displacement.y);
        float3 spherical = float3(
            length(Displacement), //rho
            atan2(Displacement.z, Displacement.x), //theta
            atan2(sqrt(Displacement.x * Displacement.x + Displacement.z * Displacement.z), Displacement.y) //phi
        );
        float2 ThetaSpace = float2(spherical.x * sin(spherical.z), spherical.x * cos(spherical.z));
        //ThetaSpace = float2(Displacement.x, Displacement.z);
        //float gradient = -1f * ((ThetaSpace.x - (((ThetaSpace.x * ThetaSpace.x) + (ThetaSpace.y * ThetaSpace.y)) / (2f * ThetaSpace.x))) / ThetaSpace.y);
        float gradient = (1. - 2. * cos(spherical.z) * cos(spherical.z)) / sin(2. * spherical.z);
        float2 Direction = float2(1., gradient);
        Direction *= (ThetaSpace.x < 0) != (ThetaSpace.y < 0) ? 1. : -1.;
        Direction = normalize(Direction);
        
        float3 Direction3D = float3(Direction.x, Direction.y, 0);
        Direction3D = RotateVector(Direction3D, float3(0, spherical.y, 0));

        float3 Force = float3(
            Direction.x * cos(spherical.z),
            Direction.x * sin(spherical.z),
            Direction.y
        );

        //Force = float3(Direction.x, Direction.y, 0f);

        //Force = float3(gradient, 0f, 0f);

        //Force = float3(Direction.x, 0f, Direction.y);
        
        //Force = float3(ThetaSpace.x, ThetaSpace.y, 0);

        //check theta
        //Force = float3(cos(spherical.y), 0f, sin(spherical.y));

        //float ThetaRadius = sqrt(Displacement.x * Displacement.x + Displacement.z * Displacement.z);
        //Force = float3(ThetaRadius * cos(spherical.y), spherical.x * cos(spherical.z), ThetaRadius * sin(spherical.y));

        //check phi
        //Force = float3(cos(spherical.z), 0f, sin(spherical.z));

        /*
        float3 Force = float3(
            cos(spherical.z),
            sin(spherical.z),
            0
        );*/



        return normalize(Force);
    }

    return float3(0, 0, 0);
}

float3 GetForceAtPoint(ForceData Force, float3 Position) {
    float3 Displacement = Force.Position - Position;
    
    float3 Direction = GetDirectionAtDisplacement(Force.Shape, Force.Rotation, Displacement);
    float Attenuation = GetAttenuation(Force.AttenuationMode, Force.AttenuationDistance, length(Displacement));

    return Direction * Attenuation * Force.Strength;
}