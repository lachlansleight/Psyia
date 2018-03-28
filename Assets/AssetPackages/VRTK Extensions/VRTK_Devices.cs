using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using System.Reflection;

public enum VRDevice {
	Left,
	Right,
	HMD
}

public class VRTK_Devices {

	public class ControllerButtonState {
		public int TriggerPressDown = -1;
		public int TriggerPressUp = -1;
		public int TriggerTouchDown = -1;
		public int TriggerTouchUp = -1;
		public int TriggerHairlineDown = -1;
		public int TriggerHairlineUp = -1;
		public int TriggerClickDown = -1;
		public int TriggerClickUp = -1;

		public int GripPressDown = -1;
		public int GripPressUp = -1;
		public int GripTouchDown = -1;
		public int GripTouchUp = -1;
		public int GripHairlineDown = -1;
		public int GripHairlineUp = -1;
		public int GripClickDown = -1;
		public int GripClickUp = -1;

		public int TouchpadPressDown = -1;
		public int TouchpadPressUp = -1;
		public int TouchpadTouchDown = -1;
		public int TouchpadTouchUp = -1;

		public int ButtonOneTouchDown = -1;
		public int ButtonOneTouchUp = -1;
		public int ButtonOneClickDown = -1;
		public int ButtonOneClickUp = -1;

		public int ButtonTwoTouchDown = -1;
		public int ButtonTwoTouchUp = -1;
		public int ButtonTwoClickDown = -1;
		public int ButtonTwoClickUp = -1;

		public int StartButtonClickDown = -1;
		public int StartButtonClickUp = -1;

		public int ControllerEnabled = -1;
		public int ControllerDisabled = -1;
		
		public int ControllerVisible = -1;
		public int ControllerHidden = -1;

		public int ControllerIndex;
		
		public float TriggerAxis;
		public float GripAxis;
		public Vector2 TouchpadAxis;
		public float TouchpadAngle;
	}

	#region
	private string[] EventNames = new string[] {
		"TriggerPressed",
		"TriggerReleased",
		"TriggerTouchStart",
		"TriggerTouchEnd",
		"TriggerHairlineStart",
		"TriggerHairlineEnd",
		"TriggerClicked",
		"TriggerUnclicked",
		"TriggerAxisChanged",
		"GripPressed",
		"GripReleased",
		"GripTouchStart",
		"GripTouchEnd",
		"GripHairlineStart",
		"GripHairlineEnd",
		"GripClicked",
		"GripUnclicked",
		"GripAxisChanged",
		"TouchpadPressed",
		"TouchpadReleased",
		"TouchpadTouchStart",
		"TouchpadTouchEnd",
		"TouchpadAxisChanged",
		"ButtonOneTouchStart",
		"ButtonOneTouchEnd",
		"ButtonOnePressed",
		"ButtonOneReleased",
		"ButtonTwoTouchStart",
		"ButtonTwoTouchEnd",
		"ButtonTwoPressed",
		"ButtonTwoReleased",
		"StartMenuPressed",
		"StartMenuReleased",
		"AliasPointerOn",
		"AliasPointerOff",
		"AliasPointerSet",
		"AliasGrabOn",
		"AliasGrabOff",
		"AliasUseOn",
		"AliasUseOff",
		"AliasMenuOn",
		"AliasMenuOff",
		"AliasUIClickOn",
		"AliasUIClickOff",
		"ControllerEnabled",
		"ControllerDisabled",
		"ControllerIndexChanged",
		"ControllerVisible",
		"ControllerHidden"
	};
	#endregion

	static Dictionary<VRDevice, Transform> _Transforms;
	static Dictionary<VRDevice, Transform> Transforms {
		get {
			if(_Transforms == null) _Transforms = new Dictionary<VRDevice, Transform>();
			return _Transforms;
		} set {
			_Transforms = value;
		}
	}

	public static bool HasDevice(VRDevice Key) { return Transforms.ContainsKey(Key); }

