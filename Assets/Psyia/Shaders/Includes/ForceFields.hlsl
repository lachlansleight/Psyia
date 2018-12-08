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
    if(AttenuationDistance == 0 && AttenuationMode != 0) {
        return 0;
    }
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
    if(AttenuationMode == 5) { //hyperbolic softened
        return 1.0 / (Distance + AttenuationDistance);
    }
    if(AttenuationMode == 6) { //hyperbolic squared softened
        return 1.0 / (Distance * Distance + AttenuationDistance);
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
        float Separation = 0.001;
        float3 SeparationOffset = Separation * 0.5 * RotateVector(float3(0, 0, 1), ForceRotation);

        float3 AttractDisplacement = Displacement + SeparationOffset;
        float3 RepelDisplacement = Displacement - SeparationOffset;

        float AttractDisplacementLength = length(AttractDisplacement);
        float RepelDisplacementLength = length(RepelDisplacement);

        float3 AttractForce = normalize(AttractDisplacement) / (AttractDisplacementLength * AttractDisplacementLength);
        float3 RepelForce = -1. * normalize(RepelDisplacement) / (RepelDisplacementLength * RepelDisplacementLength);

        return normalize(AttractForce + RepelForce);
    }

    return float3(0, 0, 0);
}

float3 GetForceAtPoint(ForceData Force, float3 Position) {
    float3 Displacement = Force.Position - Position;
    
    float3 Direction = GetDirectionAtDisplacement(Force.Shape, Force.Rotation, Displacement);
    float Attenuation = GetAttenuation(Force.AttenuationMode, Force.AttenuationDistance, length(Displacement));

    return Direction * Attenuation * Force.Strength;
}