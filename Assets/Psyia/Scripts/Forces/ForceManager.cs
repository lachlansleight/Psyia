using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class ForceManager : MonoBehaviour {

	private static ForceManager _Instance;
	public static ForceManager Instance {
		get {
			if(_Instance == null) _Instance = GameObject.FindObjectOfType<ForceManager>();
			return _Instance;
		}
	}
	public GpuBuffer ForceBuffer;

	private List<ForceSource> _Sources;
	private List<ForceSource> Sources {
		get {
			if(_Sources == null) _Sources = new List<ForceSource>();
			return _Sources;
		} set {
			_Sources = value;
		}
	}

	public string[] CurrentForces;
	
	void Update () {
		//debug
		CurrentForces = new string[Sources.Count];
		for(int i = 0; i < CurrentForces.Length; i++) {
			CurrentForces[i] = Sources[i].gameObject.name;
		}

		SetData();
	}

	public void AddSource(ForceSource NewSource) {
		if(Sources.Contains(NewSource)) return;

		Sources.Add(NewSource);

		UpdateCount();
	}

	public void RemoveSource(int i) {
		if(i < 0 || i >= Sources.Count) return;

		Sources.RemoveAt(i);

		UpdateCount();
	}

	void UpdateCount() {
		ForceBuffer.SetCount(Sources.Count);
	}

	void SetData() {
		ForceData[] OutputData = new ForceData[Sources.Count];

		if(ForceBuffer.Count != OutputData.Length) UpdateCount();

		for(int i = OutputData.Length - 1; i >= 0; i--) {
			if(Sources[i] == null) RemoveSource(i);

			OutputData[i] = Sources[i].gameObject.activeSelf ? Sources[i].GetForceData() : PsyiaForce.EmptyForceData;
		}
		ForceBuffer.SetData(OutputData);
	}
}