	static Dictionary<VRDevice, VRTK_ControllerEvents> _ControllerEvents;
	static Dictionary<VRDevice, VRTK_ControllerEvents> ControllerEvents {
		get {
			if(_ControllerEvents == null) _ControllerEvents = new Dictionary<VRDevice, VRTK_ControllerEvents>();
			return _ControllerEvents;
		} set {
			_ControllerEvents = value;
		}
	}

	static Dictionary<VRDevice, EventInfo[]> _Events;
	static Dictionary<VRDevice, EventInfo[]> Events {
		get {
			if(_Events == null) _Events = new Dictionary<VRDevice, EventInfo[]>();
			return _Events;
		} set {
			_Events = value;
		}
	}


	static Dictionary<VRDevice, ControllerButtonState> _ButtonStates;
	static Dictionary<VRDevice, ControllerButtonState> ButtonStates {
		get {
			if(_ButtonStates == null) _ButtonStates = new Dictionary<VRDevice, ControllerButtonState>();
			return _ButtonStates;
		} set {
			_ButtonStates = value;
		}
	}

	/// <summary>
	/// debug only!
	/// </summary>
	public static ControllerButtonState GetState(VRDevice Key) {
		if(!ButtonStates.ContainsKey(Key)) return null;
		return ButtonStates[Key];
	}

	/// <summary>
	/// Gets the position of this device
	/// </summary>
	public static Vector3 Position(VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return Vector3.zero;
		return Transforms[Key].position;
	}

	/// <summary>
	/// Gets the position of this device
	/// </summary>
	public static Quaternion Rotation(VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return Quaternion.identity;
		return Transforms[Key].rotation;
	}

	/// <summary>
	/// Gets the position of this device
	/// </summary>
	public static Vector3 EulerAngles(VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return Vector3.zero;
		return Transforms[Key].eulerAngles;
	}

	/// <summary>
	/// Gets the forward vector of this device
	/// </summary>
	public static Vector3 Forward(VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return Vector3.zero;
		return Transforms[Key].forward;
	}

	/// <summary>
	/// Gets the transform for this device
	/// </summary>
	public static Transform Transform(VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return null;
		return Transforms[Key];
	}

	public static Vector3 Velocity(VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return Vector3.zero;
		if(Key == VRDevice.HMD) return VRTK_DeviceFinder.GetHeadsetVelocity();
		else return VRTK_DeviceFinder.GetControllerVelocity(VRTK_ControllerReference.GetControllerReference(Transforms[Key].gameObject));
	}

	public static Vector3 AngularVelocity(VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return Vector3.zero;
		if(Key == VRDevice.HMD) return VRTK_DeviceFinder.GetHeadsetAngularVelocity();
		else return VRTK_DeviceFinder.GetControllerAngularVelocity(VRTK_ControllerReference.GetControllerReference(Transforms[Key].gameObject));
	}


