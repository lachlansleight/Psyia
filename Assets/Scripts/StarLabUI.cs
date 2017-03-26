using UnityEngine;
using System.Collections;
using VRTools.UI;

public class StarLabUI : MonoBehaviour {

	public VRUI_Panel uiPanel;
	public StarLab starLab;
	public StarMusic music;
	[Header("Panel Changing Stuff")]
	public GameObject[] panelCanvases;
	public GameObject[] panelControls;
	public Renderer[] panelButtonRenderers;
	Material[] panelFrontMaterials;
	private Material[] panelButtonMaterials;
	public Color inactiveButtonColor = Color.red;
	public Color activeButtonColor = Color.black;
	[Header("Miscellaneous")]
	public GameObject lineLengthTitleText;
	public GameObject lineLengthLabelText;
	public GameObject shipSizeTitleText;
	public GameObject shipSizeLabelText;
	public GameObject room;
	public GameObject refreshSystemText;
	public UnityStandardAssets.ImageEffects.Bloom bloomEffect;
	public AudioSource musicSource;
	public string[] trackNames;
	public UnityEngine.UI.Text nowPlayingText;
	public GameObject[] loadButtons;

	private int newCount = -1;

	// Use this for initialization
	void Start () {
		panelButtonMaterials = new Material[panelButtonRenderers.Length];
		panelFrontMaterials = new Material[panelButtonRenderers.Length];
		for(int i = 0; i < panelButtonRenderers.Length; i++) {
			panelButtonMaterials[i] = panelButtonRenderers[i].material;
			panelFrontMaterials[i] = panelButtonRenderers[i].transform.GetChild(0).GetComponent<Renderer>().material;
		}
	

		SetPanel(0);

		uiPanel.GetControl("VisualSettingsButton").OnPressUp += ShowGraphicsPanel;
		uiPanel.GetControl("ControlSettingsButton").OnPressUp += ShowControlsPanel;
		uiPanel.GetControl("AudioSettingsButton").OnPressUp += ShowAudioPanel;
		uiPanel.GetControl("PhysicsSettingsButton").OnPressUp += ShowPhysicsPanel;
		uiPanel.GetControl("SystemSettingsButton").OnPressUp += ShowSystemPanel;
		uiPanel.GetControl("PresetSettingsButton").OnPressUp += ShowPresetPanel;

		uiPanel.GetControl("ParticleFormRotary").OnIntChange += ParticleFormChanged;
		uiPanel.GetControl("ColorModeRotary").OnIntChange += ParticleColorModeChanged;
		uiPanel.GetControl("LineLengthSlider").OnFloatChange += LineLengthChanged;
		uiPanel.GetControl("ShipSizeSlider").OnFloatChange += ShipSizeChanged;

		uiPanel.GetControl("ControllerForceSlider").OnFloatChange += ControllerForceChanged;
		uiPanel.GetControl("LeftTouchpadToggle").OnBooleanChange += LeftTouchpadChanged;
		uiPanel.GetControl("RightTouchpadToggle").OnBooleanChange += RightTouchpadChanged;
		uiPanel.GetControl("ControllerDistanceSlider").OnFloatChange += ControllerDistanceChanged;
		uiPanel.GetControl("ControllerModelsToggle").OnBooleanChange += ControllerModelsChanged;

		uiPanel.GetControl("ParticleMassSlider").OnFloatChange += ParticleMassChanged;
		uiPanel.GetControl("VelocityDampeningSlider").OnFloatChange += VelocityDampeningChanged;
		uiPanel.GetControl("VortexStrengthSlider").OnFloatChange += VortexStrengthChanged;
		uiPanel.GetControl("RoomCollisionToggle").OnBooleanChange += RoomCollisionChanged;
		uiPanel.GetControl("JellyModeToggle").OnBooleanChange += JellyModeChanged;

		uiPanel.GetControl("ParticleCountSlider").OnFloatChange += ParticleCountChanged;
		uiPanel.GetControl("AntialiasingRotary").OnIntChange += AntialiasingChanged;
		uiPanel.GetControl("BloomToggle").OnBooleanChange += BloomChanged;
		uiPanel.GetControl("RefreshButton").OnPressUp += ResetSystem;
		uiPanel.GetControl("ReturnToMenuButton").OnPressUp += ReturnToMenu;

		uiPanel.GetControl("VisualizationStrengthPhysicsSlider").OnFloatChange += VisualizationStrengthPhysicsChanged;
		uiPanel.GetControl("VisualizationStrengthGraphicsSlider").OnFloatChange += VisualizationStrengthGraphicsChanged;
		uiPanel.GetControl("PlayPauseButton").OnPressUp += PlayPauseSelected;
		uiPanel.GetControl("NextTrackButton").OnPressUp += NextTrackSelected;
		uiPanel.GetControl("PreviousTrackButton").OnPressUp += PreviousTrackSelected;
		uiPanel.GetControl("LoopToggle").OnBooleanChange += LoopChanged;
		uiPanel.GetControl("VolumeDial").OnFloatChange += VolumeChanged;
		uiPanel.GetControl("MusicSlowsWithTimeToggle").OnBooleanChange += MusicSlowsWithTimeChanged;

		uiPanel.GetControl("Slot1LoadButton").OnPressUp += Slot1Load;
		uiPanel.GetControl("Slot1SaveButton").OnPressUp += Slot1Save;
		uiPanel.GetControl("Slot2LoadButton").OnPressUp += Slot2Load;
		uiPanel.GetControl("Slot2SaveButton").OnPressUp += Slot2Save;
		uiPanel.GetControl("Slot3LoadButton").OnPressUp += Slot3Load;
		uiPanel.GetControl("Slot3SaveButton").OnPressUp += Slot3Save;
		uiPanel.GetControl("Slot4LoadButton").OnPressUp += Slot4Load;
		uiPanel.GetControl("Slot4SaveButton").OnPressUp += Slot4Save;

		for(int i = 0; i < uiPanel.controls.Length; i++) uiPanel.controls[i].SetDefaultValue();
		//gameObject.SetActive(false);
	}

