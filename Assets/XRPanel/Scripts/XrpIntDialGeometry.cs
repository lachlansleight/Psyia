using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRP;

public class XrpIntDialGeometry : XrpControlGeometry {

	private Transform[] _intMarkers;
	
	public override void Awake()
	{
		FixGeometry();
	}
	
	[ContextMenu("FixMarkerGeometry")]
	public override void FixGeometry()
	{
		var intMarkerParent = transform.Find("Markers");
		for (var i = intMarkerParent.childCount - 1; i > 0; i--) {
			DestroyImmediate(intMarkerParent.GetChild(i).gameObject);
		}

		var parentDial = transform.parent.GetComponent<XrpIntDial>();
		var markerCount = parentDial.MaxValue - parentDial.MinValue + 1;
		_intMarkers = new Transform[markerCount];
		var templateMarker = intMarkerParent.GetChild(0).gameObject;
		_intMarkers[0] = templateMarker.transform;
		for (var i = 1; i < markerCount; i++) {
			var newObj = Instantiate(templateMarker);
			newObj.transform.parent = intMarkerParent;
			newObj.transform.localPosition = templateMarker.transform.localPosition;
			newObj.transform.localRotation = templateMarker.transform.localRotation;
			newObj.transform.localScale = templateMarker.transform.localScale;
			_intMarkers[i] = newObj.transform;
		}

		var radius = 0.5f + templateMarker.transform.localScale.y * 0.4f;

		for (var i = 0; i < markerCount; i++) {
			var iScaled = parentDial.MinValue + i;
			var iLerp = Mathf.InverseLerp(parentDial.MinValue, parentDial.MaxValue, iScaled);
			var iTheta = Mathf.Lerp(-55f, 235f, iLerp) * Mathf.Deg2Rad;
			//iTheta = 0;
			_intMarkers[i].localPosition = new Vector3(
				radius * Mathf.Cos(iTheta),
				radius * Mathf.Sin(iTheta),
				_intMarkers[i].localPosition.z
			);
			_intMarkers[i].localEulerAngles = new Vector3(
				_intMarkers[i].localEulerAngles.x,
				_intMarkers[i].localEulerAngles.y,
				iTheta * Mathf.Rad2Deg * -1 + 90f
			);
		}
	}
}
