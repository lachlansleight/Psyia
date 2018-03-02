using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class TestQueue : MonoBehaviour {

	public ComputeDispatcher Simulation;
	public ComputeDispatcher DistanceCalculator;
	public TestSorter Sorter;
	
	public bool Simulate;
	public bool Calculate;
	public bool Sort;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Simulate) Simulation.Dispatch();
		if(Calculate) DistanceCalculator.Dispatch();
		if(Sort) Sorter.DoSortSteps();

		if(Input.GetKeyDown(KeyCode.Space)) {
			Sorter.DoSortSteps();
		}
	}
}
