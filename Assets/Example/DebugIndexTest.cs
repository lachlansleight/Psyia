using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class DebugIndexTest : MonoBehaviour {

	public GpuBuffer ForceField;

	FieldStruct[] Field;
	bool HasData = false;

	public ExampleShaderValues Values;

	public int Index;

	public int Choice = 0;

	// Use this for initialization
	void Start () {
		StartCoroutine(GetField());
	}

	IEnumerator GetField() {
		yield return new WaitForSeconds(0.5f);
		Field = ForceField.GetData<FieldStruct>();
		HasData = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!HasData) return;

		Vector3 Position = VRTK_Devices.Position(VRDevice.Right);
		Index = IndexFromPosition(Position);

		if(Index > 0 && Index < Field.Length)
			VRTools.VRDebug.DrawLine(Position, Field[Index].pos, Color.green, true, 0.001f);
	}

	int IndexFromPosition(Vector3 Position) {
		Vector3Int IndexVector = new Vector3Int(
			Mathf.FloorToInt(Mathf.Lerp(0, Values.FieldCount.x - 1, Mathf.Clamp01(Mathf.InverseLerp(Values.FieldStartPos.x, Values.FieldEndPos.x, Position.x)))),
			Mathf.FloorToInt(Mathf.Lerp(0, Values.FieldCount.y - 1, Mathf.Clamp01(Mathf.InverseLerp(Values.FieldStartPos.y, Values.FieldEndPos.y, Position.y)))),
			Mathf.FloorToInt(Mathf.Lerp(0, Values.FieldCount.z - 1, Mathf.Clamp01(Mathf.InverseLerp(Values.FieldStartPos.z, Values.FieldEndPos.z, Position.z))))
		);

		return (int)(IndexVector.x * Values.FieldCount.y * Values.FieldCount.z + IndexVector.y * Values.FieldCount.z + IndexVector.z);
	}
}
