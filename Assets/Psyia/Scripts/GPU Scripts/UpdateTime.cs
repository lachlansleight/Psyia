using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTime : MonoBehaviour {

	public ComputeShader[] ComputeShaders;
	public Material[] Materials;
	
	void Update () {
		foreach(ComputeShader cs in ComputeShaders) {
			cs.SetFloat("Time", Time.time);
			cs.SetFloat("DeltaTime", Time.deltaTime);
		}
		foreach(Material m in Materials) {
			m.SetFloat("Time", Time.time);
			m.SetFloat("DeltaTime", Time.deltaTime);
		}
	}
}