	void SetPanel(int newPanel) {
		for(int i = 0; i < panelCanvases.Length; i++) {
			panelCanvases[i].SetActive(false);
			panelControls[i].SetActive(false);
			panelButtonMaterials[i].color = inactiveButtonColor;
			panelFrontMaterials[i].color = activeButtonColor;
		}

		panelCanvases[newPanel].SetActive(true);
		panelControls[newPanel].SetActive(true);
		panelButtonMaterials[newPanel].color = activeButtonColor;
		panelFrontMaterials[newPanel].color = inactiveButtonColor;
	}
	public void ShowGraphicsPanel() {
		SetPanel(0);
	}
	public void ShowControlsPanel() {
		SetPanel(1);
	}
	public void ShowAudioPanel() {
		SetPanel(2);
	}
	public void ShowPhysicsPanel() {
		SetPanel(3);
	}
	public void ShowSystemPanel() {
		SetPanel(4);
	}
	public void ShowPresetPanel() {
		SetPanel(5);
		CheckPresetExistence();
	}

	public void ParticleFormChanged(int newValue) {
		PsyiaSettings.ParticleForm = newValue;
		if(newValue != 0) {
			uiPanel.GetControl("LineLengthSlider").gameObject.SetActive(true);
			lineLengthTitleText.SetActive(true);
			lineLengthLabelText.SetActive(true);
			if(newValue == 2) {
				uiPanel.GetControl("ShipSizeSlider").gameObject.SetActive(true);
				shipSizeTitleText.SetActive(true);
				shipSizeLabelText.SetActive(true);
			} else {
				uiPanel.GetControl("ShipSizeSlider").gameObject.SetActive(false);
				shipSizeTitleText.SetActive(false);
				shipSizeLabelText.SetActive(false);
			}
		} else {
			uiPanel.GetControl("LineLengthSlider").gameObject.SetActive(false);
			uiPanel.GetControl("ShipSizeSlider").gameObject.SetActive(false);
			lineLengthTitleText.SetActive(false);
			lineLengthLabelText.SetActive(false);
			shipSizeTitleText.SetActive(false);
			shipSizeLabelText.SetActive(false);
		}
	}

