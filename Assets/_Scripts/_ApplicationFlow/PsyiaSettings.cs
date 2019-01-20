using CSCore.Streams.SampleConverter;
using CSCore.XAudio2.X3DAudio;
using SimpleJSON;
using UnityEngine;
using Valve.VR;

[CreateAssetMenu(fileName = "PsyiaSetting", menuName = "ScriptableObjects/PsyiaSetting")]
public class PsyiaSettings : ScriptableObject
{
	public PsyiasSystemSettings System;
	public PsyiaVisualSettings Visual;
	public PsyiaPhysicsSettings Physics;
	public PsyiaAudioSettings Audio;
	public PsyiaControllerSettings Controller;
	public PsyiaForceSettings ForceLeft;
	public PsyiaForceSettings ForceRight;
	public PsyiaEmitterSettings EmitterLeft;
	public PsyiaEmitterSettings EmitterRight;

	public void CopyValuesFrom(PsyiaSettings copyTarget)
	{
		System = new PsyiasSystemSettings(copyTarget.System);
		Visual = new PsyiaVisualSettings(copyTarget.Visual);
		Physics = new PsyiaPhysicsSettings(copyTarget.Physics);
		Audio = new PsyiaAudioSettings(copyTarget.Audio);
		Controller = new PsyiaControllerSettings(copyTarget.Controller);
		ForceLeft = new PsyiaForceSettings(copyTarget.ForceLeft);
		ForceRight = new PsyiaForceSettings(copyTarget.ForceRight);
		EmitterLeft = new PsyiaEmitterSettings(copyTarget.EmitterLeft);
		EmitterRight = new PsyiaEmitterSettings(copyTarget.EmitterRight);
	}

