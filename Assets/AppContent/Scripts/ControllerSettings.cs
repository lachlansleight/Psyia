using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Psyia;
using UnityEngine;

public class ControllerSettings : MonoBehaviour
{
	public PsyiaSettingsApplicator SettingsApplicator;
	
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
	public GameObject LeftDistanceButton;
	public GameObject LeftSofteningFactorSlider;
	public GameObject LeftSofteningFactorButton;
	public GameObject LeftWavelengthSlider;
	public GameObject LeftWavelengthButton;
	[Space(10)]
	public GameObject RightDistanceSlider;
	public GameObject RightDistanceButton;
	public GameObject RightSofteningFactorSlider;
	public GameObject RightSofteningFactorButton;
	public GameObject RightWavelengthSlider;
	public GameObject RightWavelengthButton;
	
	public void Start()
	{
		foreach (var go in LeftPanels) go.SetActive(true);
		LeftSymmetryReporter.SetActive(false);
		foreach (var go in RightPanels) go.SetActive(true);
		RightSymmetryReporter.SetActive(false);
		
		var mode = (int) LeftForce.AttenuationMode;
		LeftDistanceSlider.SetActive(mode > 0 && mode < 4 );
		LeftDistanceButton.SetActive(LeftDistanceSlider.activeSelf);
		LeftSofteningFactorSlider.SetActive(mode > 3 && mode < 6);
		LeftSofteningFactorButton.SetActive(LeftSofteningFactorSlider.activeSelf);
		LeftWavelengthSlider.SetActive(mode == 6);
		LeftWavelengthButton.SetActive(LeftWavelengthSlider.activeSelf);
		
		mode = (int) RightForce.AttenuationMode;
		RightDistanceSlider.SetActive(mode > 0 && mode < 4 );
		RightDistanceButton.SetActive(RightDistanceSlider.activeSelf);
		RightSofteningFactorSlider.SetActive(mode > 3 && mode < 6);
		RightSofteningFactorButton.SetActive(RightSofteningFactorSlider.activeSelf);
		RightWavelengthSlider.SetActive(mode == 6);
		RightWavelengthButton.SetActive(RightWavelengthSlider.activeSelf);

		#if UNITY_EDITOR
		//TODO: Add knuckles support
		var output = "Connected joysticks:";
		foreach (var s in Input.GetJoystickNames()) {
			output += "\n" + s;
		}
		Debug.Log(output);
		#endif
	}

	public void SetSymmetry(int value)
	{
		SettingsApplicator.SetSymmetry(value);
		
		LeftSymmetryReporter.SetActive(LeftSymmetry.enabled);
		RightSymmetryReporter.SetActive(RightSymmetry.enabled);
		
		foreach (var go in LeftPanels) go.SetActive(!LeftSymmetry.enabled);
		foreach (var go in RightPanels) go.SetActive(!RightSymmetry.enabled);
	}

	public void SetControllerModels(bool value)
	{
		SettingsApplicator.SetControllerModels(value);
	}

	public void SetControllerDistance(float value)
	{
		SettingsApplicator.SetControllerDistance(value);
	}

	public void SetControllerHaptics(bool value)
	{
		SettingsApplicator.SetControllerHaptics(value);
	}

	public void SetLeftForceShape(int value)
	{
		SettingsApplicator.SetLeftForceShape(value);
	}
	
	public void SetRightForceShape(int value)
	{
		SettingsApplicator.SetRightForceShape(value);
	}

	public void SetLeftForceAttenuationMode(int value)
	{
		SettingsApplicator.SetLeftForceAttenuationMode(value);	
	}
	
	public void SetRightForceAttenuationMode(int value)
	{
		SettingsApplicator.SetRightForceAttenuationMode(value);
	}

	public void SetLeftForceStrength(float value)
	{
		SettingsApplicator.SetLeftForceStrength(value);
	}

	public void SetRightForceStrength(float value)
	{
		SettingsApplicator.SetRightForceStrength(value);
	}

	public void SetLeftForceAttenuationDistance(float value)
	{
		SettingsApplicator.SetLeftForceAttenuationDistance(value);
	}

	public void SetRightForceAttenuationDistance(float value)
	{
		SettingsApplicator.SetRightForceAttenuationDistance(value);
	}
	
	public void SetLeftForceAttenuationSofteningFactor(float value)
	{
		SettingsApplicator.SetLeftForceAttenuationSofteningFactor(value);
	}

	public void SetRightForceAttenuationSofteningFactor(float value)
	{
		SettingsApplicator.SetRightForceAttenuationDistance(value);
	}
	
	public void SetLeftForceAttenuationWavelength(float value)
	{
		SettingsApplicator.SetLeftForceAttenuationWavelength(value);
	}

	public void SetRightForceAttenuationWavelength(float value)
	{
		SettingsApplicator.SetRightForceAttenuationWavelength(value);
	}

	public void SetLeftEmissionCount(float value)
	{
		SettingsApplicator.SetLeftEmitterCount(Mathf.FloorToInt(value));
	}

	public void SetRightEmissionCount(float value)
	{
		SettingsApplicator.SetRightEmitterCount(Mathf.FloorToInt(value));
	}

	public void SetLeftEmissionRadius(float value)
	{
		SettingsApplicator.SetLeftEmitterRadius(value);
	}

	public void SetRightEmissionRadius(float value)
	{
		SettingsApplicator.SetRightEmitterRadius(value);
	}

	public void SetLeftEmissionVelocity(float value)
	{
		SettingsApplicator.SetLeftEmitterVelocity(value);
	}

	public void SetRightEmissionVelocity(float value)
	{
		SettingsApplicator.SetRightEmitterVelocity(value);
	}

	public void SetLeftEmissionVelocitySpread(float value)
	{
		SettingsApplicator.SetLeftEmitterVelocitySpread(value);
	}
	
	public void SetRightEmissionVelocitySpread(float value)
	{
		SettingsApplicator.SetRightEmitterVelocitySpread(value);
	}

	public void SetLeftEmissionInheritVelocity(float value)
	{
		SettingsApplicator.SetLeftEmitterInheritVelocity(value);
	}
	
	public void SetRightEmissionInheritVelocity(float value)
	{
		SettingsApplicator.SetRightEmitterInheritVelocity(value);
	}
}
