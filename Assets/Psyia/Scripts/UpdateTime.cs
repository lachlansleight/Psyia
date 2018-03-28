using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTime : MonoBehaviour {

	public TimeData Data;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Data.Time = Time.time;
		Data.DeltaTime = Time.deltaTime;
	}
}
