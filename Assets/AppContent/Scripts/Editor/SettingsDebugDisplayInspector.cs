using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SettingsDebugDisplay))]
public class SettingsDebugDisplayInspector : Editor
{
	public override void OnInspectorGUI()
	{
		var settingsDisplay = (SettingsDebugDisplay) target;
		if (settingsDisplay.SettingsApplicator == null) {
			EditorGUILayout.LabelField("Settings applicator is null");
			return;
		}

		var settings = settingsDisplay.SettingsApplicator.CurrentSettings;
		if (settings == null) {
			EditorGUILayout.LabelField("Settings not initialized");
			return;
		}

		EditorGUILayout.LabelField("System Settings", EditorStyles.boldLabel);
		EditorGUILayout.IntField("Max Particle Count", settings.System.MaxParticleCount);
		EditorGUILayout.EnumPopup("Antialiasing", settings.System.Antialiasing);
		EditorGUILayout.Toggle("Bloom", settings.System.Bloom);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Visual Settings", EditorStyles.boldLabel);
		EditorGUILayout.EnumPopup("Particle Form", settings.Visual.ParticleForm);
		EditorGUILayout.EnumPopup("Particle Colour", settings.Visual.ParticleColor);
		EditorGUILayout.FloatField("Color Amount", settings.Visual.ParticleColorAmount);
		EditorGUILayout.EnumPopup("Color Amount Vis", settings.Visual.ParticleColorAmountVis);
		EditorGUILayout.FloatField("Particle Size", settings.Visual.ParticleSize);
		EditorGUILayout.EnumPopup("Particle Size Vis", settings.Visual.ParticleSizeVis);
		EditorGUILayout.FloatField("Line Length", settings.Visual.LineLength);
		EditorGUILayout.EnumPopup("Line Length Vis", settings.Visual.LineLengthVis);
		EditorGUILayout.EnumPopup("Particle Shape", settings.Visual.ParticleShape);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Physics Settings", EditorStyles.boldLabel);
		EditorGUILayout.FloatField("ParticleMass", settings.Physics.ParticleMass);
		EditorGUILayout.EnumPopup("ParticleMassVis", settings.Physics.ParticleMassVis);
		EditorGUILayout.FloatField("ParticleDamping", settings.Physics.ParticleDamping);
		EditorGUILayout.EnumPopup("ParticleDampingVis", settings.Physics.ParticleDampingVis);
		EditorGUILayout.FloatField("TimeSpeed", settings.Physics.TimeSpeed);
		EditorGUILayout.EnumPopup("TimeSpeedVis", settings.Physics.TimeSpeedVis);
		EditorGUILayout.Toggle("FloorCollision", settings.Physics.FloorCollision);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Audio Settings", EditorStyles.boldLabel);	
		EditorGUILayout.FloatField("VisualsAudioreactivity", settings.Audio.VisualsAudioreactivity);
		EditorGUILayout.FloatField("PhysicsAudioreactivity", settings.Audio.PhysicsAudioreactivity);
		EditorGUILayout.FloatField("Volume", settings.Audio.Volume);
		EditorGUILayout.Toggle("Loop", settings.Audio.Loop);
		EditorGUILayout.Toggle("SlowWithTime", settings.Audio.SlowWithTime);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Controller Settings", EditorStyles.boldLabel);
		EditorGUILayout.EnumPopup("ControllerSymmetry", settings.Controller.ControllerSymmetry);
		EditorGUILayout.Toggle("ControllerModels", settings.Controller.ControllerModels);
		EditorGUILayout.FloatField("ControllerDistance", settings.Controller.ControllerDistance);
		EditorGUILayout.EnumPopup("ControllerDistanceVis", settings.Controller.ControllerDistanceVis);
		EditorGUILayout.Toggle("ControllerHaptics", settings.Controller.ControllerHaptics);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Left Force Settings", EditorStyles.boldLabel);
		EditorGUILayout.EnumPopup("ForceShape", settings.ForceLeft.ForceShape);
		EditorGUILayout.EnumPopup("ForceAttenuation", settings.ForceLeft.ForceAttenuation);
		EditorGUILayout.FloatField("ForceStrength", settings.ForceLeft.ForceStrength);
		EditorGUILayout.EnumPopup("ForceStrengthVis", settings.ForceLeft.ForceStrengthVis);
		EditorGUILayout.FloatField("AttenuationDistance", settings.ForceLeft.AttenuationDistance);
		EditorGUILayout.EnumPopup("AttenuationDistanceVis", settings.ForceLeft.AttenuationDistanceVis);
		EditorGUILayout.FloatField("SofteningFactor", settings.ForceLeft.SofteningFactor);
		EditorGUILayout.EnumPopup("SofteningFactorVis", settings.ForceLeft.SofteningFactorVis);
		EditorGUILayout.FloatField("Wavelength", settings.ForceLeft.Wavelength);
		EditorGUILayout.EnumPopup("WavelengthVis", settings.ForceLeft.WavelengthVis);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Right Force Settings", EditorStyles.boldLabel);
		EditorGUILayout.EnumPopup("ForceShape", settings.ForceRight.ForceShape);
		EditorGUILayout.EnumPopup("ForceAttenuation", settings.ForceRight.ForceAttenuation);
		EditorGUILayout.FloatField("ForceStrength", settings.ForceRight.ForceStrength);
		EditorGUILayout.EnumPopup("ForceStrengthVis", settings.ForceRight.ForceStrengthVis);
		EditorGUILayout.FloatField("AttenuationDistance", settings.ForceRight.AttenuationDistance);
		EditorGUILayout.EnumPopup("AttenuationDistanceVis", settings.ForceRight.AttenuationDistanceVis);
		EditorGUILayout.FloatField("SofteningFactor", settings.ForceRight.SofteningFactor);
		EditorGUILayout.EnumPopup("SofteningFactorVis", settings.ForceRight.SofteningFactorVis);
		EditorGUILayout.FloatField("Wavelength", settings.ForceRight.Wavelength);
		EditorGUILayout.EnumPopup("WavelengthVis", settings.ForceRight.WavelengthVis);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Left Emitter Settings", EditorStyles.boldLabel);
		EditorGUILayout.FloatField("EmitterCount", settings.EmitterLeft.EmitterCount);
		EditorGUILayout.FloatField("EmitterRadius", settings.EmitterLeft.EmitterRadius);
		EditorGUILayout.EnumPopup("EmitterRadiusVis", settings.EmitterLeft.EmitterRadiusVis);
		EditorGUILayout.FloatField("EmitterVelocity", settings.EmitterLeft.EmitterVelocity);
		EditorGUILayout.EnumPopup("EmitterVelocityVis", settings.EmitterLeft.EmitterVelocityVis);
		EditorGUILayout.FloatField("VelocitySpread", settings.EmitterLeft.VelocitySpread);
		EditorGUILayout.EnumPopup("VelocitySpreadVis", settings.EmitterLeft.VelocitySpreadVis);
		EditorGUILayout.FloatField("InheritVelocity", settings.EmitterLeft.InheritVelocity);
		EditorGUILayout.EnumPopup("InheritVelocityVis", settings.EmitterLeft.InheritVelocityVis);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Right Emitter Settings", EditorStyles.boldLabel);
		EditorGUILayout.FloatField("EmitterCount", settings.EmitterRight.EmitterCount);
		EditorGUILayout.FloatField("EmitterRadius", settings.EmitterRight.EmitterRadius);
		EditorGUILayout.EnumPopup("EmitterRadiusVis", settings.EmitterRight.EmitterRadiusVis);
		EditorGUILayout.FloatField("EmitterVelocity", settings.EmitterRight.EmitterVelocity);
		EditorGUILayout.EnumPopup("EmitterVelocityVis", settings.EmitterRight.EmitterVelocityVis);
		EditorGUILayout.FloatField("VelocitySpread", settings.EmitterRight.VelocitySpread);
		EditorGUILayout.EnumPopup("VelocitySpreadVis", settings.EmitterRight.VelocitySpreadVis);
		EditorGUILayout.FloatField("InheritVelocity", settings.EmitterRight.InheritVelocity);
		EditorGUILayout.EnumPopup("InheritVelocityVis", settings.EmitterRight.InheritVelocityVis);
	}
}