using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XRP
{

	public class XrpDial : XrpControl
	{

		public float MinValue = 0f;
		public float MaxValue = 1f;
		public float CurrentValue = 0.5f;


		public FloatDelegate OnValueChanged;
		public UnityFloatEvent OnValueChangedEvent;

		private Image _fillImage;
		private Transform _main;

		public override void Awake()
		{
			base.Awake();
			
			_main = transform.Find("Geometry/Main");
			_fillImage = transform.Find("ActiveGeometry/FadePanel/Canvas/Image").GetComponent<Image>();
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

			var valueLerp = Mathf.InverseLerp(MinValue, MaxValue, CurrentValue);
			_main.localRotation = Quaternion.Euler(Mathf.Lerp(145f, -145f, valueLerp), 90f, -90f);
			_fillImage.fillAmount = valueLerp;
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
			CurrentValue = Mathf.Lerp(MinValue, MaxValue, Mathf.InverseLerp(-145f, 145f, pointAngle));

			if (Mathf.Abs(CurrentValue - preValue) > float.MinValue) {
				OnValueChanged?.Invoke(CurrentValue);
				OnValueChangedEvent.Invoke(CurrentValue);
			}
		}
	}

}