	public void CopyValuesFrom(string jsonString)
	{
		var jsonObject = (JSONObject) JSONNode.Parse(jsonString);

		System = new PsyiasSystemSettings
		{
			MaxParticleCount = jsonObject["System"]["MaxParticleCount"].AsInt,
			Antialiasing = (PsyiasSystemSettings.AntialiasingLevel)jsonObject["System"]["Antialiasing"].AsInt,
			Bloom = jsonObject["System"]["Bloom"].AsBool
		};

		Visual = new PsyiaVisualSettings
		{
			ParticleForm = (PsyiaVisualSettings.Form) jsonObject["Visual"]["ParticleForm"].AsInt,
			ParticleColor = (PsyiaVisualSettings.Color) jsonObject["Visual"]["ParticleColor"].AsInt,
			ParticleColorAmount = jsonObject["Visual"]["ParticleColorAmount"].AsFloat,
			ParticleColorAmountVis = (SliderAudioHook.AudioDataSource)jsonObject["Visual"]["ParticleColorAmount"].AsInt,
			ParticleSize = jsonObject["Visual"]["ParticleSize"].AsFloat,
			ParticleSizeVis = (SliderAudioHook.AudioDataSource)jsonObject["Visual"]["ParticleSize"].AsInt,
			LineLength = jsonObject["Visual"]["LineLength"].AsFloat,
			LineLengthVis = (SliderAudioHook.AudioDataSource)jsonObject["Visual"]["LineLength"].AsInt,
			ParticleShape = (PsyiaVisualSettings.Shape) jsonObject["Visual"]["ParticleShape"].AsInt
		};

		Physics = new PsyiaPhysicsSettings
		{
			ParticleMass = jsonObject["Physics"]["ParticleMass"].AsFloat,
			ParticleMassVis = (SliderAudioHook.AudioDataSource)jsonObject["Physics"]["ParticleMass"].AsInt,
			ParticleDamping = jsonObject["Physics"]["ParticleDamping"].AsFloat,
			ParticleDampingVis = (SliderAudioHook.AudioDataSource)jsonObject["Physics"]["ParticleDamping"].AsInt,
			TimeSpeed = jsonObject["Physics"]["TimeSpeed"].AsFloat,
			TimeSpeedVis = (SliderAudioHook.AudioDataSource)jsonObject["Physics"]["TimeSpeed"].AsInt,
			FloorCollision = jsonObject["Physics"]["FloorCollision"].AsBool
		};

		Audio = new PsyiaAudioSettings
		{
			VisualsAudioreactivity = jsonObject["Audio"]["VisualsAudioreactivity"].AsFloat,
			PhysicsAudioreactivity = jsonObject["Audio"]["PhysicsAudioreactivity"].AsFloat,
			Volume = jsonObject["Audio"]["Volume"].AsFloat,
			Loop = jsonObject["Audio"]["Loop"].AsBool,
			SlowWithTime = jsonObject["Audio"]["SlowWithTime"].AsBool
		};

		Controller = new PsyiaControllerSettings
		{
			ControllerSymmetry = (PsyiaControllerSettings.Symmetry) jsonObject["Controller"]["ControllerSymmetry"].AsInt,
			ControllerModels = jsonObject["Controller"]["ControllerModels"].AsBool,
			ControllerDistance = jsonObject["Controller"]["ControllerDistance"].AsFloat,
			ControllerDistanceVis = (SliderAudioHook.AudioDataSource)jsonObject["Controller"]["ControllerDistance"].AsInt,
			ControllerHaptics = jsonObject["Controller"]["ControllerHaptics"].AsBool
		};

		ForceLeft = new PsyiaForceSettings
		{
			ForceShape = (PsyiaForceSettings.Shape) jsonObject["ForceLeft"]["ForceShape"].AsInt,
			ForceAttenuation = (PsyiaForceSettings.Attenuation) jsonObject["ForceLeft"]["ForceAttenuation"].AsInt,
			ForceStrength = jsonObject["ForceLeft"]["ForceStrength"].AsFloat,
			ForceStrengthVis = (SliderAudioHook.AudioDataSource)jsonObject["ForceLeft"]["ForceStrength"].AsInt,
			AttenuationDistance = jsonObject["ForceLeft"]["AttenuationDistance"].AsFloat,
			AttenuationDistanceVis = (SliderAudioHook.AudioDataSource)jsonObject["ForceLeft"]["AttenuationDistance"].AsInt,
			SofteningFactor = jsonObject["ForceLeft"]["SofteningFactor"].AsFloat,
			SofteningFactorVis = (SliderAudioHook.AudioDataSource)jsonObject["ForceLeft"]["SofteningFactor"].AsInt,
			Wavelength = jsonObject["ForceLeft"]["Wavelength"].AsFloat,
			WavelengthVis = (SliderAudioHook.AudioDataSource)jsonObject["ForceLeft"]["Wavelength"].AsInt
		};
		
		ForceRight = new PsyiaForceSettings
		{
			ForceShape = (PsyiaForceSettings.Shape) jsonObject["ForceRight"]["ForceShape"].AsInt,
			ForceAttenuation = (PsyiaForceSettings.Attenuation) jsonObject["ForceRight"]["ForceAttenuation"].AsInt,
			ForceStrength = jsonObject["ForceRight"]["ForceStrength"].AsFloat,
			ForceStrengthVis = (SliderAudioHook.AudioDataSource)jsonObject["ForceRight"]["ForceStrength"].AsInt,
			AttenuationDistance = jsonObject["ForceRight"]["AttenuationDistance"].AsFloat,
			AttenuationDistanceVis = (SliderAudioHook.AudioDataSource)jsonObject["ForceRight"]["AttenuationDistance"].AsInt,
			SofteningFactor = jsonObject["ForceRight"]["SofteningFactor"].AsFloat,
			SofteningFactorVis = (SliderAudioHook.AudioDataSource)jsonObject["ForceRight"]["SofteningFactor"].AsInt,
			Wavelength = jsonObject["ForceRight"]["Wavelength"].AsFloat,
			WavelengthVis = (SliderAudioHook.AudioDataSource)jsonObject["ForceRight"]["Wavelength"].AsInt
		};

		EmitterLeft= new PsyiaEmitterSettings
		{
			EmitterCount = jsonObject["EmitterLeft"]["EmitterCount"].AsInt,
			EmitterRadius = jsonObject["EmitterLeft"]["EmitterRadius"].AsFloat,
			EmitterRadiusVis = (SliderAudioHook.AudioDataSource)jsonObject["EmitterLeft"]["EmitterRadius"].AsInt,
			EmitterVelocity = jsonObject["EmitterLeft"]["EmitterVelocity"].AsFloat,
			EmitterVelocityVis = (SliderAudioHook.AudioDataSource)jsonObject["EmitterLeft"]["EmitterVelocity"].AsInt,
			VelocitySpread = jsonObject["EmitterLeft"]["VelocitySpread"].AsFloat,
			VelocitySpreadVis = (SliderAudioHook.AudioDataSource)jsonObject["EmitterLeft"]["VelocitySpread"].AsInt,
			InheritVelocity = jsonObject["EmitterLeft"]["InheritVelocity"].AsFloat,
			InheritVelocityVis = (SliderAudioHook.AudioDataSource)jsonObject["EmitterLeft"]["InheritVelocity"].AsInt
		};
		
		EmitterRight = new PsyiaEmitterSettings
		{
			EmitterCount = jsonObject["EmitterRight"]["EmitterCount"].AsInt,
			EmitterRadius = jsonObject["EmitterRight"]["EmitterRadius"].AsFloat,
			EmitterRadiusVis = (SliderAudioHook.AudioDataSource)jsonObject["EmitterRight"]["EmitterRadius"].AsInt,
			EmitterVelocity = jsonObject["EmitterRight"]["EmitterVelocity"].AsFloat,
			EmitterVelocityVis = (SliderAudioHook.AudioDataSource)jsonObject["EmitterRight"]["EmitterVelocity"].AsInt,
			VelocitySpread = jsonObject["EmitterRight"]["VelocitySpread"].AsFloat,
			VelocitySpreadVis = (SliderAudioHook.AudioDataSource)jsonObject["EmitterRight"]["VelocitySpread"].AsInt,
			InheritVelocity = jsonObject["EmitterRight"]["InheritVelocity"].AsFloat,
			InheritVelocityVis = (SliderAudioHook.AudioDataSource)jsonObject["EmitterRight"]["InheritVelocity"].AsInt
		};
	}
	public string GetSettingsJson()
	{
		var settingsObject = new JSONObject();
		settingsObject.Add("System", System.GetJsonObject());
		settingsObject.Add("Visual", Visual.GetJsonObject());
		settingsObject.Add("Physics", Physics.GetJsonObject());
		settingsObject.Add("Audio", Audio.GetJsonObject());
		settingsObject.Add("Controller", Controller.GetJsonObject());
		settingsObject.Add("ForceLeft", ForceLeft.GetJsonObject());
		settingsObject.Add("ForceRight", ForceRight.GetJsonObject());
		settingsObject.Add("EmitterLeft", EmitterLeft.GetJsonObject());
		settingsObject.Add("EmitterRight", EmitterRight.GetJsonObject());
		return settingsObject.ToString(4);
	}
}

