using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace VRTools {

	public class ConsoleMin : MonoBehaviour {

		public Text mainText;

		public float retentionTime = 10f;

		int lastRecordedMessage = -1;

		private struct MinMessage {
			public float deleteTime;
			public string message;
		}

		System.Collections.Generic.List<MinMessage> messages;

		// Use this for initialization
		void Start () {
			messages = new System.Collections.Generic.List<MinMessage>();
		}
		
		// Update is called once per frame
		void Update () {
			if(messages == null) messages = new System.Collections.Generic.List<MinMessage>();
			for(int i = 0; i < VRDebug.logs.Count; i++) {
				if(VRDebug.logs[i].id > lastRecordedMessage) {
					MinMessage newMessage = new MinMessage();
					newMessage.message = VRDebug.logs[i].message;
					newMessage.deleteTime = Time.time + retentionTime;
					messages.Add(newMessage);

					lastRecordedMessage = VRDebug.logs[i].id;
				}
			}


			string correctString = "";
			for(int i = 0; i < messages.Count; i++) {
				float timeRemaining = messages[i].deleteTime - Time.time;
				if(timeRemaining <= 1f) {
					correctString = "<color=" + getHexCol((int)Mathf.Lerp(222f, 0f, timeRemaining)) + ">" + messages[i].message + "</color>\n\n" + correctString;
				} else {
					correctString = messages[i].message + "\n\n" + correctString;
				}
			}

			mainText.text = correctString;

			for(int i = 0; i < messages.Count; i++) {
				if(messages[i].deleteTime < Time.time) messages.RemoveAt(i);
			}
		}

		string getHexCol(int greyscaleValue) {
			string hexOrder = "0123456789ABCDEF";
			int firstDigit = Mathf.FloorToInt((float)greyscaleValue / 16f);
			int secondDigit = greyscaleValue % 16;
			return "#" + hexOrder[firstDigit] + hexOrder[secondDigit] + hexOrder[firstDigit] + hexOrder[secondDigit] + hexOrder[firstDigit] + hexOrder[secondDigit];
		}
	}

}