using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Psyia;
using UnityEngine;

public class ControllerSettings : MonoBehaviour
{
	[Header("Controllers")]
	public PsyiaController LeftController;
	public PsyiaForce LeftForce;
	public PsyiaEmitter LeftEmitter;
	public ControllerSymmetry LeftSymmetry;
	public ControllerHaptics LeftHaptics;
	[Space(10)]
	public PsyiaController RightController;
	public PsyiaForce RightForce;
	public PsyiaEmitter RightEmitter;
	public ControllerSymmetry RightSymmetry;
	public ControllerHaptics RightHaptics;
	
	[Header("Panels")]
	public GameObject[] LeftPanels;
	public GameObject LeftSymmetryReporter;
	public GameObject[] RightPanels;
	public GameObject RightSymmetryReporter;

	[Header("Sliders")]
	public GameObject LeftDistanceSlider;
	public GameObject LeftSofteningFactorSlider;
	public GameObject LeftWavelengthSlider;
	[Space(10)]
	public GameObject RightDistanceSlider;
	public GameObject RightSofteningFactorSlider;
	public GameObject RightWavelengthSlider;
	
	public void Start()
	{
		foreach (var go in LeftPanels) go.SetActive(true);
		LeftSymmetryReporter.SetActive(false);
		foreach (var go in RightPanels) go.SetActive(true);
		RightSymmetryReporter.SetActive(false);
		
		var mode = (int) LeftForce.AttenuationMode;
		LeftDistanceSlider.SetActive(mode > 0 && mode < 4 );
		LeftSofteningFactorSlider.SetActive(mode > 3 && mode < 6);
		LeftWavelengthSlider.SetActive(mode == 6);
		
		mode = (int) RightForce.AttenuationMode;
		RightDistanceSlider.SetActive(mode > 0 && mode < 4 );
		RightSofteningFactorSlider.SetActive(mode > 3 && mode < 6);
		RightWavelengthSlider.SetActive(mode == 6);

		var output = "Connected joysticks:";
		foreach (var s in Input.GetJoystickNames()) {
			output += "\n" + s;
		}
		
		Debug.Log(output);
	}

	public void SetSymmetry(int value)
	{
		LeftSymmetry.enabled = value == 2;
		RightSymmetry.enabled = value == 0;
		
		LeftSymmetryReporter.SetActive(LeftSymmetry.enabled);
		RightSymmetryReporter.SetActive(RightSymmetry.enabled);
		
		foreach (var go in LeftPanels) go.SetActive(!LeftSymmetry.enabled);
		foreach (var go in RightPanels) go.SetActive(!RightSymmetry.enabled);
	}

	public void SetControllerModels(bool value)
	{
		LeftController.ShowFullModel = RightController.ShowFullModel = value;
	}

	public void SetControllerDistance(float value)
	{
		LeftController.ControllerDistance = RightController.ControllerDistance = value;
	}

	public void SetControllerHaptics(bool value)
	{
		LeftHaptics.enabled = RightHaptics.enabled = value;
	}

	public void SetLeftForceShape(int value)
	{
		switch (value) {
			case 0:
				LeftForce.Shape = PsyiaForce.ForceShape.Radial;
				break;
			case 1:
				LeftForce.Shape = PsyiaForce.ForceShape.Orbital;
				break;
			case 2:
				LeftForce.Shape = PsyiaForce.ForceShape.Dipole;
				break;
			case 3:
				LeftForce.Shape = PsyiaForce.ForceShape.Linear;
				break;
			default:
				throw new System.FormatException("Unexpected value '" + value + "' for SetLeftForceShape");
		}
	}
	
	public void SetRightForceShape(int value)
	{
		switch (value) {
			case 0:
				RightForce.Shape = PsyiaForce.ForceShape.Radial;
				break;
			case 1:
				RightForce.Shape = PsyiaForce.ForceShape.Orbital;
				break;
			case 2:
				RightForce.Shape = PsyiaForce.ForceShape.Dipole;
				break;
			case 3:
				RightForce.Shape = PsyiaForce.ForceShape.Linear;
				break;
			default:
				throw new System.FormatException("Unexpected value '" + value + "' for SetRightForceShape");
		}
	}

