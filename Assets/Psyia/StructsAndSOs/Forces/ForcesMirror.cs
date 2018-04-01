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
		} else if(ForceShape == 3) { //Dipole
			//Displacement = new Vector3(Displacement.x, Displacement.z, Displacement.y);
			Vector3 spherical = new Vector3(
				Displacement.magnitude, //rho
				Mathf.Atan2(Displacement.z, Displacement.x), //theta
				Mathf.Atan2(Mathf.Sqrt(Displacement.x * Displacement.x + Displacement.z * Displacement.z), Displacement.y) //phi
			);
			Vector2 ThetaSpace = new Vector2(spherical.x * Mathf.Sin(spherical.z), spherical.x * Mathf.Cos(spherical.z));
			//ThetaSpace = new Vector2(Displacement.x, Displacement.z);
			//float gradient = -1f * ((ThetaSpace.x - (((ThetaSpace.x * ThetaSpace.x) + (ThetaSpace.y * ThetaSpace.y)) / (2f * ThetaSpace.x))) / ThetaSpace.y);
			float gradient = (1f - 2f * Mathf.Cos(spherical.z) * Mathf.Cos(spherical.z)) / Mathf.Sin(2f * spherical.z);
			Vector2 Direction = new Vector2(1f, gradient);
			Direction *= (ThetaSpace.x < 0) != (ThetaSpace.y < 0) ? 1 : -1;
			Direction.Normalize();
			
			Vector3 Direction3D = new Vector3(Direction.x, Direction.y, 0);
			Direction3D = RotateVector(Direction3D, new Vector3(0f, spherical.y, 0f));

			Vector3 Force = new Vector3(
				Direction.x * Mathf.Cos(spherical.z),
				Direction.x * Mathf.Sin(spherical.z),
				Direction.y
			);

			//Force = new Vector3(Direction.x, Direction.y, 0f);

			//Force = new Vector3(gradient, 0f, 0f);

			//Force = new Vector3(Direction.x, 0f, Direction.y);
			
			//Force = new Vector3(ThetaSpace.x, ThetaSpace.y, 0);

			//check theta
			//Force = new Vector3(Mathf.Cos(spherical.y), 0f, Mathf.Sin(spherical.y));

			//float ThetaRadius = Mathf.Sqrt(Displacement.x * Displacement.x + Displacement.z * Displacement.z);
			//Force = new Vector3(ThetaRadius * Mathf.Cos(spherical.y), spherical.x * Mathf.Cos(spherical.z), ThetaRadius * Mathf.Sin(spherical.y));

			//check phi
			//Force = new Vector3(Mathf.Cos(spherical.z), 0f, Mathf.Sin(spherical.z));

			/*
			Vector3 Force = new Vector3(
				Mathf.Cos(spherical.z),
				Mathf.Sin(spherical.z),
				0
			);*/

			Force.Normalize();

			return Force;
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
