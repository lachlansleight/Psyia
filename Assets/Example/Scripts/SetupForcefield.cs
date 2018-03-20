using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class SetupForcefield : MonoBehaviour {

	public ExampleShaderValues Values;
	public ComputeParameterSetter SetupSetter;
	public ComputeDispatcher FieldSetupShader;
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
			Forcefield[i].pos = Vector4.zero;
			Forcefield[i].instantForce = Vector4.zero;
			Forcefield[i].attenuatingForce = Vector4.zero;
		}
		FieldBuffer.SetData(Forcefield);

		SetupSetter.ApplyNow();

		FieldSetupShader.Dispatch();
	}

	private void Update() {
		if(VRTK_Devices.ButtonTwoPressed(VRDevice.Left) || VRTK_Devices.ButtonTwoPressed(VRDevice.Right)) {
			FieldSetupShader.Dispatch();
		}
	}
}