[System.Serializable]
public class PsyiasSystemSettings
{
	public enum AntialiasingLevel
	{
		None,
		TwoTimes,
		FourTimes,
		EightTimes
	}
	
	public int MaxParticleCount;
	public AntialiasingLevel Antialiasing;
	public bool Bloom;
	
	public PsyiasSystemSettings() { }

	public PsyiasSystemSettings(PsyiasSystemSettings copyTarget)
	{
			MaxParticleCount = copyTarget.MaxParticleCount;
			Antialiasing = copyTarget.Antialiasing;
			Bloom = copyTarget.Bloom;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		
		outputObject.Add("MaxParticleCount", MaxParticleCount);
		outputObject.Add("Antialiasing", (int)Antialiasing);
		outputObject.Add("Bloom", Bloom);
		
		return outputObject;
	}
}

[System.Serializable]
public class PsyiaVisualSettings
{
	public enum Form
	{
		Points,
		Lines,
		Shapes,
		Diamonds
	}

	public enum Color
	{
		MagicOne,
		MagicTwo,
		Air,
		Fire,
		Earth,
		Water
	}

	public enum Shape
	{
		Spark,
		Smoke,
		Ring,
		Blur,
		Circle,
		Square
	}

	

	public Form ParticleForm;
	public Color ParticleColor;
	public float ParticleColorAmount;
	public SliderAudioHook.AudioDataSource ParticleColorAmountVis;
	public float ParticleSize;
	public SliderAudioHook.AudioDataSource ParticleSizeVis;
	public float LineLength;
	public SliderAudioHook.AudioDataSource LineLengthVis;
	public Shape ParticleShape;
	