	public void ParticleColorModeChanged(int newValue) {
		PsyiaSettings.ColorMode = newValue;
	}

	public void LineLengthChanged(float newValue) {
		PsyiaSettings.LineLength = newValue;
	}

	public void ShipSizeChanged(float newValue) {
		PsyiaSettings.ShipSize = newValue;
	}
	
	public void ControllerForceChanged(float newValue) {
		PsyiaSettings.ControllerForce = newValue;
	}

	public void LeftTouchpadChanged(bool newValue) {
		PsyiaSettings.LeftTouchpadFunction = newValue ? 1 : 0;
	}

	public void RightTouchpadChanged(bool newValue) {
		PsyiaSettings.RightTouchpadFunction = newValue ? 1 : 0;
	}

	public void ControllerDistanceChanged(float newValue) {
		PsyiaSettings.ControllerDistance = newValue;
	}

	public void ControllerModelsChanged(bool newValue) {
		PsyiaSettings.ControllerModels = newValue;
	}

	public void ParticleMassChanged(float newValue) {
		PsyiaSettings.ParticleMass = newValue;
	}

	public void VelocityDampeningChanged(float newValue) {
		PsyiaSettings.VelocityDampening = newValue;
	}

	public void VortexStrengthChanged(float newValue) {
		PsyiaSettings.VortexStrength = newValue;
	}

	public void RoomCollisionChanged(bool newValue) {
		PsyiaSettings.RoomCollision = newValue;
		room.SetActive(newValue);
	}

	public void JellyModeChanged(bool newValue) {
		PsyiaSettings.JellyMode = newValue;
	}

	public void ParticleCountChanged(float newValue) {
		newCount = Mathf.RoundToInt(newValue / 1000f) * 1000;
		refreshSystemText.SetActive(true);
		PsyiaSettings.ParticleCount = newCount;
	}

	public void AntialiasingChanged(int newValue) {
		switch(newValue) {
		case 0:
			QualitySettings.antiAliasing = 0;
			break;
		case 1:
			QualitySettings.antiAliasing = 2;
			break;
		case 2:
			QualitySettings.antiAliasing = 4;
			break;
		case 3:
			QualitySettings.antiAliasing = 8;
			break;
		default:
			QualitySettings.antiAliasing = 2;
			break;
		}
		PsyiaSettings.Antialiasing = QualitySettings.antiAliasing;
	}

	public void BloomChanged(bool newValue) {
		bloomEffect.enabled = newValue;
		PsyiaSettings.Bloom = newValue;
	}

	public void ResetSystem() {
		starLab.Reset();
		refreshSystemText.SetActive(false);

	}

	public void ReturnToMenu() {
		UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
	}

	public void VisualizationStrengthGraphicsChanged(float newValue) {
		PsyiaSettings.VisualizationStrengthGraphics = newValue;
	}
	public void VisualizationStrengthPhysicsChanged(float newValue) {
		PsyiaSettings.VisualizationStrengthPhysics = newValue;
	}

	public void NextTrackSelected() {
		music.NextTrack();
	}

	public void PreviousTrackSelected() {
		music.PreviousTrack();
	}

	public void SetSong(AudioClip newSong) {
		musicSource.Stop();
		musicSource.clip = newSong;
		musicSource.Play();
	}

	public void PlayPauseSelected() {
		music.PlayPause();
	}

	public void LoopChanged(bool newValue) {
		PsyiaSettings.Loop = newValue;
		music.loop = newValue;
	}

	public void VolumeChanged(float newValue) {
		PsyiaSettings.Volume = newValue;
		musicSource.volume = newValue;
	}

	public void MusicSlowsWithTimeChanged(bool newValue) {
		PsyiaSettings.MusicSlowsWithTime = newValue;
	}

	void CheckPresetExistence() {
		string basePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Psyia/Presets/";
		loadButtons[0].SetActive(System.IO.File.Exists(basePath + "Preset_Slot_1.psy"));
		loadButtons[1].SetActive(System.IO.File.Exists(basePath + "Preset_Slot_2.psy"));
		loadButtons[2].SetActive(System.IO.File.Exists(basePath + "Preset_Slot_3.psy"));
		loadButtons[3].SetActive(System.IO.File.Exists(basePath + "Preset_Slot_4.psy"));
	}

