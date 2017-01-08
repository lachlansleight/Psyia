using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace VRTools
{
	namespace UI {

		public class VRUI_Panel : MonoBehaviour
		{

			public VRUI_Control[] controls;
			private bool gotControls = false;
			//public VRUI_Control activeControl;

			// Use this for initialization
			void Start()
			{
				//build control array
				if(!gotControls) BuildControls();
				//for(int i = 0; i < controls.Length; i++) controls[i].OnHapticPulse += PulseViveHaptics;
			}

			void BuildControls() {
				System.Collections.Generic.List<VRUI_Control> controlList = new System.Collections.Generic.List<VRUI_Control>();
				GetControlsInChildren(transform, ref controlList);
				controls = controlList.ToArray();

				//for(int i = 0; i < controls.Length; i++) controls[i].Bang();

				gotControls = true;
			}

			void GetControlsInChildren(Transform parent, ref System.Collections.Generic.List<VRUI_Control> controlList) {
				VRUI_Control myControl = parent.GetComponent<VRUI_Control>();
				if(myControl != null) controlList.Add(myControl);
				else if(parent.childCount > 0) {
					for(int i = 0; i < parent.childCount; i++) {
						GetControlsInChildren(parent.GetChild(i), ref controlList);
					}
				}
			}

			/*
			// Update is called once per frame
			void Update()
			{	
				VRTools.VRInputDevice device = VRTools.VRInput.GetDevice("ViveRight");
				for(int i = 0; i < controls.Length; i++) {
					controls[i].Active(device.position, GetClickStatus(device, "Trigger"));
				}
				

				
				device = VRTools.VRInput.GetDevice("ViveLeft");
				for(int i = 0; i < controls.Length; i++) {
					controls[i].Active(device.position, GetClickStatus(device, "Trigger"));
				}
				
			}
			*/	

			public void Active(Vector3 position, int clickStatus) {

			}

			public void InputMoved(Vector3 position, VRUI_Input inputDevice) {
				for(int i = 0; i < controls.Length; i++) {
					if(controls[i].gameObject.activeSelf) controls[i].InputMoved(position, inputDevice);
				}
			}

			int GetClickStatus(VRTools.VRInputDevice device, string buttonName) {
				if(device.GetButtonDown(buttonName)) return 2;
				else if(device.GetButtonUp(buttonName)) return -1;
				else if(device.GetButton(buttonName)) return 1;
				else return 0;
			}

			void PulseViveHaptics(Vector3 pos, float intensity) {
				//if((VRInput.GetDevice("ViveRight").position - pos).magnitude < (VRInput.GetDevice("ViveLeft").position - pos).magnitude) {
					//VRInput.GetDevice("ViveRight").PulseHaptic("Main", intensity);
				//} else {
				//	VRInput.GetDevice("ViveLeft").PulseHaptic("Main", intensity);
				//}
			}

			public VRUI_Control GetControl(string name) {
				if(!gotControls) BuildControls();

				if(controls == null) {
					Debug.LogError("Error - controls array not initialised");
					return null;
				}
				for(int i = 0; i < controls.Length; i++) {
					//Debug.Log(controls[i].name + " compared to search string " + name);
					if(controls[i].gameObject.name.Equals(name)) {
						return controls[i];
					}
				}
				Debug.LogError("Error - didn't find a control with name " + name);
				return null;
			}
		}

	}
}
