using UnityEngine;
using UnityEngine.UI;

namespace XRP
{
	public class XrpIntDial : XrpControl
	{

		public int MinValue = 0;
		public int MaxValue = 4;
		public int CurrentValue = 1;
		
		public IntDelegate OnValueChanged;
		public UnityIntEvent OnValueChangedEvent;

		private Transform _main;
		private float _currentDisplayValue;

		private Image _fillImage;

		public override void Awake()
		{
			base.Awake();
			
			_main = transform.Find("Geometry/Main");
			_fillImage = transform.Find("ActiveGeometry/FadePanel/Canvas/Image").GetComponent<Image>();
			_currentDisplayValue = CurrentValue;
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

			var valueLerp = Mathf.InverseLerp(MinValue, MaxValue, _currentDisplayValue);
			_main.localRotation = Quaternion.Euler(Mathf.Lerp(150f, -150f, valueLerp), 90f, -90f);
			_fillImage.fillAmount = valueLerp;
		}
		
		public override void StopPress()
		{
			base.StopPress();

			var preValue = CurrentValue;
			var remainder = _currentDisplayValue % 1f;
			remainder = QuinticLerpInOut(remainder);
			_currentDisplayValue = Mathf.Floor(_currentDisplayValue) + remainder;

			var currentActualValue = Mathf.RoundToInt(_currentDisplayValue);

			if (currentActualValue != preValue) {
				CurrentValue = currentActualValue;
				OnValueChanged?.Invoke(CurrentValue);
				OnValueChangedEvent.Invoke(CurrentValue);
			}
		}
		
		protected override void DoPress()
		{
			base.DoPress();
			if (CurrentState != State.Press) return;
			
			/*
			FadePanel.localScale = new Vector3(
				Mathf.InverseLerp(MinValue, MaxValue, CurrentValue),
				FadePanel.localScale.y, 
				FadePanel.localScale.z
			);
			
			FadePanel.localPosition = new Vector3(
				Mathf.Lerp(0.5f, 0f, Mathf.InverseLerp(MinValue, MaxValue, CurrentValue)),
				FadePanel.localPosition.y,
				FadePanel.localPosition.z
			);
			*/

			var localPoint = transform.InverseTransformPoint(ActivePointer.transform.position);
			var pointAngle = Mathf.Atan2(-localPoint.x, localPoint.y) * Mathf.Rad2Deg;
			var pointRadius = new Vector2(localPoint.x, localPoint.y).magnitude * 2f;

			_fillImage.transform.parent.parent.localScale = Vector3.one * Mathf.Max(1f, pointRadius);
			
			var preValue = CurrentValue;
			_currentDisplayValue = Mathf.Lerp(MinValue, MaxValue, Mathf.InverseLerp(-150f, 150f, pointAngle));
			var remainder = _currentDisplayValue % 1f;
			remainder = QuinticLerpInOut(remainder);
			_currentDisplayValue = Mathf.Floor(_currentDisplayValue) + remainder;

			var currentActualValue = Mathf.RoundToInt(_currentDisplayValue);

			if (currentActualValue != preValue) {
				CurrentValue = currentActualValue;
				OnValueChanged?.Invoke(CurrentValue);
				OnValueChangedEvent.Invoke(CurrentValue);
			}
		}
		
		[ContextMenu("FixMarkerGeometry")]
		public void FixMarkerGeometry()
		{
			GetComponentInChildren<XrpIntDialGeometry>().FixGeometry();
		}
		
		private float QuinticLerpInOut(float t)
		{
			if (t < 0.5f) return 16f * t * t * t * t * t;
			t--;
			return 16f * t * t * t * t * t + 1f;
		}
	}
}