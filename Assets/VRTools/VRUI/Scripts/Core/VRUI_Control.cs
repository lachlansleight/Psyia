using UnityEngine;
using System.Collections;


namespace VRTools
{
	namespace UI {

		public enum VRUI_State {
			Inactive,
			Hover,
			Down
		}

		public class VRUI_Control : MonoBehaviour {

			[Tooltip("The distance in meters the interaction point needs to get from the active element to activate this control")]
			public float HoverDistance = 0.05f;
            private float SourceHoverDistance = 0.05f;
            [Tooltip("Once this control is 'hovered' over, interaction point needs to get HoverDistance x HoverStretch meters away to deactivate")] [Range(1f, 3f)]
			public float HoverStretch = 1f;
            private float SourceHoverStretch = 1f;
			[Tooltip("While this control is being interacted with, interaction point needs to get HoverDistance x ClickStretch meters away to end interaction")] [Range(1f, 3f)]
			public float ClickStretch = 2f;
            private float SourceClickStretch = 2f;

			[Space(20)]

			public Transform ActiveElement;
			private Material ActiveMaterial;
			private Color emissionTarget = Color.black;
			private float emissionLerpRate = 0.2f;

			public delegate void EmptyEvent();
			public delegate void BooleanEvent(bool value);
			public delegate void IntEvent(int newValue);
			public delegate void FloatEvent(float value);
			public delegate void Vector2Event(Vector2 value);
			public delegate void Vector3Event(Vector3 value);

			public event EmptyEvent OnHoverEnter;
			public event EmptyEvent OnHoverExit;
			public event EmptyEvent OnPressDown;
			public event EmptyEvent OnPressUp;

			public event FloatEvent OnFloatChange;
			public event IntEvent OnIntChange;
			public event BooleanEvent OnBooleanChange;
			public event Vector2Event OnVector2Change;
			public event Vector3Event OnVector3Change;

			public delegate void HapticEvent(Vector3 position, float intensity);
			public event HapticEvent OnHapticPulse;

			 public float floatValue = 0;
			 public bool boolValue = false;
			 public int intValue = 0;
			 public Vector2 vector2Value = Vector2.zero;
			 public Vector3 vector3Value = Vector3.zero;

			public VRUI_State currentState;

			public VRUI_Input activeInput = null;

			// Use this for initialization
			void Start () {
	            SourceHoverDistance = HoverDistance;
                SourceHoverStretch = HoverStretch;
                SourceClickStretch = ClickStretch;
			}

			public virtual void Bang() {
				PressDown();
				ChangeFloatValue(floatValue);
				ChangeIntValue(intValue);
				ChangeBoolValue(boolValue);
				ChangeVector2Value(vector2Value);
				ChangeVector3Value(vector3Value);
			}

			public virtual void SetDefaultValue() {

			}
	
			public virtual void Awake() {
				ActiveMaterial = ActiveElement.GetChild(0).GetComponent<Renderer>().material;
			}
			
			public virtual void Update () {
                //HoverDistance = SourceHoverDistance * VRInput.GetDevice("ViveHMD").transform.parent.localScale.x;
                //HoverStretch = SourceHoverStretch * VRInput.GetDevice("ViveHMD").transform.parent.localScale.x;
                //ClickStretch = SourceClickStretch * VRInput.GetDevice("ViveHMD").transform.parent.localScale.x;

				//if(currentState.Equals(VRUI_State.Inactive)) emissionTarget = Color.black;
				//else emissionTarget = Color.Lerp(Color.black, Color.white, 0.25f);

				ActiveMaterial.SetColor("_EmissionColor", Color.Lerp(ActiveMaterial.GetColor("_EmissionColor"), emissionTarget, emissionLerpRate));
			}

			public virtual void InputMoved(Vector3 newPosition, VRUI_Input uiInput) {
				//if we already are active and it's not by the input that called this, ignore anything it says!
				if(uiInput != null) {
					if(uiInput != activeInput) return;
				}

				float hoverVal = GetScaledRange(newPosition);
				
				//if we're not active, just update hover state
				if(currentState.Equals(VRUI_State.Inactive)) {
					if(hoverVal < 1f) {
						//set hover
						currentState = VRUI_State.Hover;
						HoverOn();
					}
				} else if(currentState.Equals(VRUI_State.Hover)) {
					if(hoverVal > HoverStretch) {
						currentState = VRUI_State.Inactive;
						HoverOff();
					}
				}
				
				//if we are... 
				else {
					//check if we should disable it (input moved too far away)
					if(hoverVal > ClickStretch) {
						currentState = VRUI_State.Inactive;
						PressUp();
						HoverOff();
					//otherwise, time to update the value
					} else {
						Active(newPosition);
					}
				}
			}

