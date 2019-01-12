using System.Collections;
using System.Collections.Generic;
using UCTK;
using UnityEngine;

public class PsyiaSandbox : MonoBehaviour
{
	public ComputeRenderer PsyiaRenderer;
	public PsyiaController LeftController;
	public PsyiaController RightController;
	public ControllerHaptics LeftHaptics;
	public ControllerHaptics RightHaptics;
	
	public void Start()
	{
		PsyiaRenderer.enabled = true;
		LeftController.gameObject.SetActive(true);
		RightController.gameObject.SetActive(true);
		LeftHaptics.enabled = RightHaptics.enabled = true;
	}
}