	public PsyiaVisualSettings() { }
	public PsyiaVisualSettings(PsyiaVisualSettings copyTarget) 
	{
		ParticleForm = copyTarget.ParticleForm;
		ParticleColor = copyTarget.ParticleColor;
		ParticleColorAmount = copyTarget.ParticleColorAmount;
		ParticleColorAmountVis = copyTarget.ParticleColorAmountVis;
		ParticleSize = copyTarget.ParticleSize;
		ParticleSizeVis = copyTarget.ParticleSizeVis;
		LineLength = copyTarget.LineLength;
		LineLengthVis = copyTarget.LineLengthVis;
		ParticleShape = copyTarget.ParticleShape;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		outputObject.Add("ParticleForm", (int)ParticleForm);
		outputObject.Add("ParticleColor", (int)ParticleColor);
		outputObject.Add("ParticleColorAmount", ParticleColorAmount);
		outputObject.Add("ParticleColorAmountVis", (int)ParticleColorAmountVis);
		outputObject.Add("ParticleSize", ParticleSize);
		outputObject.Add("ParticleSizeVis", (int)ParticleSizeVis);
		outputObject.Add("LineLength", LineLength);
		outputObject.Add("LineLengthVis", (int)LineLengthVis);
		outputObject.Add("ParticleShape", (int)ParticleShape);
		return outputObject;
	}
}

[System.Serializable]
public class PsyiaPhysicsSettings
{
	public float ParticleMass;
	public SliderAudioHook.AudioDataSource ParticleMassVis;
	public float ParticleDamping;
	public SliderAudioHook.AudioDataSource ParticleDampingVis;
	public float TimeSpeed;
	public SliderAudioHook.AudioDataSource TimeSpeedVis;
	public bool FloorCollision;
	
	public PsyiaPhysicsSettings() { }
	public PsyiaPhysicsSettings(PsyiaPhysicsSettings copyTarget) 
	{
		ParticleMass = copyTarget.ParticleMass;
		ParticleMassVis = copyTarget.ParticleMassVis;
		ParticleDamping = copyTarget.ParticleDamping;
		ParticleDampingVis = copyTarget.ParticleDampingVis;
		TimeSpeed = copyTarget.TimeSpeed;
		TimeSpeedVis = copyTarget.TimeSpeedVis;
		FloorCollision = copyTarget.FloorCollision;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		outputObject.Add("ParticleMass", ParticleMass);
		outputObject.Add("ParticleMassVis", (int)ParticleMassVis);
		outputObject.Add("ParticleDamping", ParticleDamping);
		outputObject.Add("ParticleDampingVis", (int)ParticleDampingVis);
		outputObject.Add("TimeSpeed", TimeSpeed);
		outputObject.Add("TimeSpeedVis", (int)TimeSpeedVis);
		outputObject.Add("FloorCollision", FloorCollision);
		return outputObject;
	}
}

[System.Serializable]
public class PsyiaAudioSettings
{
	public float VisualsAudioreactivity;
	public float PhysicsAudioreactivity;
	public float Volume;
	public bool Loop;
	public bool SlowWithTime;
	
	public PsyiaAudioSettings() { }
	public PsyiaAudioSettings(PsyiaAudioSettings copyTarget) 
	{
		VisualsAudioreactivity = copyTarget.VisualsAudioreactivity;
		PhysicsAudioreactivity = copyTarget.PhysicsAudioreactivity;
		Volume = copyTarget.Volume;
		Loop = copyTarget.Loop;
		SlowWithTime = copyTarget.SlowWithTime;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		outputObject.Add("VisualsAudioreactivity", VisualsAudioreactivity);
		outputObject.Add("PhysicsAudioreactivity", PhysicsAudioreactivity);
		outputObject.Add("Volume", Volume);
		outputObject.Add("Loop", Loop);
		outputObject.Add("SlowWithTime", SlowWithTime);
		return outputObject;
	}
}

[System.Serializable]
public class PsyiaControllerSettings
{
	public enum Symmetry
	{
		CopyFromLeft,
		Separate,
		CopyFromRight
	}

	public Symmetry ControllerSymmetry;
	public bool ControllerModels;
	public float ControllerDistance;
	public SliderAudioHook.AudioDataSource ControllerDistanceVis;
	public bool ControllerHaptics;
	
	public PsyiaControllerSettings() { }
	public PsyiaControllerSettings(PsyiaControllerSettings copyTarget) 
	{
		ControllerSymmetry = copyTarget.ControllerSymmetry;
		ControllerModels = copyTarget.ControllerModels;
		ControllerDistance = copyTarget.ControllerDistance;
		ControllerDistanceVis = copyTarget.ControllerDistanceVis;
		ControllerHaptics = copyTarget.ControllerHaptics;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		outputObject.Add("ControllerSymmetry", (int)ControllerSymmetry);
		outputObject.Add("ControllerModels", ControllerModels);
		outputObject.Add("ControllerDistance", ControllerDistance);
		outputObject.Add("ControllerDistanceVis", (int)ControllerDistanceVis);
		Debug.Log("Controller distance vis is " + ControllerDistanceVis);
		outputObject.Add("ControllerHaptics", ControllerHaptics);
		return outputObject;
	}
}

