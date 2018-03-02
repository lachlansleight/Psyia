using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class VRTK_Device : MonoBehaviour {

	//we use a private one to make sure nobody changes the value in the inspector!
	private string _Key;

	public string Key;

	private void Awake() {
		_Key = Key;

		VRTK_ControllerEvents MyEvents = GetComponent<VRTK_ControllerEvents>();
		if(MyEvents == null) VRTK_Devices.AddDevice(_Key, transform);
		else VRTK_Devices.AddDevice(_Key, transform, GetComponent<VRTK_ControllerEvents>());
	}

	private void OnDestroy() {
		VRTK_Devices.RemoveDevice(_Key);
	}
}
