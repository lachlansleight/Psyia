using UnityEngine;
using System.Collections;
using Valve.VR;
using UnityEngine.VR;

namespace VRTools {

	/// <summary>
	/// Vive helper
	/// Utility component - should be placed on the [CameraRig] prefab
	/// Makes external syntax for handling vive inputs easier
	/// </summary>
	public class ViveInput : MonoBehaviour {
		
		#if UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0 || UNITY_4
		/// <summary>
		/// Assign the right controller object from the Vive CameraRig prefab
		/// </summary>
		public SteamVR_TrackedObject hmd;
		#else
		public Transform hmd;
		private Vector3 lastPosition;
		private Quaternion lastRotation;
		#endif
		/// <summary>
		/// Assign the left controller object from the Vive CameraRig prefab
		/// </summary>
		public SteamVR_TrackedObject Left;
		/// <summary>
		/// Assign the right controller object from the Vive CameraRig prefab
		/// </summary>
		public SteamVR_TrackedObject Right;
	
		private SteamVR_Controller.Device deviceLeft;
		private SteamVR_Controller.Device deviceRight;
		private SteamVR_Controller.Device deviceHMD;

		private Transform nativeHMD;

		private Vector2 lastLeftTouchpad;
		private Vector2 lastRightTouchpad;

		void Awake () {
			#if UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0 || UNITY_4
				if(hmd == null) {
					if(transform.FindChild("Camera (head)") != null) hmd = transform.FindChild("Camera (head)").GetComponent<SteamVR_TrackedObject>();
					if(hmd == null) {
						hmd = GameObject.Find("Camera (head)").GetComponent<SteamVR_TrackedObject>();
						if(hmd == null) {
							Debug.LogError("Error - couldn't find gameobject with name 'Camera (head)' for the HMD on ViveInput!");
						}
					}
				}
			#else
			//we don't need to reference the vive tracked object here because we have it from the native unity implementation
				if(hmd == null) {
					if(transform.FindChild("Camera (eye)") != null) hmd = transform.FindChild("Camera (eye)");
					if(hmd == null) {
						hmd = GameObject.Find("Camera (eye)").transform;
						if(hmd == null) {
							Debug.LogError("Error - couldn't find gameobject with name 'Camera (eye)' for the HMD on ViveInput!");
						}
					}
				}
			#endif
			if(Left == null) {
				if(transform.FindChild("Controller (left)") != null) Left = transform.FindChild("Controller (left)").GetComponent<SteamVR_TrackedObject>();
				if(Left == null) {
					Left = GameObject.Find("Controller (left)").GetComponent<SteamVR_TrackedObject>();
					if(Left == null) {
						Debug.LogError("Error - couldn't find gameobject with name 'Controller (left)' for the Left Controller on ViveInput!");
					}
				}
			}
			if(Right == null) {
				if(transform.FindChild("Controller (right)") != null) Right = transform.FindChild("Controller (right)").GetComponent<SteamVR_TrackedObject>();
				if(Right == null) {
					Right = GameObject.Find("Controller (right)").GetComponent<SteamVR_TrackedObject>();
					if(Right == null) {
						Debug.LogError("Error - couldn't find gameobject with name 'Controller (right)' for the Right Controller on ViveInput!");
					}
				}
			}

			if(hmd == null || Left == null || Right == null) {
				Debug.LogError("One or more Vive devices couldn't be found - ViveInput will not function. :(");
			}

			//ViveInput.Left = new ControllerState();
			//ViveInput.Right = new ControllerState();
			//ViveInput.HMD = new ControllerState();

			VRInput.AddVive();

			//StartCoroutine(GetChaperoneData());

			//StartCoroutine(GetChaperoneData());
		}
		
		string formatFloat(float input, int sigfigs, bool addPlus) {
			string temp = "" + Mathf.Round (input * Mathf.Pow (10, sigfigs)) / Mathf.Pow (10, sigfigs);
			if(input >= 0 && addPlus) temp = "+" + temp;
			while(temp.Length <= sigfigs + 1) {
				temp = temp + "0";
			}
			return temp;
		}
		