			public virtual void Active(Vector3 inputPos) {
				//nothing here (the overrides handle this)
			}

			public float GetScaledRange(Vector3 inputPos) {
				return (ActiveElement.position - inputPos).magnitude / (HoverDistance * VRInput.GetDevice("ViveHMD").transform.parent.localScale.x);
			}


			//if we get here, a VRUI_Input object has created a list of elements valid for activation, and chosen this one as the closest
			public virtual void MakeActive(Vector3 inputPos, VRUI_Input inputDevice) {
				if(activeInput != null) {
					if(activeInput != inputDevice) return;
				}
				PressDown();
				currentState = VRUI_State.Down;
				activeInput = inputDevice;
			}

			//if we get here, a VRUI_Input object with this object as its activeControl has just released its click button
			public virtual void MakeInactive(Vector3 inputPos, VRUI_Input inputDevice) {
				if(activeInput != null) {
					if(activeInput != inputDevice) return;
				}
				PressUp();
				if(GetScaledRange(inputPos) < 1f) currentState = VRUI_State.Hover;
				else currentState = VRUI_State.Inactive;
				activeInput = null;
			}

			/*
			public virtual void Active(Vector3 inputPos, int clickStatus) {
				float hoverVal = (ActiveElement.position - inputPos).magnitude / HoverDistance;

				if(currentState.Equals(VRUI_State.Inactive)) {
					if(hoverVal < 1f) {
						//set hover
						currentState = VRUI_State.Hover;
						HoverOn();


						if(clickStatus == 2) {
							//set click
							currentState = VRUI_State.Down;
							PressDown();
						}
					}
				} else if(currentState.Equals(VRUI_State.Hover)) {
					if(hoverVal > HoverStretch) {
						currentState = VRUI_State.Inactive;
						HoverOff();

					}

					if(clickStatus == 2) {
						//set click
						currentState = VRUI_State.Down;
						PressDown();
					}
				} else { //i.e. if(currentState.Equals(VRUI_State.Down))
					if(hoverVal > ClickStretch) {
						currentState = VRUI_State.Inactive;
						PressUp();
						HoverOff();
					} else if(clickStatus == 0 || clickStatus == -1) {
						currentState = VRUI_State.Hover;
						PressUp();
					}
				}
			}*/

			public void HoverOn() {
				if(OnHoverEnter != null) OnHoverEnter();
				HapticPulse(0.1f);
				
			}
			public void HoverOff() {
				if(OnHoverExit != null) OnHoverExit();
			}

			public void PressDown() {
				if(OnPressDown != null) OnPressDown();
				HapticPulse(0.7f);
				VRUI_Audio.PlayPressDown(ActiveElement.position);
			}
			public void PressUp() {
				if(OnPressUp != null) OnPressUp();
				HapticPulse(0.5f);
				VRUI_Audio.PlayPressUp(ActiveElement.position);
			}

			public void ChangeFloatValue(float newValue) {
				floatValue = newValue;
				if(OnFloatChange != null) OnFloatChange(newValue);
			}

			public void ChangeIntValue(int newValue) {
				intValue = newValue;
				if(OnIntChange != null) OnIntChange(newValue);
			}

			public void ChangeBoolValue(bool newValue) {
				boolValue = newValue;
				if(OnBooleanChange != null) OnBooleanChange(newValue);
			}

			public void ChangeVector2Value(Vector2 newValue) {
				vector2Value = newValue;
				if(OnVector2Change != null) OnVector2Change(newValue);
			}

			public void ChangeVector3Value(Vector3 newValue) {
				vector3Value = newValue;
				if(OnVector3Change != null) OnVector3Change(newValue);
			}

			public void HapticPulse(float intensity) {
				if(OnHapticPulse != null) OnHapticPulse(ActiveElement.position, intensity);
			}

			public virtual void SetFloatValue(float newValue) {

			}
			public virtual void SetIntValue(int newValue) {

			}
			public virtual void SetBoolValue(bool newValue) {

			}
			public virtual void SetVector2Value(Vector2 newValue) {

			}
			public virtual void SetVector3Value(Vector3 newValue) {

			}
		}

	}

}
