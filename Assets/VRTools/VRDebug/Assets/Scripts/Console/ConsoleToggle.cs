using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace VRTools {

	public class ConsoleToggle : MonoBehaviour {

		public Console console;

		public Sprite selectedGradient;
		public Sprite deselectedGradient;

		public Sprite selectedIcon;
		public Sprite deselectedIcon;

		public Image icon;
		public Image background;
		public Text numberText;

		public LogStyle style = LogStyle.Log;

		bool selected = true;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			switch(style) {
			case LogStyle.Log:
				numberText.text = "" + VRDebug.logCount;
				break;
			case LogStyle.Warning:
				numberText.text = "" + VRDebug.warningCount;
				if(VRDebug.warningCount > 0 && selected) icon.sprite = selectedIcon;
				else icon.sprite = deselectedIcon;
				break;
			case LogStyle.Error:
				numberText.text = "" + VRDebug.errorCount;
				if(VRDebug.errorCount > 0 && selected) icon.sprite = selectedIcon;
				else icon.sprite = deselectedIcon;
				break;
			}
		}

		public void Toggle() {
			if(selected) {
				icon.sprite = deselectedIcon;
				background.sprite = deselectedGradient;
			} else {
				icon.sprite = selectedIcon;
				background.sprite = selectedGradient;
			}

			selected = !selected;

			switch(style) {
			case LogStyle.Log:
				console.showLogs = selected;
				break;
			case LogStyle.Warning:
				console.showWarnings = selected;
				break;
			case LogStyle.Error:
				console.showErrors = selected;
				break;
			}
		}
	}

}