[System.Serializable]
public class PsyiaForceSettings
{
	public enum Shape
	{
		Radial,
		Vortex,
		Linear,
		Dipole
	}

	public enum Attenuation
	{
		Infinite,
		Linear,
		Exp,
		Exp2,
		ExpSoft,
		Exp2Soft,
		Wave
	}

	public Shape ForceShape;
	public Attenuation ForceAttenuation;
	public float ForceStrength;
	public SliderAudioHook.AudioDataSource ForceStrengthVis;
	public float AttenuationDistance;
	public SliderAudioHook.AudioDataSource AttenuationDistanceVis;
	public float SofteningFactor;
	public SliderAudioHook.AudioDataSource SofteningFactorVis;
	public float Wavelength;
	public SliderAudioHook.AudioDataSource WavelengthVis;
	
	public PsyiaForceSettings() { }
	public PsyiaForceSettings(PsyiaForceSettings copyTarget) 
	{
		ForceShape = copyTarget.ForceShape;
		ForceAttenuation = copyTarget.ForceAttenuation;
		ForceStrength = copyTarget.ForceStrength;
		ForceStrengthVis = copyTarget.ForceStrengthVis;
		AttenuationDistance = copyTarget.AttenuationDistance;
		AttenuationDistanceVis = copyTarget.AttenuationDistanceVis;
		SofteningFactor = copyTarget.SofteningFactor;
		SofteningFactorVis = copyTarget.SofteningFactorVis;
		Wavelength = copyTarget.Wavelength;
		WavelengthVis = copyTarget.WavelengthVis;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		outputObject.Add("ForceShape", (int)ForceShape);
		outputObject.Add("ForceAttenuation", (int)ForceAttenuation);
		outputObject.Add("ForceStrength", ForceStrength);
		outputObject.Add("ForceStrengthVis", (int)ForceStrengthVis);
		outputObject.Add("AttenuationDistance", AttenuationDistance);
		outputObject.Add("AttenuationDistanceVis", (int)AttenuationDistanceVis);
		outputObject.Add("SofteningFactor", SofteningFactor);
		outputObject.Add("SofteningFactorVis", (int)SofteningFactorVis);
		outputObject.Add("Wavelength", Wavelength);
		outputObject.Add("WavelengthVis", (int)WavelengthVis);
		return outputObject;
	}
}

[System.Serializable]
public class PsyiaEmitterSettings
{
	public int EmitterCount;
	public float EmitterRadius;
	public SliderAudioHook.AudioDataSource EmitterRadiusVis;
	public float EmitterVelocity;
	public SliderAudioHook.AudioDataSource EmitterVelocityVis;
	public float VelocitySpread;
	public SliderAudioHook.AudioDataSource VelocitySpreadVis;
	public float InheritVelocity;
	public SliderAudioHook.AudioDataSource InheritVelocityVis;
	
	public PsyiaEmitterSettings() { }
	public PsyiaEmitterSettings(PsyiaEmitterSettings copyTarget) 
	{
		EmitterCount = copyTarget.EmitterCount;
		EmitterRadius = copyTarget.EmitterRadius;
		EmitterRadiusVis = copyTarget.EmitterRadiusVis;
		EmitterVelocity = copyTarget.EmitterVelocity;
		EmitterVelocityVis = copyTarget.EmitterVelocityVis;
		VelocitySpread = copyTarget.VelocitySpread;
		VelocitySpreadVis = copyTarget.VelocitySpreadVis;
		InheritVelocity = copyTarget.InheritVelocity;
		InheritVelocityVis = copyTarget.InheritVelocityVis;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		outputObject.Add("EmitterCount", EmitterCount);
		outputObject.Add("EmitterRadius", EmitterRadius);
		outputObject.Add("EmitterRadiusVis", (int)EmitterRadiusVis);
		outputObject.Add("EmitterVelocity", EmitterVelocity);
		outputObject.Add("EmitterVelocityVis", (int)EmitterVelocityVis);
		outputObject.Add("VelocitySpread", VelocitySpread);
		outputObject.Add("VelocitySpreadVis", (int)VelocitySpreadVis);
		outputObject.Add("InheritVelocity", InheritVelocity);
		outputObject.Add("InheritVelocityVis", (int)InheritVelocityVis);
		return outputObject;
	}
}
