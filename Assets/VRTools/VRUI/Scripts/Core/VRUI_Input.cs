using UnityEngine;
using System.Collections;

namespace VRTools {
	namespace UI {
		public class VRUI_Input : MonoBehaviour {

			public VRUI_Panel[] panels;
			public string deviceName = "ViveLeft";
			public string clickControlName = "Trigger";

			public VRUI_Control activeControl = null;

			VRInputDevice device;

			// Use this for initialization
			void Start () {
				panels = GameObject.FindObjectsOfType<VRUI_Panel>();
			}
	
			// Update is called once per frame
			void Update () {
				device = VRInput.GetDevice(deviceName);
				int currentClickStatus = GetClickStatus();

				//if we clicked down
				if(currentClickStatus == 2) {
					//find the closest object
					float closestValue = 1f;
					VRUI_Control tempControl = null;
					for(int i = 0; i < panels.Length; i++) {
						for(int j = 0; j < panels[i].controls.Length; j++) {
							if(panels[i].controls[j].GetScaledRange(device.position) < closestValue && panels[i].controls[j].gameObject.activeInHierarchy) {
								tempControl = panels[i].controls[j];
								closestValue = tempControl.GetScaledRange(device.position);
							}
						}
					}
					//if we found an object and it's within the active range, activate it
					if(tempControl != null && closestValue < 1f) {
						activeControl = tempControl;
						activeControl.MakeActive(device.position, this);
					}
				} else if(currentClickStatus == -1) {
					//clear the currently active control
					if(activeControl != null) {
						activeControl.MakeInactive(device.position, this);
						activeControl = null;
					}
				}

				for(int i = 0; i < panels.Length; i++) {
					//apply movement (i.e. update the values of the active control, if it exists, and update the hover states of all the other controls)
					panels[i].InputMoved(device.position, this);
				}
			}

			int GetClickStatus() {
				if(device.GetButtonDown(clickControlName)) return 2;
				else if(device.GetButtonUp(clickControlName)) return -1;
				else if(device.GetButton(clickControlName)) return 1;
				else return 0;
			}
		}
	}
}