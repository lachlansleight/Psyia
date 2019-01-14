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
			ParticleSize = jsonObject["Visual"]["ParticleSize"].AsFloat,
			LineLength = jsonObject["Visual"]["LineLength"].AsFloat,
			ParticleShape = (PsyiaVisualSettings.Shape) jsonObject["Visual"]["ParticleShape"].AsInt
		};

		Physics = new PsyiaPhysicsSettings
		{
			ParticleMass = jsonObject["Physics"]["ParticleMass"].AsFloat,
			ParticleDamping = jsonObject["Physics"]["ParticleDamping"].AsFloat,
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
			ControllerHaptics = jsonObject["Controller"]["ControllerHaptics"].AsBool
		};

		ForceLeft = new PsyiaForceSettings
		{
			ForceShape = (PsyiaForceSettings.Shape) jsonObject["ForceLeft"]["ForceShape"].AsInt,
			ForceAttenuation = (PsyiaForceSettings.Attenuation) jsonObject["ForceLeft"]["ForceAttenuation"].AsInt,
			ForceStrength = jsonObject["ForceLeft"]["ForceStrength"].AsFloat,
			AttenuationDistance = jsonObject["ForceLeft"]["AttenuationDistance"].AsFloat,
			SofteningFactor = jsonObject["ForceLeft"]["SofteningFactor"].AsFloat,
			Wavelength = jsonObject["ForceLeft"]["Wavelength"].AsFloat
		};
		
		ForceRight = new PsyiaForceSettings
		{
			ForceShape = (PsyiaForceSettings.Shape) jsonObject["ForceRight"]["ForceShape"].AsInt,
			ForceAttenuation = (PsyiaForceSettings.Attenuation) jsonObject["ForceRight"]["ForceAttenuation"].AsInt,
			ForceStrength = jsonObject["ForceRight"]["ForceStrength"].AsFloat,
			AttenuationDistance = jsonObject["ForceRight"]["AttenuationDistance"].AsFloat,
			SofteningFactor = jsonObject["ForceRight"]["SofteningFactor"].AsFloat,
			Wavelength = jsonObject["ForceRight"]["Wavelength"].AsFloat
		};

		EmitterLeft = new PsyiaEmitterSettings
		{
			EmitterCount = jsonObject["EmitterLeft"]["EmitterCount"].AsInt,
			EmitterRadius = jsonObject["EmitterLeft"]["EmitterRadius"].AsFloat,
			EmitterVelocity = jsonObject["EmitterLeft"]["EmitterVelocity"].AsFloat,
			VelocitySpread = jsonObject["EmitterLeft"]["VelocitySpread"].AsFloat,
			InheritVelocity = jsonObject["EmitterLeft"]["InheritVelocity"].AsFloat
		};
		
		EmitterRight = new PsyiaEmitterSettings
		{
			EmitterCount = jsonObject["EmitterRight"]["EmitterCount"].AsInt,
			EmitterRadius = jsonObject["EmitterRight"]["EmitterRadius"].AsFloat,
			EmitterVelocity = jsonObject["EmitterRight"]["EmitterVelocity"].AsFloat,
			VelocitySpread = jsonObject["EmitterRight"]["VelocitySpread"].AsFloat,
			InheritVelocity = jsonObject["EmitterRight"]["InheritVelocity"].AsFloat
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
	public float ParticleSize;
	public float LineLength;
	public Shape ParticleShape;
	
	public PsyiaVisualSettings() { }
	public PsyiaVisualSettings(PsyiaVisualSettings copyTarget) 
	{
		ParticleForm = copyTarget.ParticleForm;
		ParticleColor = copyTarget.ParticleColor;
		ParticleColorAmount = copyTarget.ParticleColorAmount;
		ParticleSize = copyTarget.ParticleSize;
		LineLength = copyTarget.LineLength;
		ParticleShape = copyTarget.ParticleShape;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		outputObject.Add("ParticleForm", (int)ParticleForm);
		outputObject.Add("ParticleColor", (int)ParticleColor);
		outputObject.Add("ParticleColorAmount", ParticleColorAmount);
		outputObject.Add("ParticleSize", ParticleSize);
		outputObject.Add("LineLength", LineLength);
		outputObject.Add("ParticleShape", (int)ParticleShape);
		return outputObject;
	}
}

