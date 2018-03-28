using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcesMirror : MonoBehaviour {

	static Vector3 RotateVector(Vector3 input, Vector3 eulers) {
		return Quaternion.Euler(eulers) * input;
	}

	static float GetAttenuation(int AttenuationMode, float AttenuationDistance, float Distance) {
		if(AttenuationMode == 0) { //Constant
			return 1f;
		}
		if(AttenuationMode == 1) { //Linear
			return Mathf.Clamp01((-Distance / AttenuationDistance) + 1f);
		}
		if(AttenuationMode == 2) { //Hyperbolic
			return Mathf.Clamp01((2f / ((Distance / AttenuationDistance) + 1f)) - 1f);
		}
		if(AttenuationMode == 3) { //Hyperbolic Squared
			float PreCalcDistance = (Distance / AttenuationDistance) + 1;
			return Mathf.Clamp01((1f / 3f) * (4f / (PreCalcDistance * PreCalcDistance) - 1f));
		}
		if(AttenuationMode == 4) { //Sine
			return Mathf.Sin(6.2831853f * (Distance / AttenuationDistance) + 1.570796f);
		}

		return 0;
	}

	static Vector3 GetDirectionAtDisplacement(int ForceShape, Vector3 ForceRotation, Vector3 Displacement) {
		if(ForceShape == 0) { //Radial
			return Vector3.Normalize(Displacement);
		} else if(ForceShape == 1) { //Vortex
			Vector3 Forward = RotateVector(new Vector3(0, 1, 0), ForceRotation);
			Vector3 VortexVector = Vector3.Cross(Forward, Displacement);
			
			return VortexVector;
		} else if(ForceShape == 2) { //Linear
			Vector3 LinearForce = new Vector3(0, 0, 1);

			LinearForce = RotateVector(LinearForce, ForceRotation);
			return LinearForce;
		}

		return new Vector3(0, 0, 0);
	}

	public static Vector3 GetForceAtPoint(ForceData Force, Vector3 Position) {
		Vector3 Displacement = Force.Position - Position;
		
		Vector3 Direction = GetDirectionAtDisplacement(Force.Shape, Force.Rotation, Displacement);
		float Attenuation = GetAttenuation(Force.AttenuationMode, Force.AttenuationDistance, Vector3.Magnitude(Displacement));

		return Direction * Attenuation * Force.Strength;
	}
}
