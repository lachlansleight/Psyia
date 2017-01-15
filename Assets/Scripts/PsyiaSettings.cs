using UnityEngine;
using System.Collections;

[System.Serializable]
public class PsyiaSettings {

	//Visual Settings
	public static int ParticleForm;
	public static int ColorMode;
	public static float LineLength;
	public static float ShipSize;

	//Control Settings
	public static float ControllerForce;
	public static int LeftTouchpadFunction;
	public static int RightTouchpadFunction;
	public static float ControllerDistance;
	public static bool ControllerModels;

	//Audio Settings
	public static float VisualizationStrengthPhysics;
	public static float VisualizationStrengthGraphics;
	public static int SongPlaying;
	public static bool Loop;
	public static float Volume;
	public static bool MusicSlowsWithTime;

	//Physics Settings
	public static float ParticleMass;
	public static float VelocityDampening;
	public static float VortexStrength;
	public static bool RoomCollision;
	public static bool JellyMode;

	//System Settings
	public static int ParticleCount;
	public static int Antialiasing;
	public static bool Bloom;

	//MeditationSettings
	public static int MeditationPosture;

	//App Settings
	public static bool FirstTime;

	public static void SavePreset(int slot) {
		string newName = "Preset_Slot_" + slot + ".psy";

		System.IO.FileStream stream = new System.IO.FileStream(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Psyia/Presets/" + newName, System.IO.FileMode.Create);
		System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		bf.Serialize(stream, CreateInstance());
		stream.Close();

	}

	public static void LoadPreset(string presetName) {
		string pathBase = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Psyia/Presets/";

		System.IO.FileInfo[] fileList = new System.IO.DirectoryInfo(pathBase).GetFiles();
		int foundInt = -1;
		for(int i = 0; i < fileList.Length; i++) {
			if(fileList[i].Name.Equals(presetName)) {
				foundInt = i;
				break;
			}
		}

		if(foundInt == -1) {
			Debug.LogError("Error - didn't find preset with name " + presetName);
		} else {
			System.IO.FileStream stream = new System.IO.FileStream(pathBase + presetName, System.IO.FileMode.Open);
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			PsyiaSettingsInstance newInstance = (PsyiaSettingsInstance)bf.Deserialize(stream);
			stream.Close();
			LoadFromInstance(newInstance);
		}
	}

	static PsyiaSettingsInstance CreateInstance() {
		PsyiaSettingsInstance newInstance = new PsyiaSettingsInstance();
		newInstance.ParticleForm = ParticleForm;
		newInstance.ColorMode = ColorMode;
		newInstance.LineLength = LineLength;
		newInstance.ShipSize = ShipSize;

		newInstance.ControllerForce = ControllerForce;
		newInstance.LeftTouchpadFunction = LeftTouchpadFunction;
		newInstance.RightTouchpadFunction = RightTouchpadFunction;
		newInstance.ControllerDistance = ControllerDistance;
		newInstance.ControllerModels = ControllerModels;

		newInstance.VisualizationStrengthPhysics = VisualizationStrengthPhysics;
		newInstance.VisualizationStrengthGraphics = VisualizationStrengthGraphics;
		newInstance.SongPlaying = SongPlaying;
		newInstance.Loop = Loop;
		newInstance.Volume = Volume;
		newInstance.MusicSlowsWithTime = MusicSlowsWithTime;

		newInstance.ParticleMass = ParticleMass;
		newInstance.VelocityDampening = VelocityDampening;
		newInstance.VortexStrength = VortexStrength;
		newInstance.RoomCollision = RoomCollision;
		newInstance.JellyMode = JellyMode;

		newInstance.ParticleCount = ParticleCount;
		newInstance.Antialiasing = Antialiasing;
		newInstance.Bloom = Bloom;

		newInstance.MeditationPosture = MeditationPosture;

		newInstance.FirstTime = FirstTime;

		return newInstance;
	}

	static void LoadFromInstance(PsyiaSettingsInstance newInstance) {
		ParticleForm = newInstance.ParticleForm;
		ColorMode = newInstance.ColorMode;
		LineLength = newInstance.LineLength;
		ShipSize = newInstance.ShipSize;

		ControllerForce = newInstance.ControllerForce;
		LeftTouchpadFunction = newInstance.LeftTouchpadFunction;
		RightTouchpadFunction = newInstance.RightTouchpadFunction;
		ControllerDistance = newInstance.ControllerDistance;
		ControllerModels = newInstance.ControllerModels;

		VisualizationStrengthPhysics = newInstance.VisualizationStrengthPhysics;
		VisualizationStrengthGraphics = newInstance.VisualizationStrengthGraphics;
		SongPlaying = newInstance.SongPlaying;
		Loop = newInstance.Loop;
		Volume = newInstance.Volume;
		MusicSlowsWithTime = newInstance.MusicSlowsWithTime;

		ParticleMass = newInstance.ParticleMass;
		VelocityDampening = newInstance.VelocityDampening;
		VortexStrength = newInstance.VortexStrength;
		RoomCollision = newInstance.RoomCollision;
		JellyMode = newInstance.JellyMode;

		ParticleCount = newInstance.ParticleCount;
		Antialiasing = newInstance.Antialiasing;
		Bloom = newInstance.Bloom;

		MeditationPosture = newInstance.MeditationPosture;

		FirstTime = newInstance.FirstTime;
	}
}

[System.Serializable]
public class PsyiaSettingsInstance {
	//Visual Settings
	public int ParticleForm;
	public int ColorMode;
	public float LineLength;
	public float ShipSize;

	//Control Settings
	public float ControllerForce;
	public int LeftTouchpadFunction;
	public int RightTouchpadFunction;
	public float ControllerDistance;
	public bool ControllerModels;

	//Audio Settings
	public float VisualizationStrengthPhysics;
	public float VisualizationStrengthGraphics;
	public int SongPlaying;
	public bool Loop;
	public float Volume;
	public bool MusicSlowsWithTime;

	//Physics Settings
	public float ParticleMass;
	public float VelocityDampening;
	public float VortexStrength;
	public bool RoomCollision;
	public bool JellyMode;

	//System Settings
	public int ParticleCount;
	public int Antialiasing;
	public bool Bloom;

	//MeditationSettings
	public int MeditationPosture;

	//App Settings
	public bool FirstTime;
}