[System.Serializable]
public class PsyiaPhysicsSettings
{
	public float ParticleMass;
	public float ParticleDamping;
	public float TimeSpeed;
	public bool FloorCollision;
	
	public PsyiaPhysicsSettings() { }
	public PsyiaPhysicsSettings(PsyiaPhysicsSettings copyTarget) 
	{
		ParticleMass = copyTarget.ParticleMass;
		ParticleDamping = copyTarget.ParticleDamping;
		TimeSpeed = copyTarget.TimeSpeed;
		FloorCollision = copyTarget.FloorCollision;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		outputObject.Add("ParticleMass", ParticleMass);
		outputObject.Add("ParticleDamping", ParticleDamping);
		outputObject.Add("TimeSpeed", TimeSpeed);
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
	public bool ControllerHaptics;
	
	public PsyiaControllerSettings() { }
	public PsyiaControllerSettings(PsyiaControllerSettings copyTarget) 
	{
		ControllerSymmetry = copyTarget.ControllerSymmetry;
		ControllerModels = copyTarget.ControllerModels;
		ControllerDistance = copyTarget.ControllerDistance;
		ControllerHaptics = copyTarget.ControllerHaptics;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		outputObject.Add("ControllerSymmetry", (int)ControllerSymmetry);
		outputObject.Add("ControllerModels", ControllerModels);
		outputObject.Add("ControllerDistance", ControllerDistance);
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
		Dipole,
		Linear
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
	public float AttenuationDistance;
	public float SofteningFactor;
	public float Wavelength;
	
	public PsyiaForceSettings() { }
	public PsyiaForceSettings(PsyiaForceSettings copyTarget) 
	{
		ForceShape = copyTarget.ForceShape;
		ForceAttenuation = copyTarget.ForceAttenuation;
		ForceStrength = copyTarget.ForceStrength;
		AttenuationDistance = copyTarget.AttenuationDistance;
		SofteningFactor = copyTarget.SofteningFactor;
		Wavelength = copyTarget.Wavelength;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		outputObject.Add("ForceShape", (int)ForceShape);
		outputObject.Add("ForceAttenuation", (int)ForceAttenuation);
		outputObject.Add("ForceStrength", ForceStrength);
		outputObject.Add("AttenuationDistance", AttenuationDistance);
		outputObject.Add("SofteningFactor", SofteningFactor);
		outputObject.Add("Wavelength", Wavelength);
		return outputObject;
	}
}

[System.Serializable]
public class PsyiaEmitterSettings
{
	public int EmitterCount;
	public float EmitterRadius;
	public float EmitterVelocity;
	public float VelocitySpread;
	public float InheritVelocity;
	
	public PsyiaEmitterSettings() { }
	public PsyiaEmitterSettings(PsyiaEmitterSettings copyTarget) 
	{
		EmitterCount = copyTarget.EmitterCount;
		EmitterRadius = copyTarget.EmitterRadius;
		EmitterVelocity = copyTarget.EmitterVelocity;
		VelocitySpread = copyTarget.VelocitySpread;
		InheritVelocity = copyTarget.InheritVelocity;
	}
	
	public JSONObject GetJsonObject()
	{
		var outputObject = new JSONObject();
		outputObject.Add("EmitterCount", EmitterCount);
		outputObject.Add("EmitterRadius", EmitterRadius);
		outputObject.Add("EmitterVelocity", EmitterVelocity);
		outputObject.Add("VelocitySpread", VelocitySpread);
		outputObject.Add("InheritVelocity", InheritVelocity);
		return outputObject;
	}
}