		void Update () {
			//if we don't have one of these, something is wrong, and we shouldn't do anything
			if(hmd == null || Left == null || Right == null) return;

			//If we have the left controller
			if(Left.isValid) {
				deviceLeft = SteamVR_Controller.Input ((int)Left.index);

				VRInputDevice tempDevice = VRInput.GetDevice("ViveLeft");

				tempDevice.isTracked = true;
				tempDevice.SetInt("Index", (int)Left.index);
				tempDevice.transform = Left.transform;

				tempDevice.position = Left.transform.position;
				tempDevice.forward = Left.transform.forward;
				tempDevice.up = Left.transform.up;
				tempDevice.right = Left.transform.right;
				tempDevice.velocity = deviceLeft.velocity;
				tempDevice.rotation = Left.transform.rotation;
				tempDevice.angularVelocity = deviceLeft.angularVelocity;

				tempDevice.SetButton("Trigger", GetButtonInt(deviceLeft, EVRButtonId.k_EButton_SteamVR_Trigger));
				tempDevice.SetButton("Menu", GetButtonInt(deviceLeft, EVRButtonId.k_EButton_ApplicationMenu));
				tempDevice.SetButton("Touchpad", GetButtonInt(deviceLeft, EVRButtonId.k_EButton_SteamVR_Touchpad));
				tempDevice.SetButton("Grip", GetButtonInt(deviceLeft, EVRButtonId.k_EButton_Grip));

				tempDevice.SetTouchButton("Touchpad", GetTouchButtonInt(deviceLeft, EVRButtonId.k_EButton_SteamVR_Touchpad));

				Vector2 touchpadVector = deviceLeft.GetAxis (EVRButtonId.k_EButton_SteamVR_Touchpad);
				tempDevice.SetAxis("TouchpadX", touchpadVector.x);
				tempDevice.SetAxis("TouchpadY", touchpadVector.y);
				tempDevice.SetAxis("TouchpadR", Mathf.Sqrt(touchpadVector.x * touchpadVector.x + touchpadVector.y * touchpadVector.y));
				tempDevice.SetAxis("TouchpadT", Mathf.Atan2(touchpadVector.y, touchpadVector.x) * Mathf.Rad2Deg);
				tempDevice.SetAxis("Trigger", deviceLeft.GetAxis (EVRButtonId.k_EButton_SteamVR_Trigger).x);

				tempDevice.Vibrate();

				/*
				ViveInput.Left.IsTracked = true;
				ViveInput.Left.Index = (int)Left.index;
				ViveInput.Left.Transform = Left.transform;
				
				ViveInput.Left.Position = Left.transform.position;//deviceLeft.transform.pos;
				ViveInput.Left.Forward = Left.transform.forward;
				ViveInput.Left.Up = Left.transform.up;
				ViveInput.Left.Right = Left.transform.right;
				ViveInput.Left.Velocity = deviceLeft.velocity;
				ViveInput.Left.Rotation = Left.transform.rotation;//deviceLeft.transform.rot;
				ViveInput.Left.AngularVelocity = deviceLeft.angularVelocity;
				
				
				ViveInput.Left.Buttons.Trigger = getButtonState(deviceLeft, EVRButtonId.k_EButton_SteamVR_Trigger);
				ViveInput.Left.TriggerAxis = deviceLeft.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
				ViveInput.Left.Buttons.Grip = getButtonState(deviceLeft, EVRButtonId.k_EButton_Grip);
				ViveInput.Left.Buttons.TouchpadButton = getButtonState (deviceLeft, EVRButtonId.k_EButton_SteamVR_Touchpad);
				ViveInput.Left.Buttons.Menu = getButtonState (deviceLeft, EVRButtonId.k_EButton_ApplicationMenu);
				
				ViveInput.Left.TouchpadAxis = deviceLeft.GetAxis (EVRButtonId.k_EButton_SteamVR_Touchpad);
				if(ViveInput.Left.Buttons.TouchpadButton.Equals(ButtonState.Touched) || ViveInput.Left.Buttons.TouchpadButton.Equals(ButtonState.Pressed)) {
					ViveInput.Left.TouchpadDeltaX = (ViveInput.Left.TouchpadAxis.x - lastLeftTouchpad.x) * Time.deltaTime;
					ViveInput.Left.TouchpadDeltaY = (ViveInput.Left.TouchpadAxis.y - lastLeftTouchpad.y) * Time.deltaTime;
					ViveInput.Left.TouchpadDeltaR = ((ViveInput.Left.TouchpadAxis).magnitude - (lastLeftTouchpad).magnitude) * Time.deltaTime;
					ViveInput.Left.TouchpadDeltaT = getDeltaAngle(ViveInput.Left.TouchpadAxis, lastLeftTouchpad) * Time.deltaTime;
				} else {
					ViveInput.Left.TouchpadDeltaX = ViveInput.Left.TouchpadDeltaY = ViveInput.Left.TouchpadDeltaR = ViveInput.Left.TouchpadDeltaT = 0f;
				}
				ViveInput.Left.TouchpadDir = getTouchpadDir(ViveInput.Left.TouchpadAxis, ViveInput.Left.Buttons.TouchpadButton);

				lastLeftTouchpad = ViveInput.Left.TouchpadAxis;
				*/
			} else {
				VRInput.GetDevice("ViveLeft").isTracked = false;
				deviceLeft = null;
				//deviceLeft = null;
				//ViveInput.Left.IsTracked = false;
			}
			//If we have the right controller
			if(Right.isValid) {
				deviceRight = SteamVR_Controller.Input ((int)Right.index);

				VRInputDevice tempDevice = VRInput.GetDevice("ViveRight");

				tempDevice.isTracked = true;
				tempDevice.SetInt("Index", (int)Right.index);
				tempDevice.transform = Right.transform;

				tempDevice.position = Right.transform.position;
				tempDevice.forward = Right.transform.forward;
				tempDevice.up = Right.transform.up;
				tempDevice.right = Right.transform.right;
				tempDevice.velocity = deviceRight.velocity;
				tempDevice.rotation = Right.transform.rotation;
				tempDevice.angularVelocity = deviceRight.angularVelocity;

				tempDevice.SetButton("Trigger", GetButtonInt(deviceRight, EVRButtonId.k_EButton_SteamVR_Trigger));
				tempDevice.SetButton("Menu", GetButtonInt(deviceRight, EVRButtonId.k_EButton_ApplicationMenu));
				tempDevice.SetButton("Touchpad", GetButtonInt(deviceRight, EVRButtonId.k_EButton_SteamVR_Touchpad));
				tempDevice.SetButton("Grip", GetButtonInt(deviceRight, EVRButtonId.k_EButton_Grip));

				tempDevice.SetTouchButton("Touchpad", GetTouchButtonInt(deviceRight, EVRButtonId.k_EButton_SteamVR_Touchpad));

				Vector2 touchpadVector = deviceRight.GetAxis (EVRButtonId.k_EButton_SteamVR_Touchpad);
				tempDevice.SetAxis("TouchpadX", touchpadVector.x);
				tempDevice.SetAxis("TouchpadY", touchpadVector.y);
				tempDevice.SetAxis("TouchpadR", Mathf.Sqrt(touchpadVector.x * touchpadVector.x + touchpadVector.y * touchpadVector.y));
				tempDevice.SetAxis("TouchpadT", Mathf.Atan2(touchpadVector.y, touchpadVector.x) * Mathf.Rad2Deg);
				tempDevice.SetAxis("Trigger", deviceRight.GetAxis (EVRButtonId.k_EButton_SteamVR_Trigger).x);

				tempDevice.Vibrate();

				/*
				ViveInput.Right.IsTracked = true;
				ViveInput.Right.Index = (int)Right.index;
				ViveInput.Right.Transform = Right.transform;

				ViveInput.Right.Position = Right.transform.position;//deviceRight.transform.pos;
				ViveInput.Right.Forward = Right.transform.forward;
				ViveInput.Right.Up = Right.transform.up;
				ViveInput.Right.Right = Right.transform.right;
				ViveInput.Right.Velocity = deviceRight.velocity;
				ViveInput.Right.Rotation = Right.transform.rotation;//deviceRight.transform.rot;
				ViveInput.Right.AngularVelocity = deviceRight.angularVelocity;

				ViveInput.Right.Buttons.Trigger = getButtonState(deviceRight, EVRButtonId.k_EButton_SteamVR_Trigger);
				ViveInput.Right.TriggerAxis = deviceRight.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
				ViveInput.Right.Buttons.Grip = getButtonState(deviceRight, EVRButtonId.k_EButton_Grip);
				ViveInput.Right.Buttons.TouchpadButton = getButtonState (deviceRight, EVRButtonId.k_EButton_SteamVR_Touchpad);
				ViveInput.Right.Buttons.Menu = getButtonState (deviceRight, EVRButtonId.k_EButton_ApplicationMenu);
				
				ViveInput.Right.TouchpadAxis = deviceRight.GetAxis (EVRButtonId.k_EButton_SteamVR_Touchpad);
				if(ViveInput.Right.Buttons.TouchpadButton.Equals(ButtonState.Touched) || ViveInput.Right.Buttons.TouchpadButton.Equals(ButtonState.Pressed)) {
					ViveInput.Right.TouchpadDeltaX = (ViveInput.Right.TouchpadAxis.x - lastRightTouchpad.x) * Time.deltaTime;
					ViveInput.Right.TouchpadDeltaY = (ViveInput.Right.TouchpadAxis.y - lastRightTouchpad.y) * Time.deltaTime;
					ViveInput.Right.TouchpadDeltaR = ((ViveInput.Right.TouchpadAxis).magnitude - (lastRightTouchpad).magnitude) * Time.deltaTime;
					ViveInput.Right.TouchpadDeltaT = getDeltaAngle(ViveInput.Right.TouchpadAxis, lastRightTouchpad) * Time.deltaTime;
				} else {
					ViveInput.Right.TouchpadDeltaX = ViveInput.Right.TouchpadDeltaY = ViveInput.Right.TouchpadDeltaR = ViveInput.Right.TouchpadDeltaT = 0f;
				}
				ViveInput.Right.TouchpadDir = getTouchpadDir(ViveInput.Right.TouchpadAxis, ViveInput.Right.Buttons.TouchpadButton);

				lastRightTouchpad = ViveInput.Right.TouchpadAxis;
				*/
			} else {
				VRInput.GetDevice("ViveRight").isTracked = false;
				deviceRight = null;
				//ViveInput.Right.IsTracked = false;
			}
			#if UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0 || UNITY_4
			//If we have the HMD
			if(hmd.isValid) {
				deviceHMD = SteamVR_Controller.Input ((int)hmd.index);

				VRInputDevice tempDevice = VRInput.GetDevice("ViveHMD");

				tempDevice.isTracked = true;
				tempDevice.SetInt("Index", (int)hmd.index);
				tempDevice.transform = hmd.transform;

				tempDevice.position = hmd.transform.position;
				tempDevice.forward = hmd.transform.forward;
				tempDevice.up = hmd.transform.up;
				tempDevice.right = hmd.transform.right;
				tempDevice.velocity = deviceHMD.velocity;
				tempDevice.rotation = hmd.transform.rotation;
				tempDevice.angularVelocity = deviceHMD.angularVelocity;

				/*
				ViveInput.HMD.IsTracked = true;
				ViveInput.HMD.Index = (int)hmd.index;
				ViveInput.HMD.Transform = hmd.transform;

				ViveInput.HMD.Position = hmd.transform.position;
				ViveInput.HMD.Forward = hmd.transform.forward;
				ViveInput.HMD.Up = hmd.transform.up;
				ViveInput.HMD.Right = hmd.transform.right;
				ViveInput.HMD.Velocity = deviceHMD.velocity;
				ViveInput.HMD.Rotation = deviceHMD.transform.rot;
				ViveInput.HMD.AngularVelocity = deviceHMD.angularVelocity;
				
				ViveInput.HMD.Buttons.Trigger = ButtonState.Inactive;
				ViveInput.HMD.Buttons.Grip = ButtonState.Inactive;
				ViveInput.HMD.Buttons.TouchpadButton = ButtonState.Inactive;
				ViveInput.HMD.Buttons.Menu = ButtonState.Inactive;
				
				ViveInput.HMD.TouchpadAxis = Vector3.zero;
				*/
			} else {
				VRInput.GetDevice("ViveHMD").isTracked = false;
				deviceHMD = null;
				//ViveInput.HMD.IsTracked = false;
			}
			#else
			if(VRDevice.isPresent) {
				VRInputDevice tempDevice = VRInput.GetDevice("ViveHMD");

				tempDevice.isTracked = true;
				tempDevice.transform = hmd;

				tempDevice.position = hmd.position;
				tempDevice.forward = hmd.forward;
				tempDevice.up = hmd.up;
				tempDevice.right = hmd.right;
				tempDevice.rotation = hmd.transform.rotation;

				tempDevice.velocity = (hmd.position - lastPosition) / Time.deltaTime;
				tempDevice.angularVelocity = (hmd.rotation * Quaternion.Inverse(lastRotation)).eulerAngles / Time.deltaTime;

				lastPosition = hmd.position;
				lastRotation = hmd.rotation;

				tempDevice.SetString("Model", VRDevice.model);
				tempDevice.SetFloat("RefreshRate", VRDevice.refreshRate);

				tempDevice.SetVector3("Head", InputTracking.GetLocalPosition(VRNode.Head));
				tempDevice.SetVector3("LeftEye", InputTracking.GetLocalPosition(VRNode.LeftEye));
				tempDevice.SetVector3("RightEye", InputTracking.GetLocalPosition(VRNode.RightEye));
				tempDevice.SetVector3("CenterEye", InputTracking.GetLocalPosition(VRNode.CenterEye));

				tempDevice.SetQuaternion("Head", InputTracking.GetLocalRotation(VRNode.Head));
				tempDevice.SetQuaternion("LeftEye", InputTracking.GetLocalRotation(VRNode.LeftEye));
				tempDevice.SetQuaternion("RightEye", InputTracking.GetLocalRotation(VRNode.RightEye));
				tempDevice.SetQuaternion("CenterEye", InputTracking.GetLocalRotation(VRNode.CenterEye));

			} else {
				VRInput.GetDevice("ViveHMD").isTracked = false;
			}
			#endif
		}

