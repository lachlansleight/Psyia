using UnityEngine;

namespace XRP
{
	public class XrpIntSliderGeometry : XrpControlGeometry
	{

		private Transform[] _intMarkers;

		public override void Awake()
		{
			base.Awake();
		}

		[ContextMenu("FixMarkerGeometry")]
		public override void FixGeometry()
		{
			base.FixGeometry();

			var intMarkerParent = transform.Find("Markers");
			for (var i = intMarkerParent.childCount - 1; i > 0; i--) {
				DestroyImmediate(intMarkerParent.GetChild(i).gameObject);
			}

			var parentSlider = transform.parent.GetComponent<XrpIntSlider>();
			var markerCount = parentSlider.MaxValue - parentSlider.MinValue + 1;
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
			
			var parentSize = transform.parent.localScale;
			var shortest = Mathf.Min(parentSize.x, parentSize.y);
			var sizeX = 0.05f * shortest / parentSize.x;

			var minX = 0.5f * (1f - 4f * sizeX);
			var maxX = -0.5f * (1f - 4f * sizeX);
			for (var i = 0; i < markerCount; i++) {
				var iScaled = parentSlider.MinValue + i;
				_intMarkers[i].localPosition = new Vector3(
					Mathf.Lerp(minX, maxX, Mathf.InverseLerp(parentSlider.MinValue, parentSlider.MaxValue, iScaled)), 
					_intMarkers[i].localPosition.y, 
					0f
				);
			}
		}
	}
}