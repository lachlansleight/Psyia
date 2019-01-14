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
	public XrpSlider ParticleSizeSlider;
	public XrpSlider LineLengthSlider;
	public XrpEnumSlider ParticleShapeSlider;

	[Header("Physics")]
	public XrpSlider ParticleMassSlider;
	public XrpSlider ParticleDampingSlider;
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
	public XrpToggle ControllerHapticsToggle;

	[Header("Left Force")]
	public XrpIntDial LeftForceShapeDial;
	public XrpIntDial LeftForceAttenuationDial;
	public XrpSlider LeftForceStrengthSlider;
	public XrpSlider LeftForceAttenuationDistanceSlider;
	public XrpSlider LeftForceSofteningFactorSlider;
	public XrpSlider LeftForceWavelengthSlider;
	
	[Header("Right Force")]
	public XrpIntDial RightForceShapeDial;
	public XrpIntDial RightForceAttenuationDial;
	public XrpSlider RightForceStrengthSlider;
	public XrpSlider RightForceAttenuationDistanceSlider;
	public XrpSlider RightForceSofteningFactorSlider;
	public XrpSlider RightForceWavelengthSlider;

	[Header("Left Emission")]
	public XrpSlider LeftEmitterCountSlider;
	public XrpSlider LeftEmitterRadiusSlider;
	public XrpSlider LeftEmitterVelocitySlider;
	public XrpSlider LeftEmitterVelocitySpreadSlider;
	public XrpSlider LeftEmitterInheritVelocitySlider;
	
	[Header("Right Emission")]
	public XrpSlider RightEmitterCountSlider;
	public XrpSlider RightEmitterRadiusSlider;
	public XrpSlider RightEmitterVelocitySlider;
	public XrpSlider RightEmitterVelocitySpreadSlider;
	public XrpSlider RightEmitterInheritVelocitySlider;
	
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
		ParticleSizeSlider.CurrentValue = _settings.Visual.ParticleSize;
		ParticleSizeSlider.Bang();
		LineLengthSlider.CurrentValue = _settings.Visual.LineLength;
		LineLengthSlider.Bang();
		ParticleShapeSlider.CurrentValue = (int)_settings.Visual.ParticleShape;
		ParticleShapeSlider.Bang();
	
		ParticleMassSlider.CurrentValue = _settings.Physics.ParticleMass;
		ParticleMassSlider.Bang();
		ParticleDampingSlider.CurrentValue = _settings.Physics.ParticleDamping;
		ParticleDampingSlider.Bang();
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
		ControllerHapticsToggle.CurrentValue = _settings.Controller.ControllerHaptics;
		ControllerHapticsToggle.Bang();
	
		LeftForceShapeDial.CurrentValue = (int)_settings.ForceLeft.ForceShape;
		LeftForceShapeDial.Bang();
		LeftForceAttenuationDial.CurrentValue = (int)_settings.ForceLeft.ForceAttenuation;
		LeftForceAttenuationDial.Bang();
		LeftForceStrengthSlider.CurrentValue = _settings.ForceLeft.ForceStrength;
		LeftForceStrengthSlider.Bang();
		LeftForceAttenuationDistanceSlider.CurrentValue = _settings.ForceLeft.AttenuationDistance;
		LeftForceAttenuationDistanceSlider.Bang();
		LeftForceSofteningFactorSlider.CurrentValue = _settings.ForceLeft.SofteningFactor;
		LeftForceSofteningFactorSlider.Bang();
		LeftForceWavelengthSlider.CurrentValue = _settings.ForceLeft.Wavelength;
		LeftForceWavelengthSlider.Bang();
	
		RightForceShapeDial.CurrentValue = (int)_settings.ForceRight.ForceShape;
		RightForceShapeDial.Bang();
		RightForceAttenuationDial.CurrentValue = (int)_settings.ForceRight.ForceAttenuation;
		RightForceAttenuationDial.Bang();
		RightForceStrengthSlider.CurrentValue = _settings.ForceRight.ForceStrength;
		RightForceStrengthSlider.Bang();
		RightForceAttenuationDistanceSlider.CurrentValue = _settings.ForceRight.AttenuationDistance;
		RightForceAttenuationDistanceSlider.Bang();
		RightForceSofteningFactorSlider.CurrentValue = _settings.ForceRight.SofteningFactor;
		RightForceSofteningFactorSlider.Bang();
		RightForceWavelengthSlider.CurrentValue = _settings.ForceRight.Wavelength;
		RightForceWavelengthSlider.Bang();
		
		LeftEmitterCountSlider.CurrentValue = _settings.EmitterLeft.EmitterCount;
		LeftEmitterCountSlider.Bang();
		LeftEmitterRadiusSlider.CurrentValue = _settings.EmitterLeft.EmitterRadius;
		LeftEmitterRadiusSlider.Bang();
		LeftEmitterVelocitySlider.CurrentValue = _settings.EmitterLeft.EmitterVelocity;
		LeftEmitterVelocitySlider.Bang();
		LeftEmitterVelocitySpreadSlider.CurrentValue = _settings.EmitterLeft.VelocitySpread;
		LeftEmitterVelocitySpreadSlider.Bang();
		LeftEmitterInheritVelocitySlider.CurrentValue = _settings.EmitterLeft.InheritVelocity;
		LeftEmitterInheritVelocitySlider.Bang();
		
		RightEmitterCountSlider.CurrentValue = _settings.EmitterRight.EmitterCount;
		RightEmitterCountSlider.Bang();
		RightEmitterRadiusSlider.CurrentValue = _settings.EmitterRight.EmitterRadius;
		RightEmitterRadiusSlider.Bang();
		RightEmitterVelocitySlider.CurrentValue = _settings.EmitterRight.EmitterVelocity;
		RightEmitterVelocitySlider.Bang();
		RightEmitterVelocitySpreadSlider.CurrentValue = _settings.EmitterRight.VelocitySpread;
		RightEmitterVelocitySpreadSlider.Bang();
		RightEmitterInheritVelocitySlider.CurrentValue = _settings.EmitterRight.InheritVelocity;
		RightEmitterInheritVelocitySlider.Bang();
		
		ApplyParticleCountButton.Bang();
	}
}
