using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PrettyScreenCamera : MonoBehaviour
{

	public Transform HeadTransform;
	[Range(0f, 1f)] public float CameraLerpRate = 0.2f;
	public string ToggleKey = "v";

	private Camera _camera;
	
	public void Start()
	{
		_camera = GetComponent<Camera>();
		_camera.enabled = false;
	}

	public void Update()
	{
		if(_camera == null) _camera = GetComponent<Camera>();
		
		if (Input.GetKeyDown(ToggleKey)) {
			_camera.enabled = !_camera.enabled;
		}
		
		transform.position = Vector3.Lerp(transform.position, HeadTransform.position, CameraLerpRate);
		var rotation = Quaternion.Lerp(transform.rotation, HeadTransform.rotation, CameraLerpRate).eulerAngles;
		rotation.z = 0f;
		transform.rotation = Quaternion.Euler(rotation);
	}
}
