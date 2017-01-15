using UnityEngine;
using System.Collections;
using VRTools;

public class CustomControllerAnimation : MonoBehaviour {

	public Transform GripLeft;
	public Transform GripRight;
	public Transform Menu;
	public Transform Trigger;
	public Transform Touchpad;

	public float GripXDisplacement;
	public float TouchpadYDisplacement;
	public float MenuYDisplacement;
	public float TriggerXRotation;

	public string DeviceName;

	VRInputDevice MyDevice;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		try {
			MyDevice = VRInput.GetDevice(DeviceName);
		} catch (System.NullReferenceException e) {
			//fail silently - this usually happens for a few frames - should probably fix this
			return;
		}

		SetVectorElement(GripLeft, 0, MyDevice.GetButton("Grip") ? -GripXDisplacement : 0);
		SetVectorElement(GripRight, 0, MyDevice.GetButton("Grip") ? GripXDisplacement : 0);

		SetVectorElement(Touchpad, 1, MyDevice.GetButton("Touchpad") ? TouchpadYDisplacement : 0);

		SetVectorElement(Menu, 1, MyDevice.GetButton("Menu") ? MenuYDisplacement: 0);

		Trigger.localEulerAngles = new Vector3(MyDevice.GetAxis("Trigger") * TriggerXRotation, 0, 0);
	}

	void SetVectorElement(Transform input, int elementIndex, float newValue) {
		Vector3 tempV = input.localPosition;
		if(elementIndex == 0) {
			tempV.x = newValue;
		} else if(elementIndex == 1) {
			tempV.y = newValue;
		} else if(elementIndex == 2) {
			tempV.z = newValue;
		} else {
			Debug.Log("Error - elementIndex must be 0, 1 or 2 (for x, y or z)");
			return;
		}

		input.localPosition = tempV;
	}
}
