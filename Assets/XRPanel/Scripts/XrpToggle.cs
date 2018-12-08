using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRP
{

	public class XrpToggle : XrpControl
	{

		public bool Toggled = false;

		public BoolDelegate OnValueChanged;
		public UnityBoolEvent OnValueChangedEvent;

		private Transform _main;

		public override void Awake()
		{
			base.Awake();
			
			_main = transform.Find("Geometry/Main");
		}
		
		public override void Start()
		{
			if (ThrowEventOnStart) {
				OnValueChangedEvent.Invoke(Toggled);
				OnValueChanged?.Invoke(Toggled);
			}
		}
		
		public override void Update()
		{
			base.Update();

			var newScale = Vector3.one * Mathf.Lerp(_main.localScale.x, Toggled ? 0.8f : 0.1f, 0.1f);
			newScale.y = 0.04f;
			_main.localScale = newScale;
			
			
			if (CurrentState == State.Disabled) CheckReEnable();
		}
		
		protected override void DoPress()
		{
			base.DoPress();
			Toggle();
		}
		
		private void CheckReEnable()
		{
			var pointerPos = ActivePointer.transform.position;
			var localPos = transform.InverseTransformPoint(pointerPos);
			if (localPos.z < Panel.TouchDistance) return;


			StopPress();
		}

		private void Toggle()
		{
			Toggled = !Toggled;
			OnValueChanged?.Invoke(Toggled);
			OnValueChangedEvent.Invoke(Toggled);
			
			PopFadePanel();
			
			CurrentState = State.Disabled;
			AudioSource.PlayClipAtPoint(Panel.PressClip, transform.position, 0.3f);
		}
	}

}