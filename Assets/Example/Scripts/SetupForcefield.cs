using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class SetupForcefield : MonoBehaviour {

	public ExampleShaderValues Values;
	public GpuBuffer FieldBuffer;

	// Use this for initialization
	void Awake () {
		InitializeData();
		SetupData();
	}
	
	void InitializeData() {
		FieldBuffer.SetType(typeof(FieldStruct));
		FieldBuffer.SetCount((int)(Values.FieldCount.x * Values.FieldCount.y * Values.FieldCount.z));
	}

	void SetupData() {
		FieldStruct[] Forcefield = new FieldStruct[FieldBuffer.Count];
		for(int i = 0; i < Forcefield.Length; i++) {
			Forcefield[i].pos = Vector3.zero;
			Forcefield[i].force = Vector3.zero;
		}
		FieldBuffer.SetData(Forcefield);
	}

	private void Update() {
		if(VRTK_Devices.ButtonTwoPressed(VRDevice.Left) || VRTK_Devices.ButtonTwoPressed(VRDevice.Right)) {
			StartCoroutine(ResetField());
		}
	}

	IEnumerator ResetField() {
		float StoredValue = Values.FieldDamping;
		Values.FieldDamping = 1f;
		yield return null;
		Values.FieldDamping = StoredValue;
	}
}
