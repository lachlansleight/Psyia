using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerLine : MonoBehaviour
{


	public int PointCount = 20;
	public PsyiaController Controller;
	
	private LineRenderer _lineRenderer;

	public void Awake()
	{
		_lineRenderer = GetComponent<LineRenderer>();
	}
	
	public void Update()
	{
		if (Controller.ControllerDistance <= 0f) {
			_lineRenderer.positionCount = 0;
			return;
		}

		if (!Controller.gameObject.activeSelf || !Controller.enabled) {
			_lineRenderer.positionCount = 0;
			return;
		}

		_lineRenderer.positionCount = PointCount;

		var pointA = transform.position;
		var pointB = transform.position + transform.forward * Controller.ControllerDistance * 0.5f;
		var pointC = Controller.transform.position + Controller.transform.forward * -0.04388f;

		for (var i = 0; i < PointCount; i++) {
			var iF = (float) i / (PointCount - 1);
			iF = Mathf.Lerp(0.1f, 0.9f, iF);
			_lineRenderer.SetPosition(i, QuadraticBezier(pointA, pointB, pointC, iF));
		}
	}

	private Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
	{
		return Vector3.Lerp(Vector3.Lerp(a, b, t), Vector3.Lerp(b, c, t), t);
	}
}
