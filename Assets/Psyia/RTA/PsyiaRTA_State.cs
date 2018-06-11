using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityFloatEvent : UnityEvent<float> { }

public class PsyiaRTA_State : MonoBehaviour {

	public PsyiaRTA_StateKey Key;
	public UnityEvent OnStateActivate;
	public UnityEvent OnStateDeactivate;
	public UnityFloatEvent OnLerpValueChanged;

	void Update () {
		Key.CalculateValues();
	}
}