		float getDeltaAngle(Vector2 cur, Vector2 last) {
			float delta = Mathf.Atan2(cur.y, cur.x) - Mathf.Atan2(last.y, last.x);
			delta *= -Mathf.Rad2Deg;
			if(delta > 180f) delta -= 360f;
			if(delta < -180f) delta += 360f;

			return delta;
		}

		public TouchpadDir getTouchpadDir(Vector2 touchpadPosition, ButtonState touchpadState) {
			if(touchpadState.Equals(ButtonState.Inactive)) return TouchpadDir.Untouched;
			if(touchpadPosition.magnitude < 0.2f) return TouchpadDir.Center;

			float angle = Mathf.Atan2(touchpadPosition.y, touchpadPosition.x);
			angle *= Mathf.Rad2Deg;
			angle += 45f;
			angle %= 360f;
			while(angle < 0) angle += 360f;
			if(angle > 0f && angle <= 90f) return TouchpadDir.Right;
			if(angle > 90f && angle <= 180f) return TouchpadDir.Up;
			if(angle > 180f && angle <= 270f) return TouchpadDir.Left;
			if(angle > 270f) return TouchpadDir.Down;

			return TouchpadDir.Center;
		}
		
		int GetButtonInt(SteamVR_Controller.Device device, EVRButtonId id) {
			if(device.GetPressDown (id)) return 2;
			else if(device.GetPress (id)) return 1;
			else if(device.GetPressUp (id)) return -1;
			else if(device.GetTouchDown (id)) return 0;
			else if(device.GetTouch (id)) return 0;
			else if(device.GetTouchUp (id)) return 0;
			else return 0;
		}

