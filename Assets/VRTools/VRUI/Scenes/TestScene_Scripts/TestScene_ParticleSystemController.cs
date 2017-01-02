using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace VRTools
{
	namespace UI {

		public class TestScene_ParticleSystemController : MonoBehaviour
		{
			public Text sliderText;
			public Text buttonText;
			public Text toggleText;
			public Text dialText;
			public Text switchText;
			public Text xyText;
			public Text vectorText;
			public ParticleSystem debugSystem;

			public VRUI_Control[] controls;

			public Material[] mats;

			// Use this for initialization
			void Start()
			{
				controls[0].OnPressDown += ButtonPressDebug;
				controls[1].OnFloatChange += DialChangeDebug;
				controls[2].OnIntChange += RotaryChangeDebug;
				controls[3].OnFloatChange += SliderChangeDebug;
				controls[4].OnBooleanChange += ToggleSwitchedDebug;
				controls[5].OnVector3Change += VectorDebug;
				controls[6].OnVector2Change += XYChangeDebug;

				for(int i = 0; i < controls.Length; i++) controls[i].Bang();
			}

			void Update() {
				if(VRInput.GetDevice("ViveRight").GetButton("Grip")) {
					transform.position = Vector3.Lerp(transform.position, VRInput.GetDevice("ViveRight").position, 0.1f);
					transform.rotation = Quaternion.Lerp(transform.rotation, VRInput.GetDevice("ViveRight").rotation, 0.1f);
				}
			}

			void SliderChangeDebug(float val) {
				debugSystem.emissionRate = Mathf.Lerp(0f, 15000f, val);
				sliderText.text = "Emission Rate:\n" + Mathf.Round(debugSystem.emissionRate);
			}
			void ButtonPressDebug() {
				Color[] colors = new Color[] {
					Color.black, Color.white, Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta
				};
				string[] stringList = new string[] { "Black", "White", "Red", "Green", "Blue", "Yellow", "Cyan", "Magenta" };

				int choice = Random.Range(0, colors.Length);
				debugSystem.startColor = colors[choice];
				buttonText.text = "Color: " + stringList[choice];
			}
			void DialChangeDebug(float val) {
				dialText.text = "" + Mathf.Round(val * 100f) / 100f;
				debugSystem.startSize = Mathf.Lerp(0f, 0.1f, val);
				dialText.text = "Size: " + Mathf.Round(debugSystem.startSize * 100f) + "cm";
			}
			void RotaryChangeDebug(int val) {
				debugSystem.GetComponent<ParticleSystemRenderer>().material = mats[val];
				switchText.text = "Mode: " + mats[val].name;

			}
			void ToggleSwitchedDebug(bool val) {
				toggleText.text = "System " + (val ? "On" : "Off");
				debugSystem.enableEmission = val;
			}
			void XYChangeDebug(Vector2 val) {
				xyText.text = "Position[" + Mathf.Round(val.x * 100f) + ", " + Mathf.Round(val.y * 100f) + "]";
				debugSystem.transform.position = new Vector3(Mathf.Lerp(-10f, 10f, val.x), 0f, Mathf.Lerp(-10f, 10f, val.y));
			}
			void VectorDebug(Vector3 val) {
				debugSystem.transform.rotation = Quaternion.LookRotation(val);
				vectorText.text = "Travel Direction";
				//vectorText.text = "X:\t" + (Mathf.Round(val.x * 1000f) / 1000f) + "\nY:\t" + (Mathf.Round(val.y * 1000f) / 1000f) + "\nZ:\t" + (Mathf.Round(val.z * 1000f) / 1000f);
			}
		}

	}
}
