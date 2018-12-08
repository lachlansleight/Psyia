using UnityEngine;

namespace XRP
{
	public class XrpSlider : XrpControl
	{
		
		public float MinValue = 0f;
		public float MaxValue = 1f;
		public float CurrentValue = 0.5f;
		
		public FloatDelegate OnValueChanged;
		public UnityFloatEvent OnValueChangedEvent;

		private Transform _sliderGeometry;


		public override void Awake()
		{
			base.Awake();

			_sliderGeometry = transform.Find("Geometry/Main/Slider");
		}
		
		public override void Start()
		{
			if (ThrowEventOnStart) {
				OnValueChangedEvent.Invoke(CurrentValue);
				OnValueChanged?.Invoke(CurrentValue);
			}
		}

		public override void Update()
		{
			base.Update();
			SetSliderGeometry();
		}

		private void SetSliderGeometry()
		{
			var sliderLerpValue = Mathf.InverseLerp(MinValue, MaxValue, CurrentValue);
			
			//find shortest side - that side x 0.05 = width of border geometry and width of padding
			var shortest = Mathf.Min(transform.localScale.x, transform.localScale.y);
			var gapX = 0.05f * shortest / transform.localScale.x;
			var gapY = 0.05f * shortest / transform.localScale.y;

			var maxSize = 1f - (gapX * 2f);
			var xScale = Mathf.Lerp(0f, maxSize, sliderLerpValue);
			var yScale = 1f - (gapY * 2f);

			_sliderGeometry.localScale = new Vector3(xScale, yScale, 1.5f);

			_sliderGeometry.localPosition = new Vector3(Mathf.Lerp(-maxSize / 2f, 0f, sliderLerpValue), 0f, 0f);
		}

		protected override void DoPress()
		{
			base.DoPress();
			if (CurrentState != State.Press) return;
			
			var localPoint = transform.InverseTransformPoint(ActivePointer.transform.position);
			
			FadePanel.localScale = new Vector3(
				Mathf.InverseLerp(MinValue, MaxValue, CurrentValue),
				Mathf.Max(1f, Mathf.Abs(localPoint.y) * 2f),
				FadePanel.localScale.z
			);
			
			FadePanel.localPosition = new Vector3(
				Mathf.Lerp(0.5f, 0f, Mathf.InverseLerp(MinValue, MaxValue, CurrentValue)),
				FadePanel.localPosition.y,
				FadePanel.localPosition.z
			);
			
			var preValue = CurrentValue;
			CurrentValue = Mathf.Lerp(MinValue, MaxValue, Mathf.InverseLerp(0.5f, -0.5f, localPoint.x));

			if (Mathf.Abs(CurrentValue - preValue) > float.MinValue) {
				OnValueChanged?.Invoke(CurrentValue);
				OnValueChangedEvent.Invoke(CurrentValue);
			}
		}
	}
}