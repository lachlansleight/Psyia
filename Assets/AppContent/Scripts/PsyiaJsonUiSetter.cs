using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRP;

public class PsyiaJsonUiSetter : MonoBehaviour
{
	public PsyiaSettingsApplicator SourceSettings;
	private PsyiaSettings _settings;

	[Header("System")]
	public XrpSlider MaxParticleCountSlider;
	public XrpButton ApplyParticleCountButton;
	public XrpIntDial AntialiasingDial;
	public XrpToggle BloomToggle;

	[Header("Visual")]
	public XrpIntDial ParticleFormDial;
	public XrpIntDial ParticleColorDial;
	public XrpSlider ParticleColorAmountSlider;
	public AudioVisualisationButton ParticleColorAmountButton;
	public XrpSlider ParticleSizeSlider;
	public AudioVisualisationButton ParticleSizeButton;
	public XrpSlider LineLengthSlider;
	public AudioVisualisationButton LineLengthButton;
	public XrpEnumSlider ParticleShapeSlider;

	[Header("Physics")]
	public XrpSlider ParticleMassSlider;
	public AudioVisualisationButton ParticleMassButton;
	public XrpSlider ParticleDampingSlider;
	public AudioVisualisationButton ParticleDampingButton;
	public XrpSlider TimeSpeedSlider;
	public AudioVisualisationButton TimeSpeedButton;
	public XrpToggle FloorCollisionToggle;

	[Header("Audio")]
	public XrpSlider VisualsAudioreactivitySlider;
	public XrpSlider PhysicsAudioreactivitySlider;
	public XrpDial VolumeDial;
	public XrpToggle LoopToggle;
	public XrpToggle SlowWithTimeToggle;

	[Header("Controller")]
	public XrpEnumSlider ControllerSymmetrySlider;
	public XrpToggle ControllerModelsToggle;
	public XrpSlider ControllerDistanceSlider;
	public AudioVisualisationButton ControllerDistanceButton;
	public XrpToggle ControllerHapticsToggle;

	[Header("Left Force")]
	public XrpIntDial LeftForceShapeDial;
	public XrpIntDial LeftForceAttenuationDial;
	public XrpSlider LeftForceStrengthSlider;
	public AudioVisualisationButton LeftForceStrengthButton;
	public XrpSlider LeftForceAttenuationDistanceSlider;
	public AudioVisualisationButton LeftForceAttenuationDistanceButton;
	public XrpSlider LeftForceSofteningFactorSlider;
	public AudioVisualisationButton LeftForceSofteningFactorButton;
	public XrpSlider LeftForceWavelengthSlider;
	public AudioVisualisationButton LeftForceWavelengthButton;
	
	[Header("Right Force")]
	public XrpIntDial RightForceShapeDial;
	public XrpIntDial RightForceAttenuationDial;
	public XrpSlider RightForceStrengthSlider;
	public AudioVisualisationButton RightForceStrengthButton;
	public XrpSlider RightForceAttenuationDistanceSlider;
	public AudioVisualisationButton RightForceAttenuationDistanceButton;
	public XrpSlider RightForceSofteningFactorSlider;
	public AudioVisualisationButton RightForceSofteningFactorButton;
	public XrpSlider RightForceWavelengthSlider;
	public AudioVisualisationButton RightForceWavelengthButton;

	[Header("Left Emission")]
	public XrpSlider LeftEmitterCountSlider;
	public XrpSlider LeftEmitterRadiusSlider;
	public AudioVisualisationButton LeftEmitterRadiusButton;
	public XrpSlider LeftEmitterVelocitySlider;
	public AudioVisualisationButton LeftEmitterVelocityButton;
	public XrpSlider LeftEmitterVelocitySpreadSlider;
	public AudioVisualisationButton LeftEmitterVelocitySpreadButton;
	public XrpSlider LeftEmitterInheritVelocitySlider;
	public AudioVisualisationButton LeftEmitterInheritVelocityButton;
	
