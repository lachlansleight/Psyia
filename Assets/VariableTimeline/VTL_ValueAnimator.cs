using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace Foliar.VTL {

	[System.Serializable]
	public class VTL_ValueAnimator : VTL_Action {

		public float Duration;
		public Object TargetObject;
		public FieldInfo TargetField;
		public AnimationCurve ValueCurve;

		public VTL_ValueAnimator() {
			base.Name = "New Value Animator";
			Duration = 5f;
			ValueCurve = AnimationCurve.Linear(0, 0, 1, 1);
		}

	}
}