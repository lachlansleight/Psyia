using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace VRTools {

	public class LogMessage : MonoBehaviour {

		public bool collapsed;
		public RectTransform rectTransform;
		public Log log;
		public Text messageText;
		public Text countText;
		public Image icon;
		public Image background;
		public Image countBackground;

		float pulseAmount = 0f;
		int lastCount = 1;

		bool initialised;

		public void Initialise(Log sourceLog, Sprite iconSprite, Color backgroundColor, bool collapsedI) {
			log = sourceLog;
			rectTransform = GetComponent<RectTransform>();
			messageText.text = sourceLog.message + "\n" + sourceLog.stackTrace;
			countText.text = "" + sourceLog.count;
			lastCount = sourceLog.count;
			icon.sprite = iconSprite;
			background.color = backgroundColor;
			collapsed = collapsedI;

			initialised = true;
		}

		void Update() {
			if(!initialised) return;

			//Find my log in the log list
			int myIndex = -1;
			for(int i = 0; i < (collapsed ? VRDebug.collapsedLogs.Count : VRDebug.logs.Count); i++) {
				if((collapsed ? VRDebug.collapsedLogs[i].id : VRDebug.logs[i].id) == log.id) {
					myIndex = i;
					break;
				}
			}

			//if I'm collapsed, check if my count has changed - if so, update the textbox and pulse the icon
			int newCount = (collapsed ? VRDebug.collapsedLogs[myIndex].count : VRDebug.logs[myIndex].count);
			if(newCount != lastCount) {
				countText.text = "" + (collapsed ? VRDebug.collapsedLogs[myIndex].count : VRDebug.logs[myIndex].count);
				pulseAmount = 1f;
				lastCount = newCount;
			}

			//pulse icon
			if(pulseAmount > 0f) {
				countBackground.color = Color.Lerp(Color.white, new Color(0.8f, 0.8f, 0.8f), pulseAmount);
				pulseAmount = Mathf.Max(0f, pulseAmount - Time.deltaTime / 0.5f);
			}
		}

		public void SetColor(Color col) {
			background.color = col;
		}
		public void SetMessageColor(Color col) {
			messageText.color = col;
		}
	}

}