	[Header("Right Emission")]
	public XrpSlider RightEmitterCountSlider;
	public XrpSlider RightEmitterRadiusSlider;
	public AudioVisualisationButton RightEmitterRadiusButton;
	public XrpSlider RightEmitterVelocitySlider;
	public AudioVisualisationButton RightEmitterVelocityButton;
	public XrpSlider RightEmitterVelocitySpreadSlider;
	public AudioVisualisationButton RightEmitterVelocitySpreadButton;
	public XrpSlider RightEmitterInheritVelocitySlider;
	public AudioVisualisationButton RightEmitterInheritVelocityButton;
	
	public void LoadFromJson(string jsonString)
	{
		_settings = Instantiate(SourceSettings.DefaultSettings);
		_settings.CopyValuesFrom(jsonString);

		ApplyJsonValues();
	}

	public void ApplyJsonValues()
	{
		MaxParticleCountSlider.CurrentValue = _settings.System.MaxParticleCount;
		MaxParticleCountSlider.Bang();
		AntialiasingDial.CurrentValue = (int)_settings.System.Antialiasing;
		AntialiasingDial.Bang();
		BloomToggle.CurrentValue = _settings.System.Bloom;
		BloomToggle.Bang();
	
		ParticleFormDial.CurrentValue = (int)_settings.Visual.ParticleForm;
		ParticleFormDial.Bang();
		ParticleColorDial.CurrentValue = (int)_settings.Visual.ParticleColor;
		ParticleColorDial.Bang();
		ParticleColorAmountSlider.CurrentValue = _settings.Visual.ParticleColorAmount;
		ParticleColorAmountSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.Visual.ParticleColorAmountVis);
		ParticleSizeSlider.CurrentValue = _settings.Visual.ParticleSize;
		ParticleSizeSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.Visual.ParticleSizeVis);
		LineLengthSlider.CurrentValue = _settings.Visual.LineLength;
		LineLengthSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.Visual.LineLengthVis);
		ParticleShapeSlider.CurrentValue = (int)_settings.Visual.ParticleShape;
		ParticleShapeSlider.Bang();
	
		ParticleMassSlider.CurrentValue = _settings.Physics.ParticleMass;
		ParticleMassSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.Physics.ParticleMassVis);
		ParticleDampingSlider.CurrentValue = _settings.Physics.ParticleDamping;
		ParticleDampingSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.Physics.ParticleDampingVis);
		TimeSpeedSlider.CurrentValue = _settings.Physics.TimeSpeed;
		TimeSpeedSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.Physics.TimeSpeedVis);
		FloorCollisionToggle.CurrentValue = _settings.Physics.FloorCollision;
		FloorCollisionToggle.Bang();
	
		VisualsAudioreactivitySlider.CurrentValue = _settings.Audio.VisualsAudioreactivity;
		VisualsAudioreactivitySlider.Bang();
		PhysicsAudioreactivitySlider.CurrentValue = _settings.Audio.PhysicsAudioreactivity;
		PhysicsAudioreactivitySlider.Bang();
		VolumeDial.CurrentValue = _settings.Audio.Volume;
		VolumeDial.Bang();
		LoopToggle.CurrentValue = _settings.Audio.Loop;
		LoopToggle.Bang();
		SlowWithTimeToggle.CurrentValue = _settings.Audio.SlowWithTime;
		SlowWithTimeToggle.Bang();
	
		ControllerSymmetrySlider.CurrentValue = (int)_settings.Controller.ControllerSymmetry;
		ControllerSymmetrySlider.Bang();
		ControllerModelsToggle.CurrentValue = _settings.Controller.ControllerModels;
		ControllerModelsToggle.Bang();
		ControllerDistanceSlider.CurrentValue = _settings.Controller.ControllerDistance;
		ControllerDistanceSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.Controller.ControllerDistanceVis);
		ControllerHapticsToggle.CurrentValue = _settings.Controller.ControllerHaptics;
		ControllerHapticsToggle.Bang();
	
		LeftForceShapeDial.CurrentValue = (int)_settings.ForceLeft.ForceShape;
		LeftForceShapeDial.Bang();
		LeftForceAttenuationDial.CurrentValue = (int)_settings.ForceLeft.ForceAttenuation;
		LeftForceAttenuationDial.Bang();
		LeftForceStrengthSlider.CurrentValue = _settings.ForceLeft.ForceStrength;
		LeftForceStrengthSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.ForceLeft.ForceStrengthVis);
		LeftForceAttenuationDistanceSlider.CurrentValue = _settings.ForceLeft.AttenuationDistance;
		LeftForceAttenuationDistanceSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.ForceLeft.AttenuationDistanceVis);
		LeftForceSofteningFactorSlider.CurrentValue = _settings.ForceLeft.SofteningFactor;
		LeftForceSofteningFactorSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.ForceLeft.SofteningFactorVis);
		LeftForceWavelengthSlider.CurrentValue = _settings.ForceLeft.Wavelength;
		LeftForceWavelengthSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.ForceLeft.WavelengthVis);
	
		RightForceShapeDial.CurrentValue = (int)_settings.ForceRight.ForceShape;
		RightForceShapeDial.Bang();
		RightForceAttenuationDial.CurrentValue = (int)_settings.ForceRight.ForceAttenuation;
		RightForceAttenuationDial.Bang();
		RightForceStrengthSlider.CurrentValue = _settings.ForceRight.ForceStrength;
		RightForceStrengthSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.ForceRight.ForceStrengthVis);
		RightForceAttenuationDistanceSlider.CurrentValue = _settings.ForceRight.AttenuationDistance;
		RightForceAttenuationDistanceSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.ForceRight.AttenuationDistanceVis);
		RightForceSofteningFactorSlider.CurrentValue = _settings.ForceRight.SofteningFactor;
		RightForceSofteningFactorSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.ForceRight.SofteningFactorVis);
		RightForceWavelengthSlider.CurrentValue = _settings.ForceRight.Wavelength;
		RightForceWavelengthSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.ForceRight.WavelengthVis);
		
		LeftEmitterCountSlider.CurrentValue = _settings.EmitterLeft.EmitterCount;
		LeftEmitterCountSlider.Bang();
		LeftEmitterRadiusSlider.CurrentValue = _settings.EmitterLeft.EmitterRadius;
		LeftEmitterRadiusSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.EmitterLeft.EmitterRadiusVis);
		LeftEmitterVelocitySlider.CurrentValue = _settings.EmitterLeft.EmitterVelocity;
		LeftEmitterVelocitySlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.EmitterLeft.EmitterVelocityVis);
		LeftEmitterVelocitySpreadSlider.CurrentValue = _settings.EmitterLeft.VelocitySpread;
		LeftEmitterVelocitySpreadSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.EmitterLeft.VelocitySpreadVis);
		LeftEmitterInheritVelocitySlider.CurrentValue = _settings.EmitterLeft.InheritVelocity;
		LeftEmitterInheritVelocitySlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.EmitterLeft.InheritVelocityVis);
		
		RightEmitterCountSlider.CurrentValue = _settings.EmitterRight.EmitterCount;
		RightEmitterCountSlider.Bang();
		RightEmitterRadiusSlider.CurrentValue = _settings.EmitterRight.EmitterRadius;
		RightEmitterRadiusSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.EmitterRight.EmitterRadiusVis);
		RightEmitterVelocitySlider.CurrentValue = _settings.EmitterRight.EmitterVelocity;
		RightEmitterVelocitySlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.EmitterRight.EmitterVelocityVis);
		RightEmitterVelocitySpreadSlider.CurrentValue = _settings.EmitterRight.VelocitySpread;
		RightEmitterVelocitySpreadSlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.EmitterRight.VelocitySpreadVis);
		RightEmitterInheritVelocitySlider.CurrentValue = _settings.EmitterRight.InheritVelocity;
		RightEmitterInheritVelocitySlider.Bang();
		ParticleColorAmountButton.SetSource((int)_settings.EmitterRight.InheritVelocityVis);
		
		ApplyParticleCountButton.Bang();
	}
}
