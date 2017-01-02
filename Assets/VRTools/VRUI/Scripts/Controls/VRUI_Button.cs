using UnityEngine;
using System.Collections;


namespace VRTools
{
	namespace UI {

		public class VRUI_Button : VRUI_Control {
			private float yPosTarget = 0f;
			private float yPosLerpRate = 0.5f;

			public override void Active(Vector3 inputPos) {
				base.Active(inputPos);
			}
			
			public override void Update() {
				base.Update();

				if(base.currentState.Equals(VRUI_State.Down)) yPosTarget = -1f;
				else yPosTarget = 0f;
				float curYPos = ActiveElement.localPosition.y;
				ActiveElement.transform.localPosition = new Vector3(ActiveElement.transform.localPosition.x, Mathf.Lerp(curYPos, yPosTarget, yPosLerpRate), ActiveElement.localPosition.z);
			}
			
		}

	}
}