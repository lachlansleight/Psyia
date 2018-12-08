using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

namespace XRP
{

	[RequireComponent(typeof(BoxCollider))]
	public class XrpButton : XrpControl
	{
		public UnityEvent OnClickEvent;
		public EmptyDelegate OnClick;

		public override void Start()
		{
			if (ThrowEventOnStart) {
				OnClickEvent.Invoke();
				OnClick?.Invoke();
			}
		}

		public override void Update()
		{
			base.Update();
			
			if (ActivePointer == null) return;
			
			if (CurrentState == State.Disabled) CheckReEnable();
		}

		

		private void ShowDebugPointer()
		{
			if (ActivePointer != null) {
				Debug.DrawLine(transform.position, ActivePointer.transform.position, Color.red);
			}
		}

		private void CheckReEnable()
		{
			var pointerPos = ActivePointer.transform.position;
			var localPos = transform.InverseTransformPoint(pointerPos);
			if (localPos.z < Panel.TouchDistance) return;


			StopPress();
		}
		

		protected override void DoPress()
		{
			base.DoPress();
			Trigger();
		}

		public void Trigger()
		{
			OnClickEvent.Invoke();
			OnClick?.Invoke();

			PopFadePanel();
			
			CurrentState = State.Disabled;
			AudioSource.PlayClipAtPoint(Panel.PressClip, transform.position, 0.3f);
		}
	}

}