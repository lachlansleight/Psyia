using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

	public Vector3 RotationAxes;
	public float RotationSpeed;

	public void Start()
	{
		if (RotationAxes == Vector3.zero) RotationAxes = Vector3.up;
	}
	
	public void Update()
	{
		transform.Rotate(RotationAxes, RotationSpeed * Time.deltaTime);
	}
}
