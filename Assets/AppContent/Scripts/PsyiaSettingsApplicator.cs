using System;
using System.Collections;
using System.Collections.Generic;
using Psyia;
using UCTK;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PsyiaSettingsApplicator : MonoBehaviour
{
	public PsyiaSettings DefaultSettings;
	public PsyiaSettings CurrentSettings;
	public TextAsset TestJson;

	[Header("System Objects")]
	public ParticleCountManager CountManager;
	public PostProcessVolume PostVolume;
	public PsyiaEmitter ResetEmitter;
	
	[Header("Visual Objects")]
	public DispatchQueue ParticleQueue;
	public Material[] ParticleMaterials;
	public Texture2D[] ParticleTextures;
	public Texture2D[] ColorTextures;
	public ComputeRenderer TargetRenderer;
	public ComputeShader ColorShader;

	[Header("Physics Objects")]
	public PhysicsManager PhysicsManager;
	public GameObject FloorVisuals;

	[Header("Audio Objects")]
	public PsyiaMusic Music;

	[Header("Controller Objects")]
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
	
	public void Awake()
	{
		CurrentSettings = Instantiate(DefaultSettings);
		ApplyDefaultSettings();
		
	}
	
	public void SetMaxParticleCount(int newValue, bool skipChangeCheck = false)
	{
		if (CurrentSettings.System.MaxParticleCount == newValue && !skipChangeCheck) return;
		
		CurrentSettings.System.MaxParticleCount = newValue;
	}
	public void ApplyParticleCount(bool skipChangeCheck = false)
	{
		CountManager.ParticleCountFactor = CurrentSettings.System.MaxParticleCount;
		CountManager.ApplyParticleCount();
	}
	public void SetAntialiasing(int newValue, bool skipChangeCheck = false)
	{
		if (CurrentSettings.System.Antialiasing == newValue && !skipChangeCheck) return;
		
		CurrentSettings.System.Antialiasing = newValue;
		QualitySettings.antiAliasing = CurrentSettings.System.Antialiasing;
	}
	public void SetBloom(bool newValue, bool skipChangeCheck = false)
	{
		if (CurrentSettings.System.Bloom == newValue && !skipChangeCheck) return;
		
		CurrentSettings.System.Bloom = newValue;
		
		Bloom bloomLayer;
		if (PostVolume.profile.TryGetSettings(out bloomLayer)) {
			bloomLayer.enabled.value = newValue;
		}
	}

	public void SetParticleForm(int newValue, bool skipChangeCheck = false)
	{
		if ((int) CurrentSettings.Visual.ParticleForm == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Visual.ParticleForm = (PsyiaVisualSettings.Form)newValue;
		ParticleQueue.transform.GetChild(2).gameObject.SetActive((int) CurrentSettings.Visual.ParticleForm != 2);
		ParticleQueue.transform.GetChild(3).gameObject.SetActive((int) CurrentSettings.Visual.ParticleForm != 2);
		TargetRenderer.RenderMaterial = ParticleMaterials[(int) CurrentSettings.Visual.ParticleForm];
	}

	public void SetParticleColor(int newValue, bool skipChangeCheck = false)
	{
		if ((int) CurrentSettings.Visual.ParticleColor == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Visual.ParticleColor = (PsyiaVisualSettings.Color) newValue;
		if ((int) CurrentSettings.Visual.ParticleColor >= 2) {
			var colors = GetColorSelection(ColorTextures[(int) CurrentSettings.Visual.ParticleColor]);
			ColorShader.SetVector("xColorMin", colors[0]);
			ColorShader.SetVector("xColorMax", colors[1]);
			ColorShader.SetVector("yColorMin", colors[2]);
			ColorShader.SetVector("yColorMax", colors[3]);
			ColorShader.SetVector("zColorMin", colors[4]);
			ColorShader.SetVector("zColorMax", colors[5]);
		}

		ColorShader.SetInt("ColorMode", (int) CurrentSettings.Visual.ParticleColor);
	}

	
	public void SetParticleSize(float newValue, bool skipChangeCheck = false)
	{
		if (CurrentSettings.Visual.ParticleSize == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Visual.ParticleSize = newValue;
		foreach(var m in ParticleMaterials) m.SetFloat("_PointSize", CurrentSettings.Visual.ParticleSize);
	}
	
	public void SetLineLength(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Visual.LineLength == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Visual.LineLength = newValue;
		foreach(var m in ParticleMaterials) m.SetFloat("_LineLength", CurrentSettings.Visual.LineLength);
	}
	public void SetParticleShape(int newValue, bool skipChangeCheck = false)
	{
		if((int)CurrentSettings.Visual.ParticleShape == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Visual.ParticleShape = (PsyiaVisualSettings.Shape)newValue;
		foreach(var m in ParticleMaterials) m.SetTexture("_Image", ParticleTextures[(int)CurrentSettings.Visual.ParticleShape]);
	}

	public void SetParticleMass(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Physics.ParticleMass == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Physics.ParticleMass = newValue;
		PhysicsManager.ParticleMinimumMass = CurrentSettings.Physics.ParticleMass;
	}
	public void SetParticleDamping(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Physics.ParticleDamping == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Physics.ParticleDamping = newValue;
		PhysicsManager.ParticleDrag = CurrentSettings.Physics.ParticleDamping;
	}
	public void SetFloorCollision(bool newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Physics.FloorCollision == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Physics.FloorCollision = newValue;
		PhysicsManager.FloorCollision = CurrentSettings.Physics.FloorCollision;
		FloorVisuals.SetActive(CurrentSettings.Physics.FloorCollision);
	}

	public void SetVisualAudioreactivity(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Audio.VisualsAudioreactivity == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Audio.VisualsAudioreactivity = newValue;
		//TODO: Implement this
		//PsyiaSystem.VisualReactivityStrength = Settings.Audio.VisualAudioreactivity;
	}
	public void SetPhysicsAudioreactivity(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Audio.PhysicsAudioreactivity == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Audio.PhysicsAudioreactivity = newValue;
		//TODO: Implement this
		//PsyiaSystem.PhysicsReactivityStrength = Settings.Audio.PhysicsAudioreactivity;
	}
	public void SetVolume(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Audio.Volume == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Audio.Volume = newValue;
		Music.Volume = CurrentSettings.Audio.Volume;
	}
	public void SetLoop(bool newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Audio.Loop == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Audio.Loop = newValue;
		Music.Loop = CurrentSettings.Audio.Loop;
	}
	public void SetSlowWithTime(bool newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Audio.SlowWithTime == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Audio.SlowWithTime = newValue;
		//TODO: Implement this
		Music.SlowWithTime = CurrentSettings.Audio.SlowWithTime;
	}

	public void SetSymmetry(int newValue, bool skipChangeCheck = false)
	{
		if((int)CurrentSettings.Controller.ControllerSymmetry == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Controller.ControllerSymmetry = (PsyiaControllerSettings.Symmetry)newValue;
		LeftSymmetry.enabled = (int)CurrentSettings.Controller.ControllerSymmetry == 2;
		RightSymmetry.enabled = (int)CurrentSettings.Controller.ControllerSymmetry == 0;
	}
	public void SetControllerModels(bool newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Controller.ControllerModels == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Controller.ControllerModels = newValue;
		LeftController.ShowFullModel = RightController.ShowFullModel = CurrentSettings.Controller.ControllerModels;
	}
	public void SetControllerDistance(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Controller.ControllerDistance == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Controller.ControllerDistance = newValue;
		LeftController.ControllerDistance = RightController.ControllerDistance = CurrentSettings.Controller.ControllerDistance;
	}
	public void SetControllerHaptics(bool newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.Controller.ControllerHaptics == newValue && !skipChangeCheck) return;
		
		CurrentSettings.Controller.ControllerHaptics = newValue;
		LeftHaptics.enabled = RightHaptics.enabled = CurrentSettings.Controller.ControllerHaptics;
	}

	public void SetLeftForceShape(int newValue, bool skipChangeCheck = false)
	{
		if((int)CurrentSettings.ForceLeft.ForceShape == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceLeft.ForceShape = (PsyiaForceSettings.Shape)newValue;
		LeftForce.Shape = (PsyiaForce.ForceShape) CurrentSettings.ForceLeft.ForceShape;
	}
	public void SetLeftForceAttenuationMode(int newValue, bool skipChangeCheck = false)
	{
		if((int)CurrentSettings.ForceLeft.ForceAttenuation == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceLeft.ForceAttenuation = (PsyiaForceSettings.Attenuation)newValue;
		LeftForce.AttenuationMode = (PsyiaForce.ForceAttenuationMode) CurrentSettings.ForceLeft.ForceAttenuation;
		var mode = (int) CurrentSettings.ForceLeft.ForceAttenuation;
		if (mode > 0 && mode < 4) 
			LeftForce.AttenuationDistance = CurrentSettings.ForceLeft.AttenuationDistance;
		else if (mode > 3 && mode < 6)
			LeftForce.AttenuationDistance = CurrentSettings.ForceLeft.SofteningFactor;
		else if (mode == 6)
			LeftForce.AttenuationDistance = CurrentSettings.ForceLeft.Wavelength;
	}
	public void SetLeftForceStrength(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.ForceLeft.ForceStrength == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceLeft.ForceStrength = newValue;
		LeftForce.Strength = CurrentSettings.ForceLeft.ForceStrength;
	}
	public void SetLeftForceAttenuationDistance(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.ForceLeft.AttenuationDistance == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceLeft.AttenuationDistance = newValue;
		LeftForce.AttenuationDistance = CurrentSettings.ForceLeft.AttenuationDistance;
	}
	public void SetLeftForceAttenuationSofteningFactor(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.ForceLeft.SofteningFactor == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceLeft.SofteningFactor = newValue;
		LeftForce.AttenuationDistance = CurrentSettings.ForceLeft.SofteningFactor;
	}
	public void SetLeftForceAttenuationWavelength(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.ForceLeft.Wavelength == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceLeft.Wavelength = newValue;
		LeftForce.AttenuationDistance = CurrentSettings.ForceLeft.Wavelength;
	}
	
	public void SetRightForceShape(int newValue, bool skipChangeCheck = false)
	{
		if((int)CurrentSettings.ForceRight.ForceShape == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceRight.ForceShape = (PsyiaForceSettings.Shape)newValue;
		RightForce.Shape = (PsyiaForce.ForceShape) CurrentSettings.ForceRight.ForceShape;
	}
	public void SetRightForceAttenuationMode(int newValue, bool skipChangeCheck = false)
	{
		if((int)CurrentSettings.ForceRight.ForceAttenuation == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceRight.ForceAttenuation = (PsyiaForceSettings.Attenuation)newValue;
		RightForce.AttenuationMode = (PsyiaForce.ForceAttenuationMode) CurrentSettings.ForceRight.ForceAttenuation;
		var mode = (int) CurrentSettings.ForceRight.ForceAttenuation;
		if (mode > 0 && mode < 4) 
			RightForce.AttenuationDistance = CurrentSettings.ForceRight.AttenuationDistance;
		else if (mode > 3 && mode < 6)
			RightForce.AttenuationDistance = CurrentSettings.ForceRight.SofteningFactor;
		else if (mode == 6)
			RightForce.AttenuationDistance = CurrentSettings.ForceRight.Wavelength;
	}
	public void SetRightForceStrength(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.ForceRight.ForceStrength == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceRight.ForceStrength = newValue;
		RightForce.Strength = CurrentSettings.ForceRight.ForceStrength;
	}
	public void SetRightForceAttenuationDistance(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.ForceRight.AttenuationDistance == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceRight.AttenuationDistance = newValue;
		RightForce.AttenuationDistance = CurrentSettings.ForceRight.AttenuationDistance;
	}
	public void SetRightForceAttenuationSofteningFactor(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.ForceRight.SofteningFactor == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceRight.SofteningFactor = newValue;
		RightForce.AttenuationDistance = CurrentSettings.ForceRight.SofteningFactor;
	}
	public void SetRightForceAttenuationWavelength(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.ForceRight.Wavelength == newValue && !skipChangeCheck) return;
		
		CurrentSettings.ForceRight.Wavelength = newValue;
		RightForce.AttenuationDistance = CurrentSettings.ForceRight.Wavelength;
	}
	public void SetLeftEmitterCount(int newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.EmitterLeft.EmitterCount == newValue && !skipChangeCheck) return;
		
		CurrentSettings.EmitterLeft.EmitterCount = newValue;
		LeftEmitter.EmitOverTime = CurrentSettings.EmitterLeft.EmitterCount;
	}
	public void SetLeftEmitterRadius(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.EmitterLeft.EmitterRadius == newValue && !skipChangeCheck) return;
		
		CurrentSettings.EmitterLeft.EmitterRadius = newValue;
		LeftEmitter.Settings.EmissionRadius = CurrentSettings.EmitterLeft.EmitterRadius;
	}
	public void SetLeftEmitterVelocity(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.EmitterLeft.EmitterVelocity == newValue && !skipChangeCheck) return;
		
		CurrentSettings.EmitterLeft.EmitterVelocity = newValue;
		LeftEmitter.Settings.MinSpawnVelocity = CurrentSettings.EmitterLeft.EmitterVelocity * 0.5f;
		LeftEmitter.Settings.MaxSpawnVelocity = CurrentSettings.EmitterLeft.EmitterVelocity * 1.5f;
	}
	public void SetLeftEmitterVelocitySpread(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.EmitterLeft.VelocitySpread == newValue && !skipChangeCheck) return;
		
		CurrentSettings.EmitterLeft.VelocitySpread = newValue;
		LeftEmitter.Settings.RandomiseDirection = CurrentSettings.EmitterLeft.VelocitySpread;
	}
	public void SetLeftEmitterInheritVelocity(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.EmitterLeft.InheritVelocity == newValue && !skipChangeCheck) return;
		
		CurrentSettings.EmitterLeft.InheritVelocity = newValue;
		LeftEmitter.Settings.InheritVelocity = CurrentSettings.EmitterLeft.InheritVelocity;
	}
	
	public void SetRightEmitterCount(int newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.EmitterRight.EmitterCount == newValue && !skipChangeCheck) return;
		
		CurrentSettings.EmitterRight.EmitterCount = newValue;
		RightEmitter.EmitOverTime = CurrentSettings.EmitterRight.EmitterCount;
	}
	public void SetRightEmitterRadius(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.EmitterRight.EmitterRadius == newValue && !skipChangeCheck) return;
		
		CurrentSettings.EmitterRight.EmitterRadius = newValue;
		RightEmitter.Settings.EmissionRadius = CurrentSettings.EmitterRight.EmitterRadius;
	}
	public void SetRightEmitterVelocity(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.EmitterRight.EmitterVelocity == newValue && !skipChangeCheck) return;
		
		CurrentSettings.EmitterRight.EmitterVelocity = newValue;
		RightEmitter.Settings.MinSpawnVelocity = CurrentSettings.EmitterRight.EmitterVelocity * 0.5f;
		RightEmitter.Settings.MaxSpawnVelocity = CurrentSettings.EmitterRight.EmitterVelocity * 1.5f;
	}
	public void SetRightEmitterVelocitySpread(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.EmitterRight.VelocitySpread == newValue && !skipChangeCheck) return;
		
		CurrentSettings.EmitterRight.VelocitySpread = newValue;
		RightEmitter.Settings.RandomiseDirection = CurrentSettings.EmitterRight.VelocitySpread;
	}
	public void SetRightEmitterInheritVelocity(float newValue, bool skipChangeCheck = false)
	{
		if(CurrentSettings.EmitterRight.InheritVelocity == newValue && !skipChangeCheck) return;
		
		CurrentSettings.EmitterRight.InheritVelocity = newValue;
		RightEmitter.Settings.InheritVelocity = CurrentSettings.EmitterRight.InheritVelocity;
	}
	
	
	
	public void ApplySettings()
	{
		SetMaxParticleCount(CurrentSettings.System.MaxParticleCount, false);
		SetAntialiasing(CurrentSettings.System.Antialiasing, true);
		SetBloom(CurrentSettings.System.Bloom, true);

		SetParticleForm((int)CurrentSettings.Visual.ParticleForm, true);
		SetParticleColor((int) CurrentSettings.Visual.ParticleColor, true);
		SetParticleSize(CurrentSettings.Visual.ParticleSize, true);
		SetLineLength(CurrentSettings.Visual.LineLength, true);
		SetParticleShape((int) CurrentSettings.Visual.ParticleShape, true);

		SetParticleMass(CurrentSettings.Physics.ParticleMass, true);
		SetParticleDamping(CurrentSettings.Physics.ParticleDamping, true);
		SetFloorCollision(CurrentSettings.Physics.FloorCollision, true);

		SetVisualAudioreactivity(CurrentSettings.Audio.VisualsAudioreactivity, true);
		SetPhysicsAudioreactivity(CurrentSettings.Audio.PhysicsAudioreactivity, true);
		SetVolume(CurrentSettings.Audio.Volume, true);
		SetLoop(CurrentSettings.Audio.Loop, true);
		SetSlowWithTime(CurrentSettings.Audio.SlowWithTime, true);

		SetSymmetry((int)CurrentSettings.Controller.ControllerSymmetry, true);
		SetControllerModels(CurrentSettings.Controller.ControllerModels, true);
		SetControllerDistance(CurrentSettings.Controller.ControllerDistance, true);
		SetControllerHaptics(CurrentSettings.Controller.ControllerHaptics, true);

		SetLeftForceShape((int) CurrentSettings.ForceLeft.ForceShape, true);
		SetLeftForceAttenuationMode((int) CurrentSettings.ForceLeft.ForceAttenuation, true);
		SetLeftForceStrength(CurrentSettings.ForceLeft.ForceStrength, true);
		SetLeftForceAttenuationDistance(CurrentSettings.ForceLeft.AttenuationDistance, true);
		SetLeftForceAttenuationSofteningFactor(CurrentSettings.ForceLeft.SofteningFactor, true);
		SetLeftForceAttenuationWavelength(CurrentSettings.ForceLeft.Wavelength, true);
		
		SetRightForceShape((int) CurrentSettings.ForceRight.ForceShape, true);
		SetRightForceAttenuationMode((int) CurrentSettings.ForceRight.ForceAttenuation, true);
		SetRightForceStrength(CurrentSettings.ForceRight.ForceStrength, true);
		SetRightForceAttenuationDistance(CurrentSettings.ForceRight.AttenuationDistance, true);
		SetRightForceAttenuationSofteningFactor(CurrentSettings.ForceRight.SofteningFactor, true);
		SetRightForceAttenuationWavelength(CurrentSettings.ForceRight.Wavelength, true);

		SetLeftEmitterCount(CurrentSettings.EmitterLeft.EmitterCount, true);
		SetLeftEmitterRadius(CurrentSettings.EmitterLeft.EmitterRadius, true);
		SetLeftEmitterVelocity(CurrentSettings.EmitterLeft.EmitterVelocity, true);
		SetLeftEmitterVelocitySpread(CurrentSettings.EmitterLeft.VelocitySpread, true);
		SetLeftEmitterInheritVelocity(CurrentSettings.EmitterLeft.InheritVelocity, true);
		
		SetRightEmitterCount(CurrentSettings.EmitterRight.EmitterCount, true);
		SetRightEmitterRadius(CurrentSettings.EmitterRight.EmitterRadius, true);
		SetRightEmitterVelocity(CurrentSettings.EmitterRight.EmitterVelocity, true);
		SetRightEmitterVelocitySpread(CurrentSettings.EmitterRight.VelocitySpread, true);
		SetRightEmitterInheritVelocity(CurrentSettings.EmitterRight.InheritVelocity, true);
	}

	public void ApplyDefaultSettings()
	{
		SetMaxParticleCount(DefaultSettings.System.MaxParticleCount, false);
		SetAntialiasing(DefaultSettings.System.Antialiasing, true);
		SetBloom(DefaultSettings.System.Bloom, true);

		SetParticleForm((int)DefaultSettings.Visual.ParticleForm, true);
		SetParticleColor((int) DefaultSettings.Visual.ParticleColor, true);
		SetParticleSize(DefaultSettings.Visual.ParticleSize, true);
		SetLineLength(DefaultSettings.Visual.LineLength, true);
		SetParticleShape((int) DefaultSettings.Visual.ParticleShape, true);

		SetParticleMass(DefaultSettings.Physics.ParticleMass, true);
		SetParticleDamping(DefaultSettings.Physics.ParticleDamping, true);
		SetFloorCollision(DefaultSettings.Physics.FloorCollision, true);

		SetVisualAudioreactivity(DefaultSettings.Audio.VisualsAudioreactivity, true);
		SetPhysicsAudioreactivity(DefaultSettings.Audio.PhysicsAudioreactivity, true);
		SetVolume(DefaultSettings.Audio.Volume, true);
		SetLoop(DefaultSettings.Audio.Loop, true);
		SetSlowWithTime(DefaultSettings.Audio.SlowWithTime, true);

		SetSymmetry((int)DefaultSettings.Controller.ControllerSymmetry, true);
		SetControllerModels(DefaultSettings.Controller.ControllerModels, true);
		SetControllerDistance(DefaultSettings.Controller.ControllerDistance, true);
		SetControllerHaptics(DefaultSettings.Controller.ControllerHaptics, true);

		SetLeftForceShape((int) DefaultSettings.ForceLeft.ForceShape, true);
		SetLeftForceAttenuationMode((int) DefaultSettings.ForceLeft.ForceAttenuation, true);
		SetLeftForceStrength(DefaultSettings.ForceLeft.ForceStrength, true);
		SetLeftForceAttenuationDistance(DefaultSettings.ForceLeft.AttenuationDistance, true);
		SetLeftForceAttenuationSofteningFactor(DefaultSettings.ForceLeft.SofteningFactor, true);
		SetLeftForceAttenuationWavelength(DefaultSettings.ForceLeft.Wavelength, true);
		
		SetRightForceShape((int) DefaultSettings.ForceRight.ForceShape, true);
		SetRightForceAttenuationMode((int) DefaultSettings.ForceRight.ForceAttenuation, true);
		SetRightForceStrength(DefaultSettings.ForceRight.ForceStrength, true);
		SetRightForceAttenuationDistance(DefaultSettings.ForceRight.AttenuationDistance, true);
		SetRightForceAttenuationSofteningFactor(DefaultSettings.ForceRight.SofteningFactor, true);
		SetRightForceAttenuationWavelength(DefaultSettings.ForceRight.Wavelength, true);

		SetLeftEmitterCount(DefaultSettings.EmitterLeft.EmitterCount, true);
		SetLeftEmitterRadius(DefaultSettings.EmitterLeft.EmitterRadius, true);
		SetLeftEmitterVelocity(DefaultSettings.EmitterLeft.EmitterVelocity, true);
		SetLeftEmitterVelocitySpread(DefaultSettings.EmitterLeft.VelocitySpread, true);
		SetLeftEmitterInheritVelocity(DefaultSettings.EmitterLeft.InheritVelocity, true);
		
		SetRightEmitterCount(DefaultSettings.EmitterRight.EmitterCount, true);
		SetRightEmitterRadius(DefaultSettings.EmitterRight.EmitterRadius, true);
		SetRightEmitterVelocity(DefaultSettings.EmitterRight.EmitterVelocity, true);
		SetRightEmitterVelocitySpread(DefaultSettings.EmitterRight.VelocitySpread, true);
		SetRightEmitterInheritVelocity(DefaultSettings.EmitterRight.InheritVelocity, true);
	}

	public void SetNewDefault(PsyiaSettings newSettings)
	{
		DefaultSettings = newSettings;

		SetMaxParticleCount(DefaultSettings.System.MaxParticleCount, false);
		SetAntialiasing(DefaultSettings.System.Antialiasing, true);
		SetBloom(DefaultSettings.System.Bloom, true);

		SetParticleForm((int)DefaultSettings.Visual.ParticleForm, true);
		SetParticleColor((int) DefaultSettings.Visual.ParticleColor, true);
		SetParticleSize(DefaultSettings.Visual.ParticleSize, true);
		SetLineLength(DefaultSettings.Visual.LineLength, true);
		SetParticleShape((int) DefaultSettings.Visual.ParticleShape, true);

		SetParticleMass(DefaultSettings.Physics.ParticleMass, true);
		SetParticleDamping(DefaultSettings.Physics.ParticleDamping, true);
		SetFloorCollision(DefaultSettings.Physics.FloorCollision, true);

		SetVisualAudioreactivity(DefaultSettings.Audio.VisualsAudioreactivity, true);
		SetPhysicsAudioreactivity(DefaultSettings.Audio.PhysicsAudioreactivity, true);
		SetVolume(DefaultSettings.Audio.Volume, true);
		SetLoop(DefaultSettings.Audio.Loop, true);
		SetSlowWithTime(DefaultSettings.Audio.SlowWithTime, true);

		SetSymmetry((int)DefaultSettings.Controller.ControllerSymmetry, true);
		SetControllerModels(DefaultSettings.Controller.ControllerModels, true);
		SetControllerDistance(DefaultSettings.Controller.ControllerDistance, true);
		SetControllerHaptics(DefaultSettings.Controller.ControllerHaptics, true);

		SetLeftForceShape((int) DefaultSettings.ForceLeft.ForceShape, true);
		SetLeftForceAttenuationMode((int) DefaultSettings.ForceLeft.ForceAttenuation, true);
		SetLeftForceStrength(DefaultSettings.ForceLeft.ForceStrength, true);
		SetLeftForceAttenuationDistance(DefaultSettings.ForceLeft.AttenuationDistance, true);
		SetLeftForceAttenuationSofteningFactor(DefaultSettings.ForceLeft.SofteningFactor, true);
		SetLeftForceAttenuationWavelength(DefaultSettings.ForceLeft.Wavelength, true);
		
		SetRightForceShape((int) DefaultSettings.ForceRight.ForceShape, true);
		SetRightForceAttenuationMode((int) DefaultSettings.ForceRight.ForceAttenuation, true);
		SetRightForceStrength(DefaultSettings.ForceRight.ForceStrength, true);
		SetRightForceAttenuationDistance(DefaultSettings.ForceRight.AttenuationDistance, true);
		SetRightForceAttenuationSofteningFactor(DefaultSettings.ForceRight.SofteningFactor, true);
		SetRightForceAttenuationWavelength(DefaultSettings.ForceRight.Wavelength, true);

		SetLeftEmitterCount(DefaultSettings.EmitterLeft.EmitterCount, true);
		SetLeftEmitterRadius(DefaultSettings.EmitterLeft.EmitterRadius, true);
		SetLeftEmitterVelocity(DefaultSettings.EmitterLeft.EmitterVelocity, true);
		SetLeftEmitterVelocitySpread(DefaultSettings.EmitterLeft.VelocitySpread, true);
		SetLeftEmitterInheritVelocity(DefaultSettings.EmitterLeft.InheritVelocity, true);
		
		SetRightEmitterCount(DefaultSettings.EmitterRight.EmitterCount, true);
		SetRightEmitterRadius(DefaultSettings.EmitterRight.EmitterRadius, true);
		SetRightEmitterVelocity(DefaultSettings.EmitterRight.EmitterVelocity, true);
		SetRightEmitterVelocitySpread(DefaultSettings.EmitterRight.VelocitySpread, true);
		SetRightEmitterInheritVelocity(DefaultSettings.EmitterRight.InheritVelocity, true);
	}

	public string SaveCurrentSettings()
	{
		return CurrentSettings.GetSettingsJson();
	}

	#if UNITY_EDITOR
	[ContextMenu("Output to Project Directory")]
	public void DebugOutputJson()
	{
		var savePath = "C:\\Users\\Lachlan\\Development\\GitHub\\Psyia\\Assets\\AppContent\\Settings\\JSON\\";
			
		var sw = new System.IO.StreamWriter(savePath + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json");
		sw.Write(SaveCurrentSettings());
		sw.Close();
		sw.Dispose();
	}

	[ContextMenu("Apply Json Settings")]
	public void ApplyTestJson()
	{
		CurrentSettings.CopyValuesFrom(TestJson.text);
		ApplySettings();
		ApplyParticleCount();
		//ResetEmitter.Emit(ResetEmitter.StartEmitCount);
	}
	#endif

	private Color[] GetColorSelection(Texture2D texture)
	{
		var xColorMin = texture.GetPixel(32, 32);
		var xColorMax = texture.GetPixel(96, 64);
		var yColorMin = texture.GetPixel(32, 64);
		var yColorMax = texture.GetPixel(96, 96);
		var zColorMin = texture.GetPixel(32, 96);
		var zColorMax = texture.GetPixel(96, 32);

		return new[]
		{
			xColorMin,
			xColorMax,
			yColorMin,
			yColorMax,
			zColorMin,
			zColorMax
		};
	}

	

	
	
}
