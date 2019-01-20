using UnityEngine;

namespace XRP
{
	public class XrpEnumSliderGeometry : XrpControlGeometry
	{

		private Transform[] _intMarkers;
		private Transform _mainBar;

		public override void Awake()
		{
			base.Awake();
		}

		protected override void FetchGeometry()
		{
			base.FetchGeometry();

			_mainBar = transform.Find("Main/Slider");
		}
		
		[ContextMenu("FixMarkerGeometry")]
		public override void FixGeometry()
		{
			base.FixGeometry();

			var intMarkerParent = transform.Find("Markers");
			for (var i = intMarkerParent.childCount - 1; i > 0; i--) {
				DestroyImmediate(intMarkerParent.GetChild(i).gameObject);
			}

			var parentSlider = transform.parent.GetComponent<XrpEnumSlider>();
			var markerCount = parentSlider.MaxValue - parentSlider.MinValue + 2;
			_intMarkers = new Transform[markerCount];
			var templateMarker = intMarkerParent.GetChild(0).gameObject;
			templateMarker.transform.localScale = new Vector3(
				templateMarker.transform.localScale.x,
				_main.localScale.y,
				templateMarker.transform.localScale.z
			);
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

			var minX = 0.5f * (1f - 4f * sizeX) - _intMarkers[0].localScale.x * 0.5f;
			var maxX = -0.5f * (1f - 4f * sizeX) + _intMarkers[0].localScale.x * 0.5f;
			for (var i = 0; i < markerCount; i++) {
				var iScaled = parentSlider.MinValue + i;
				_intMarkers[i].localPosition = new Vector3(
					Mathf.Lerp(minX, maxX, Mathf.InverseLerp(parentSlider.MinValue, parentSlider.MaxValue + 1, iScaled)),
					0f,
					0f
				);
			}

			var barWidth = _intMarkers[0].localScale.x / _mainBar.parent.localScale.x;
			var mainBarWidth = (1f / (parentSlider.MaxValue - parentSlider.MinValue + 1));
			_mainBar.localScale = new Vector3(
				mainBarWidth - barWidth * 3f,
				0.9f,
				1.5f
			);

			parentSlider.SliderMinX = -0.5f + _mainBar.localScale.x * 0.5f + barWidth * 1.5f;
			parentSlider.SliderMaxX = 0.5f - _mainBar.localScale.x * 0.5f - barWidth * 1.5f;
		}
	}
}