		int GetTouchButtonInt(SteamVR_Controller.Device device, EVRButtonId id) {
			if(device.GetPressDown (id)) return 1;
			else if(device.GetPress (id)) return 1;
			else if(device.GetPressUp (id)) return 1;
			else if(device.GetTouchDown (id)) return 2;
			else if(device.GetTouch (id)) return 1;
			else if(device.GetTouchUp (id)) return -1;
			else return 0;
		}
		
		ButtonState getButtonState(bool touched, bool touchdown, bool touchup, bool pressed, bool pressdown, bool pressup) {
			if(pressdown) return ButtonState.PressDown;
			else if(pressed) return ButtonState.Pressed;
			else if(pressup) return ButtonState.PressUp;
			else if(touchdown) return ButtonState.TouchDown;
			else if(touched) return ButtonState.Touched;
			else if(touchup) return ButtonState.TouchUp;
			else return ButtonState.Inactive;
		}
		ButtonState getButtonState(SteamVR_Controller.Device device, EVRButtonId id) {
			if(device.GetPressDown (id)) return ButtonState.PressDown;
			else if(device.GetPress (id)) return ButtonState.Pressed;
			else if(device.GetPressUp (id)) return ButtonState.PressUp;
			else if(device.GetTouchDown (id)) return ButtonState.TouchDown;
			else if(device.GetTouch (id)) return ButtonState.Touched;
			else if(device.GetTouchUp (id)) return ButtonState.TouchUp;
			else return ButtonState.Inactive;
		}