	/// <summary>
	/// This will be true if the trigger is squeezed about half way in
	/// </summary>
	public static bool TriggerPressed(VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].triggerPressed;
	}
	
	/// <summary>
	/// This will be true if the trigger is squeezed a small amount.
	/// </summary>
	public static bool TriggerTouched (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].triggerTouched ;
	}
	
	/// <summary>
	/// This will be true if the trigger is squeezed a small amount more from any previous squeeze on the trigger
	/// </summary>
	public static bool TriggerHairlinePressed (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].triggerHairlinePressed ;
	}
	
	/// <summary>
	/// This will be true if the trigger is squeezed all the way down
	/// </summary>
	public static bool TriggerClicked (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].triggerClicked ;
	}
	
	/// <summary>
	/// This will be true if the trigger has been squeezed more or less
	/// </summary>
	public static bool TriggerAxisChanged (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].triggerAxisChanged ;
	}
	
	/// <summary>
	/// This will be true if the grip is squeezed about half way in
	/// </summary>
	public static bool GripPressed (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].gripPressed ;
	}
	
	/// <summary>
	/// This will be true if the grip is touched
	/// </summary>
	public static bool GripTouched (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].gripTouched ;
	}
	
	/// <summary>
	/// his will be true if the grip is squeezed a small amount more from any previous squeeze on the grip
	/// </summary>
	public static bool GripHairlinePressed (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].gripHairlinePressed ;
	}
	
	/// <summary>
	/// This will be true if the grip is squeezed all the way down
	/// </summary>
	public static bool GripClicked (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].gripClicked ;
	}
	
	/// <summary>
	/// This will be true if the grip has been squeezed more or less
	/// </summary>
	public static bool GripAxisChanged (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].triggerPressed;
	}
	
	/// <summary>
	/// This will be true if the touchpad is held down
	/// </summary>
	public static bool TouchpadPressed (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].touchpadPressed;
	}
	
	/// <summary>
	/// This will be true if the touchpad is being touched
	/// </summary>
	public static bool TouchpadTouched (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].touchpadTouched ;
	}
	
	/// <summary>
	/// This will be true if the touchpad touch position has changed
	/// </summary>
	public static bool TouchpadAxisChanged  (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].touchpadAxisChanged ;
	}
	
	/// <summary>
	/// This will be true if button one is held down
	/// </summary>
	public static bool ButtonOnePressed  (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].buttonOnePressed ;
	}
	
	/// <summary>
	/// This will be true if button one is being touched
	/// </summary>
	public static bool ButtonOneTouched (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].buttonOneTouched ;
	}
	
	/// <summary>
	/// This will be true if button two is held down
	/// </summary>
	public static bool ButtonTwoPressed (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].buttonTwoPressed ;
	}
	
	/// <summary>
	/// This will be true if button two is being touched
	/// </summary>
	public static bool ButtonTwoTouched (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].buttonTwoTouched ;
	}
	
	/// <summary>
	/// This will be true if start menu is held down
	/// </summary>
	public static bool StartMenuPressed (VRDevice Key) {
		if(!Transforms.ContainsKey(Key)) return false;

		return ControllerEvents[Key].startMenuPressed ;
	}
	
	/// <summary>
	/// Adds a device to the device list
	/// </summary>
	public static void AddDevice(VRDevice Key, Transform Target) {
		if(Transforms.ContainsKey(Key)) return;
		Transforms.Add(Key, Target);
	}
	
	/// <summary>
	/// Add a controller with a ControllerEvents component to the device list
	/// </summary>
	public static void AddDevice(VRDevice Key, Transform Target, VRTK_ControllerEvents TargetControllerEvents) {
		if(Transforms.ContainsKey(Key) || ControllerEvents.ContainsKey(Key)) return;

		Transforms.Add(Key, Target);
		ControllerEvents.Add(Key, TargetControllerEvents);
		
		EventInfo[] NewEvents = TargetControllerEvents.GetType().GetEvents();
		for(int i = 0; i < NewEvents.Length; i++) {
			
			MethodInfo CurrentMethod = typeof(VRTK_Devices).GetMethod("HandleEvent");
			MethodInfo AddHandler = NewEvents[i].GetAddMethod();
			System.Delegate d = System.Delegate.CreateDelegate(NewEvents[i].EventHandlerType, NewEvents[i].Name, CurrentMethod, false);//System.Delegate.CreateDelegate(NewEvents[i].EventHandlerType, typeof(VRTK_Devices), "HandleEvent", false);
			AddHandler.Invoke((System.Object)TargetControllerEvents, new System.Object[] { d });
			
		}
		Events.Add(Key, NewEvents);
		ButtonStates.Add(Key, new ControllerButtonState());
	}

	public static void DebugButtonState(VRDevice Key) {
		if(!ButtonStates.ContainsKey(Key)) return;

		string DebugString = "Button State for " + Key + " at frame " + Time.frameCount;
		DebugString += "\nTriggerClickDown" + ButtonStates[Key].TriggerClickDown;
		DebugString += "\nTriggerClickUp" + ButtonStates[Key].TriggerClickDown;
		Debug.Log(DebugString);
	}

	public static void HandleEvent(string eventName, object caller, ControllerInteractionEventArgs e) {
		VRTK_ControllerEvents CallingCE = caller as VRTK_ControllerEvents;
		if(CallingCE != null) {
			VRDevice Key = CallingCE.gameObject.GetComponent<VRTK_Device>().Key;
			//Debug.Log(eventName + " called on " + Key);

			if(eventName == "TriggerPressed") ButtonStates[Key].TriggerPressDown = Time.frameCount;
			if(eventName == "TriggerReleased") ButtonStates[Key].TriggerPressUp = Time.frameCount;
			if(eventName == "TriggerTouchStart") ButtonStates[Key].TriggerTouchDown = Time.frameCount;
			if(eventName == "TriggerTouchEnd") ButtonStates[Key].TriggerTouchUp = Time.frameCount;
			if(eventName == "TriggerHairlineStart") ButtonStates[Key].TriggerHairlineDown = Time.frameCount;
			if(eventName == "TriggerHairlineEnd") ButtonStates[Key].TriggerHairlineUp = Time.frameCount;
			if(eventName == "TriggerClicked") ButtonStates[Key].TriggerClickDown = Time.frameCount;
			if(eventName == "TriggerUnclicked") ButtonStates[Key].TriggerClickUp = Time.frameCount;
			
			if(eventName == "GripPressed") ButtonStates[Key].GripPressDown = Time.frameCount;
			if(eventName == "GripReleased") ButtonStates[Key].GripPressUp = Time.frameCount;
			if(eventName == "GripTouchStart") ButtonStates[Key].GripTouchDown = Time.frameCount;
			if(eventName == "GripTouchEnd") ButtonStates[Key].GripTouchUp = Time.frameCount;
			if(eventName == "GripHairlineStart") ButtonStates[Key].GripHairlineDown = Time.frameCount;
			if(eventName == "GripHairlineEnd") ButtonStates[Key].GripHairlineUp = Time.frameCount;
			if(eventName == "GripClicked") ButtonStates[Key].GripClickDown = Time.frameCount;
			if(eventName == "GripUnclicked") ButtonStates[Key].GripClickUp = Time.frameCount;

			if(eventName == "TouchpadPressed") ButtonStates[Key].TouchpadPressDown = Time.frameCount;
			if(eventName == "TouchpadReleased") ButtonStates[Key].TouchpadPressUp = Time.frameCount;
			if(eventName == "TouchpadTouchStart") ButtonStates[Key].TouchpadTouchDown = Time.frameCount;
			if(eventName == "TouchpadTouchEnd") ButtonStates[Key].TouchpadTouchUp = Time.frameCount;

			if(eventName == "ButtonOneTouchStart") ButtonStates[Key].ButtonOneTouchDown = Time.frameCount;
			if(eventName == "ButtonOneTouchEnd") ButtonStates[Key].ButtonOneTouchUp = Time.frameCount;
			if(eventName == "ButtonOnePressed") ButtonStates[Key].ButtonOneClickDown = Time.frameCount;
			if(eventName == "ButtonOneReleased") ButtonStates[Key].ButtonOneClickUp = Time.frameCount;

			if(eventName == "ButtonTwoTouchStart") ButtonStates[Key].ButtonTwoTouchDown = Time.frameCount;
			if(eventName == "ButtonTwoTouchEnd") ButtonStates[Key].ButtonTwoTouchUp = Time.frameCount;
			if(eventName == "ButtonTwoPressed") ButtonStates[Key].ButtonTwoClickDown = Time.frameCount;
			if(eventName == "ButtonTwoReleased") ButtonStates[Key].ButtonTwoClickUp = Time.frameCount;

			if(eventName == "StartMenuPressed") ButtonStates[Key].StartButtonClickDown = Time.frameCount;
			if(eventName == "StartMenuReleased") ButtonStates[Key].StartButtonClickUp = Time.frameCount;

			if(eventName == "ControllerEnabled") ButtonStates[Key].ControllerEnabled = Time.frameCount;
			if(eventName == "ControllerDisabled") ButtonStates[Key].ControllerDisabled = Time.frameCount;

			if(eventName == "ControllerVisible") ButtonStates[Key].ControllerVisible = Time.frameCount;
			if(eventName == "ControllerHidden") ButtonStates[Key].ControllerHidden = Time.frameCount;
			
			if(eventName == "ControllerIndexChanged") ButtonStates[Key].ControllerIndex = (int)e.controllerReference.index;

			if(eventName == "TriggerAxisChanged") ButtonStates[Key].TriggerAxis = Mathf.InverseLerp(0.05f, 1f, e.buttonPressure);
			if(eventName == "GripAxisChanged") ButtonStates[Key].GripAxis = e.buttonPressure;
			if(eventName == "TouchpadAxisChanged") ButtonStates[Key].TouchpadAngle = e.touchpadAngle;
			if(eventName == "TouchpadAxisChanged") ButtonStates[Key].TouchpadAxis = e.touchpadAxis;
		}
	}

	//you know what takes ages to meticulously copy-paste?
	public static bool TriggerPressDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TriggerPressDown; }
	public static bool TriggerPressUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TriggerPressUp; }
	public static bool TriggerTouchDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TriggerTouchDown; }
	public static bool TriggerTouchUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TriggerTouchUp; }
	public static bool TriggerHairlineDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TriggerHairlineDown; }
	public static bool TriggerHairlineUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TriggerHairlineUp; }
	public static bool TriggerClickDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TriggerClickDown; }
	public static bool TriggerClickUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TriggerClickUp; }

	public static bool GripPressDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].GripPressDown; }
	public static bool GripPressUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].GripPressUp; }
	public static bool GripTouchDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].GripTouchDown; }
	public static bool GripTouchUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].GripTouchUp; }
	public static bool GripHairlineDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].GripHairlineDown; }
	public static bool GripHairlineUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].GripHairlineUp; }
	public static bool GripClickDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].GripClickDown; }
	public static bool GripClickUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].GripClickUp; }

	public static bool TouchpadPressDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TouchpadPressDown; }
	public static bool TouchpadPressUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TouchpadPressUp; }
	public static bool TouchpadTouchDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TouchpadTouchDown; }
	public static bool TouchpadTouchUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].TouchpadTouchUp; }

	public static bool ButtonOneTouchDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ButtonOneTouchDown; }
	public static bool ButtonOneTouchUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ButtonOneTouchUp; }
	public static bool ButtonOneClickDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ButtonOneClickDown; }
	public static bool ButtonOneClickUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ButtonOneClickUp; }

	public static bool ButtonTwoTouchDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ButtonTwoTouchDown; }
	public static bool ButtonTwoTouchUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ButtonTwoTouchUp; }
	public static bool ButtonTwoClickDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ButtonTwoClickDown; }
	public static bool ButtonTwoClickUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ButtonTwoClickUp; }

	public static bool StartButtonClickDown(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].StartButtonClickDown; }
	public static bool StartButtonClickUp(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].StartButtonClickUp; }

	public static bool ControllerEnabled(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ControllerEnabled; }
	public static bool ControllerDisabled(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ControllerDisabled; }

	public static bool ControllerVisible(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ControllerVisible; }
	public static bool ControllerHidden(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return false; } return (Time.frameCount-1) == ButtonStates[Key].ControllerHidden; }

	public static int ControllerIndex(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return 0; } return ButtonStates[Key].ControllerIndex; }

	public static float TriggerAxis(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return 0; } return ButtonStates[Key].TriggerAxis; }
	public static float GripAxis(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return 0; } return ButtonStates[Key].GripAxis; }
	public static Vector2 TouchpadAxis(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return Vector2.zero; } return ButtonStates[Key].TouchpadAxis; }
	public static float TouchpadAngle(VRDevice Key) { if(!ButtonStates.ContainsKey(Key)) { return 0; } return ButtonStates[Key].TouchpadAngle; }

	/// <summary>
	/// Removes this device from the device list
	/// </summary>
	public static void RemoveDevice(VRDevice Key) {
		if(Transforms.ContainsKey(Key)) Transforms.Remove(Key);

		if(ControllerEvents.ContainsKey(Key)) ControllerEvents.Remove(Key);

		//TODO: remove event assignments?
		if(Events.ContainsKey(Key)) Events.Remove(Key);

		if(ButtonStates.ContainsKey(Key)) ButtonStates.Remove(Key);
	}
}
