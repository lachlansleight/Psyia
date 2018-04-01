using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTools;

public class CartesianSpherical : MonoBehaviour {

	public bool ControlCartesian = true;

	[Header("Cartesian")]
	[Range(-10f, 10f)] public float CartesianX;
	[Range(-10f, 10f)] public float CartesianY;
	[Range(-10f, 10f)] public float CartesianZ;

	[Header("Spherical")]
	[Range(-10f, 10f)] public float SphericalR;
	[Range(-360f, 360f)] public float SphericalT;
	[Range(-360f, 360f)] public float SphericalP;

	public Transform CartesianObject;
	public Transform CenterPoint;
	
	void Update () {
		DrawAxes();
		DrawCartesian();
		DrawSpherical();
	}

	void DrawAxes() {
		VRDebug.DrawLine(CenterPoint.position + new Vector3(-0.5f, 0f, 0f), CenterPoint.position + new Vector3(0.5f, 0f, 0f), Color.red, false, 0.005f);
		VRDebug.DrawLine(CenterPoint.position + new Vector3(0f, -0.5f, 0f), CenterPoint.position + new Vector3(0f, 0.5f, 0f), Color.green, false, 0.005f);
		VRDebug.DrawLine(CenterPoint.position + new Vector3(0f, 0f, -0.5f), CenterPoint.position + new Vector3(0f, 0f, 0.5f), Color.blue, false, 0.005f);
	}

	void DrawCartesian() {
		Vector3 PlanePoint = new Vector3(CenterPoint.position.x, CartesianObject.position.y, CartesianObject.position.z);
		VRDebug.DrawLine(CartesianObject.position, PlanePoint, Color.red, false, 0.001f);		
		VRDebug.DrawLine(PlanePoint, new Vector3(CenterPoint.position.x, CartesianObject.position.y, CenterPoint.position.z), Color.green, false, 0.001f);
		VRDebug.DrawLine(PlanePoint, new Vector3(CenterPoint.position.x, CenterPoint.position.y, CartesianObject.position.z), Color.blue, false, 0.001f);

		PlanePoint = new Vector3(CartesianObject.position.x, CenterPoint.position.y, CartesianObject.position.z);
		VRDebug.DrawLine(CartesianObject.position, PlanePoint, Color.green, false, 0.001f);		
		VRDebug.DrawLine(PlanePoint, new Vector3(CenterPoint.position.x, CenterPoint.position.y, CartesianObject.position.z), Color.blue, false, 0.001f);
		VRDebug.DrawLine(PlanePoint, new Vector3(CartesianObject.position.x, CenterPoint.position.y, CenterPoint.position.z), Color.red, false, 0.001f);

		PlanePoint = new Vector3(CartesianObject.position.x, CartesianObject.position.y, CenterPoint.position.z);
		VRDebug.DrawLine(CartesianObject.position, PlanePoint, Color.blue, false, 0.001f);		
		VRDebug.DrawLine(PlanePoint, new Vector3(CartesianObject.position.x, CenterPoint.position.y, CenterPoint.position.z), Color.red, false, 0.001f);
		VRDebug.DrawLine(PlanePoint, new Vector3(CenterPoint.position.x, CartesianObject.position.y, CenterPoint.position.z), Color.green, false, 0.001f);
	}

	void DrawSpherical() {
		Vector3 Displacement = CartesianObject.position - CenterPoint.position;
		Vector3 SphericalCoords = CartesianToSpherical(Displacement);
		float XY_radius = Mathf.Sqrt(Displacement.x * Displacement.x + Displacement.z * Displacement.z);
		Vector2 ThetaPos = new Vector3(XY_radius * Mathf.Cos(SphericalCoords.y), XY_radius * Mathf.Sin(SphericalCoords.y));
		VRDebug.DrawLine(CenterPoint.position, CenterPoint.position + new Vector3(ThetaPos.x, 0f, ThetaPos.y), new Color(1f, 0f, 1f), false, 0.001f);

		Vector3 PhiPos = new Vector3(ThetaPos.x, SphericalCoords.x * Mathf.Cos(SphericalCoords.z), ThetaPos.y);

		VRDebug.DrawLine(CenterPoint.position, CenterPoint.position + PhiPos, new Color(1f, 1f, 0f), false, 0.001f);
	}

	Vector3 CartesianToSpherical(Vector3 Input) {
		float R = Mathf.Sqrt(Input.x * Input.x + Input.z * Input.z + Input.y * Input.y);
		float T = Mathf.Atan2(Input.z, Input.x);
		float P = Mathf.Atan2(Mathf.Sqrt(Input.x * Input.x + Input.z * Input.z), Input.y);
		return new Vector3(R, T, P);
	}

	Vector3 SphericalToCartesian(Vector3 Input) {
		float X = Input.x * Mathf.Cos(Input.y) * Mathf.Sin(Input.z);
		float Y = Input.x * Mathf.Sin(Input.y) * Mathf.Sin(Input.z);
		float Z = Input.x * Mathf.Cos(Input.z);
		return new Vector3(X, Z, Y);
	}
}