	void LoadPreset(int number) {
		int storedParticleCount = PsyiaSettings.ParticleCount;

		string presetName = "Preset_Slot_" + number + ".psy";
		PsyiaSettings.LoadPreset(presetName);

		//set all the controls to the correct values - yeesh!

		uiPanel.GetControl("ParticleFormRotary").SetIntValue(PsyiaSettings.ParticleForm);
		uiPanel.GetControl("ColorModeRotary").SetIntValue(PsyiaSettings.ColorMode);
		uiPanel.GetControl("LineLengthSlider").SetFloatValue(PsyiaSettings.LineLength);
		uiPanel.GetControl("ShipSizeSlider").SetFloatValue(PsyiaSettings.ShipSize);

		uiPanel.GetControl("ControllerForceSlider").SetFloatValue(PsyiaSettings.ControllerForce);
		uiPanel.GetControl("LeftTouchpadToggle").SetBoolValue(PsyiaSettings.LeftTouchpadFunction == 0 ? false : true);
		uiPanel.GetControl("RightTouchpadToggle").SetBoolValue(PsyiaSettings.RightTouchpadFunction == 0 ? false : true);
		uiPanel.GetControl("ControllerDistanceSlider").SetFloatValue(PsyiaSettings.ControllerDistance);
		uiPanel.GetControl("ControllerModelsToggle").SetBoolValue(PsyiaSettings.ControllerModels);

		uiPanel.GetControl("ParticleMassSlider").SetFloatValue(PsyiaSettings.ParticleMass);
		uiPanel.GetControl("VelocityDampeningSlider").SetFloatValue(PsyiaSettings.VelocityDampening);
		uiPanel.GetControl("VortexStrengthSlider").SetFloatValue(PsyiaSettings.VortexStrength);
		uiPanel.GetControl("RoomCollisionToggle").SetBoolValue(PsyiaSettings.RoomCollision);
		uiPanel.GetControl("JellyModeToggle").SetBoolValue(PsyiaSettings.JellyMode);

		uiPanel.GetControl("ParticleCountSlider").SetFloatValue(PsyiaSettings.ParticleCount);
		switch(PsyiaSettings.Antialiasing) {
			case 0:	uiPanel.GetControl("AntialiasingRotary").SetIntValue(0); break;
			case 2:	uiPanel.GetControl("AntialiasingRotary").SetIntValue(1); break;
			case 4:	uiPanel.GetControl("AntialiasingRotary").SetIntValue(2); break;
			case 8:	uiPanel.GetControl("AntialiasingRotary").SetIntValue(3); break;
		}
		uiPanel.GetControl("BloomToggle").SetBoolValue(PsyiaSettings.Bloom);

		uiPanel.GetControl("VisualizationStrengthPhysicsSlider").SetFloatValue(PsyiaSettings.VisualizationStrengthPhysics);
		uiPanel.GetControl("VisualizationStrengthGraphicsSlider").SetFloatValue(PsyiaSettings.VisualizationStrengthGraphics);
		uiPanel.GetControl("LoopToggle").SetBoolValue(PsyiaSettings.Loop);
		uiPanel.GetControl("VolumeDial").SetFloatValue(PsyiaSettings.Volume);

		if(PsyiaSettings.ParticleCount != storedParticleCount) starLab.Reset();
	}

	void SavePreset(int number) {
		PsyiaSettings.SavePreset(number);
		CheckPresetExistence();
	}

	void Update() {
		if(starLab.noAudio) {
			nowPlayingText.text = "Now Playing:\nExternal Audio Source";
		} else {
			nowPlayingText.text = "Now Playing:\n" + trackNames[music.currentTrack];
		}
	}

	public void Slot1Load() { LoadPreset(1); }
	public void Slot2Load() { LoadPreset(2); }
	public void Slot3Load() { LoadPreset(3); }
	public void Slot4Load() { LoadPreset(4); }

	public void Slot1Save() { SavePreset(1); }
	public void Slot2Save() { SavePreset(2); }
	public void Slot3Save() { SavePreset(3); }
	public void Slot4Save() { SavePreset(4); }
}