	public void SetLeftForceAttenuationMode(int value)
	{
		switch (value) {
			case 0:
				LeftForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.Infinite;
				break;
			case 1:
				LeftForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.Linear;
				break;
			case 2:
				LeftForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.Hyperbolic;
				break;
			case 3:
				LeftForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.HyperbolicSquared;
				break;
			case 4:
				LeftForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.HyperbolicSoftened;
				break;
			case 5:
				LeftForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.HyperbolicSquaredSoftened;
				break;
			case 6:
				LeftForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.Sine;
				break;
			default:
				throw new System.FormatException("Unexpected value '" + value + "' for SetLeftForceAttenuationMode");
		}

		var mode = (int) LeftForce.AttenuationMode;
		LeftDistanceSlider.SetActive(mode > 0 && mode < 4 );
		LeftSofteningFactorSlider.SetActive(mode > 3 && mode < 6);
		LeftWavelengthSlider.SetActive(mode == 6);

		XRP.XrpSlider slider = null;
		if (LeftDistanceSlider.activeSelf) 
			slider = LeftDistanceSlider.GetComponent<XRP.XrpSlider>();
		else if (LeftSofteningFactorSlider.activeSelf) 
			slider = LeftSofteningFactorSlider.GetComponent<XRP.XrpSlider>();
		else if (LeftWavelengthSlider.activeSelf) 
			slider = LeftWavelengthSlider.GetComponent<XRP.XrpSlider>();
		
		
		if(slider != null)
			slider.OnValueChangedEvent.Invoke(slider.CurrentValue);
			
	}
	
	public void SetRightForceAttenuationMode(int value)
	{
		switch (value) {
			case 0:
				RightForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.Infinite;
				break;
			case 1:
				RightForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.Linear;
				break;
			case 2:
				RightForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.Hyperbolic;
				break;
			case 3:
				RightForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.HyperbolicSquared;
				break;
			case 4:
				RightForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.HyperbolicSoftened;
				break;
			case 5:
				RightForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.HyperbolicSquaredSoftened;
				break;
			case 6:
				RightForce.AttenuationMode = PsyiaForce.ForceAttenuationMode.Sine;
				break;
			default:
				throw new System.FormatException("Unexpected value '" + value + "' for SetRightForceAttenuationMode");
		}

		var mode = (int) RightForce.AttenuationMode;
		RightDistanceSlider.SetActive(mode > 0 && mode < 4 );
		RightSofteningFactorSlider.SetActive(mode > 3 && mode < 6);
		RightWavelengthSlider.SetActive(mode == 6);
		
		XRP.XrpSlider slider = null;
		if (RightDistanceSlider.activeSelf) 
			slider = RightDistanceSlider.GetComponent<XRP.XrpSlider>();
		else if (RightSofteningFactorSlider.activeSelf) 
			slider = RightSofteningFactorSlider.GetComponent<XRP.XrpSlider>();
		else if (RightWavelengthSlider.activeSelf) 
			slider = RightWavelengthSlider.GetComponent<XRP.XrpSlider>();
		
		
		if(slider != null)
			slider.OnValueChangedEvent.Invoke(slider.CurrentValue);
	}

	public void SetLeftForceStrength(float value)
	{
		LeftForce.Strength = value;
	}

	public void SetRightForceStrength(float value)
	{
		RightForce.Strength = value;
	}

	public void SetLeftForceAttenuationDistance(float value)
	{
		LeftForce.AttenuationDistance = value;
	}

	public void SetRightForceAttenuationDistance(float value)
	{
		RightForce.AttenuationDistance = value;
	}

	public void SetLeftEmissionCount(float value)
	{
		LeftEmitter.EmitOverTime = value;
	}

	public void SetRightEmissionCount(float value)
	{
		RightEmitter.EmitOverTime = value;
	}

	public void SetLeftEmissionRadius(float value)
	{
		LeftEmitter.Settings.EmissionRadius = value;
	}

	public void SetRightEmissionRadius(float value)
	{
		RightEmitter.Settings.EmissionRadius = value;
	}

	public void SetLeftEmissionVelocity(float value)
	{
		LeftEmitter.Settings.MinSpawnVelocity = value * 0.5f;
		LeftEmitter.Settings.MaxSpawnVelocity = value * 1.5f;
	}

	public void SetRightEmissionVelocity(float value)
	{
		RightEmitter.Settings.MinSpawnVelocity = value * 0.5f;
		RightEmitter.Settings.MaxSpawnVelocity = value * 1.5f;
	}

	public void SetLeftEmissionVelocitySpread(float value)
	{
		LeftEmitter.Settings.RandomiseDirection = value;
	}
	
	public void SetRightEmissionVelocitySpread(float value)
	{
		RightEmitter.Settings.RandomiseDirection = value;
	}

	public void SetLeftEmissionInheritVelocity(float value)
	{
		LeftEmitter.Settings.InheritVelocity = value;
	}
	
	public void SetRightEmissionInheritVelocity(float value)
	{
		RightEmitter.Settings.InheritVelocity = value;
	}
}