		/*
		IEnumerator GetChaperoneData() {
			ViveData.Chaperone = new ChaperoneData();
			while(!ViveInput.HMD.IsTracked) yield return null;

			yield return new WaitForSeconds(3f);
			HmdQuad_t bounds = new HmdQuad_t();
			ViveData.Chaperone.IsValid = SteamVR_PlayArea.GetBounds(SteamVR_PlayArea.Size.Calibrated, ref bounds);

			if(ViveData.Chaperone.IsValid) {
				ViveData.Chaperone.Width = Mathf.Abs(bounds.vCorners1.v0 - bounds.vCorners0.v0);
				ViveData.Chaperone.Depth = Mathf.Abs(bounds.vCorners2.v2 - bounds.vCorners1.v2);
				Debug.Log("Got chaperone hard bounds as " + ViveData.Chaperone.Width + "m x " + ViveData.Chaperone.Depth + "m");
			} else {
				ViveData.Chaperone.Width = 1f;
				ViveData.Chaperone.Depth = 1f;
				StartCoroutine(KeepTryingChaperone());
			}
		}	

		IEnumerator KeepTryingChaperone() {
			yield return new WaitForSeconds(3f);

			HmdQuad_t bounds = new HmdQuad_t();
			ViveData.Chaperone.IsValid = SteamVR_PlayArea.GetBounds(SteamVR_PlayArea.Size.Calibrated, ref bounds);

			if(ViveData.Chaperone.IsValid) {
				ViveData.Chaperone.Width = Mathf.Abs(bounds.vCorners1.v0 - bounds.vCorners0.v0);
				ViveData.Chaperone.Depth = Mathf.Abs(bounds.vCorners2.v2 - bounds.vCorners1.v2);
				Debug.Log("Got chaperone hard bounds as " + ViveData.Chaperone.Width + "m x " + ViveData.Chaperone.Depth + "m");
			} else {
				ViveData.Chaperone.Width = 1f;
				ViveData.Chaperone.Depth = 1f;
				StartCoroutine(KeepTryingChaperone());
			}
		}*/
	}

}