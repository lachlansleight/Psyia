using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

namespace VRTools {

	/// <summary>
	/// This goes on the world space Console canvas gameobject
	/// </summary>
	public class Console : MonoBehaviour {

		public ConsoleColliders colliders;

		public Text selectedText;
		public Transform logParent;
		public RectTransform mainWindowContent;
		public RectTransform selectedWindowContent;
		public RectTransform mainWindowScrollbar;
		public RectTransform selectedWindowScrollbar;
		public ConsoleToggle LogToggle;
		public ConsoleToggle WarningToggle;
		public ConsoleToggle ErrorToggle;
		public ButtonToggle CollapseToggle;
		public ButtonToggle StickToControllerToggle;

		List<LogMessage> logMessages;
		List<LogMessage> collapsedLogMessages;

		float mainStartIndex = 0f;
		float selectedOffset = 0f;

		public Object LogPrefab;
		public Sprite[] Icons;
		public Color colorA = new Color32(222, 222, 222, 255);
		public Color colorB = new Color32(216, 216, 216, 255);
		public Color selectedColor = new Color32(62, 125, 231, 255);

		public bool collapse = false;
		public bool showLogs = true;
		public bool showWarnings = true;
		public bool showErrors = true;

		public float mainScrollSpeed = 500f;

		bool atBottom = true;

		LogMessage selectedMessage = null;

		void Start() {
			logMessages = new List<LogMessage>();
			collapsedLogMessages = new List<LogMessage>();
			//VRDebug.OnNewLog += CreateLog;
			selectedText.text = "";
			StickToControllerToggle.SetOn(true);
		}

		void TestLogs() {
			Debug.Log("Normal Log\nOn Two lines...\nOn three lines\nOn more lines!\nOn more lines!\nOn more lines!\nOn more lines!\nOn more lines!\nOn more lines!\nOn more lines!");
			Debug.LogWarning("Warning");
			Debug.LogError("Error");
			Debug.Assert(1 == 2);
			Debug.Assert(1 == 1);
		}

		void Update() {
			if(Input.GetKeyDown(KeyCode.Space)) {
				TestLogs();
			}


			mainWindowContent.offsetMin = new Vector2(0f, 70f * mainStartIndex);
			mainWindowContent.offsetMax = new Vector2(0f, 70f * mainStartIndex);

			selectedWindowContent.offsetMin = new Vector2(0f, selectedOffset);
			selectedWindowContent.offsetMax = new Vector2(0f, selectedOffset);

			//if(Time.frameCount % 100 == 0) Debug.Log(selectedOffset);

			DoRaycast();
			UpdateBarSizes();

			//Get the correct logs
			List<Log> intendedLogs = new List<Log>();
			if(collapse) {
				for(int i = 0; i < VRDebug.collapsedLogs.Count; i++) {
					if(shouldShow(VRDebug.collapsedLogs[i])) intendedLogs.Add(VRDebug.collapsedLogs[i]);
				}
			} else {
				for(int i = 0; i < VRDebug.logs.Count; i++) {
					if(shouldShow(VRDebug.logs[i])) intendedLogs.Add(VRDebug.logs[i]);
				}
			}
			string temp = "IntendedLogs:";
			for(int i = 0; i < intendedLogs.Count; i++) {
				temp += "\n" + intendedLogs[i].message;
			}
			//Debug.Log(temp);
			temp = "Current logs:";
			for(int i = 0; i < (collapse ? logMessages.Count : collapsedLogMessages.Count); i++) {
				temp += "\n" + (collapse ? logMessages[i].log.message : collapsedLogMessages[i].log.message);
			}
			//Debug.Log(temp);

			//Get rid of the log messages we shouldn't have
			if(collapse) {
				for(int i = 0; i < collapsedLogMessages.Count; i++) {
					if(!idExists(collapsedLogMessages[i].log.id, intendedLogs)) {
						//destroy current log
						Destroy(collapsedLogMessages[i].gameObject);
						collapsedLogMessages.RemoveAt(i);
					}
				}
			} else {
				for(int i = 0; i < logMessages.Count; i++) {
					if(!idExists(logMessages[i].log.id, intendedLogs)) {
						//destroy current log
						Destroy(logMessages[i].gameObject);
						logMessages.RemoveAt(i);
					}
				}
			}

			//Add the ones we should have
			if(collapse) {
				for(int i = 0; i < intendedLogs.Count; i++) {
					if(!idExists(intendedLogs[i].id, collapsedLogMessages)) {
						//add current intended log gameobject
						CreateLog(intendedLogs[i], true);
					}
				}
			} else {
				for(int i = 0; i < intendedLogs.Count; i++) {
					if(!idExists(intendedLogs[i].id, logMessages)) {
						//add current intended log gameobject
						CreateLog(intendedLogs[i], false);
					}
				}
			}

			//Sort by time
			if(collapse) {
				float[] times = new float[collapsedLogMessages.Count];
				for(int i = 0; i < collapsedLogMessages.Count; i++) {
					times[i] = collapsedLogMessages[i].log.realtimeSinceStartup;
				}
				LogMessage[] collapsedLogArray = collapsedLogMessages.ToArray();
				System.Array.Sort<float, LogMessage>(times, collapsedLogArray);
				for(int i = 0; i < collapsedLogMessages.Count; i++) {
					collapsedLogMessages[i] = collapsedLogArray[i];
				}
			} else {
				float[] times = new float[logMessages.Count];
				for(int i = 0; i < logMessages.Count; i++) {
					times[i] = logMessages[i].log.realtimeSinceStartup;
				}
				LogMessage[] logArray = logMessages.ToArray();
				System.Array.Sort<float, LogMessage>(times, logArray);
				for(int i = 0; i < logMessages.Count; i++) {
					logMessages[i] = logArray[i];
				}
			}
			//System.Array.Sort(

			//Reposition
			if(collapse) {
				for(int i = 0; i < collapsedLogMessages.Count; i++) {
					if(i < mainStartIndex - 1 || i > mainStartIndex + 14) {
						collapsedLogMessages[i].gameObject.SetActive(false);
					} else {
						collapsedLogMessages[i].gameObject.SetActive(true);
						collapsedLogMessages[i].transform.localPosition = Vector3.zero;
						collapsedLogMessages[i].transform.localScale = Vector3.one;
						collapsedLogMessages[i].transform.localRotation = Quaternion.identity;
						collapsedLogMessages[i].rectTransform.offsetMin = new Vector3(0f, -70f * (i+1));
						collapsedLogMessages[i].rectTransform.offsetMax = new Vector3(0f, 70f - 70f * (i+1));
					}
				}
			} else {
				for(int i = 0; i < logMessages.Count; i++) {
					if(i < mainStartIndex - 1 || i > mainStartIndex + 14) {
						logMessages[i].gameObject.SetActive(false);
					} else {
						logMessages[i].gameObject.SetActive(true);
						logMessages[i].transform.localPosition = Vector3.zero;
						logMessages[i].transform.localScale = Vector3.one;
						logMessages[i].transform.localRotation = Quaternion.identity;
						logMessages[i].rectTransform.offsetMin = new Vector3(0f, -70f * (i+1));
						logMessages[i].rectTransform.offsetMax = new Vector3(0f, 70f - 70f * (i+1));
					}
				}
			}

			//if(atBottom) mainStartIndex = Mathf.Max(0f, 13f - getNumToShow());

			//get list of logs that should be visible
			/*
			List<Log> correctLogs = new List<Log>();
			if(collapse) {
				for(int i = Mathf.FloorToInt(mainStartIndex); i < Mathf.CeilToInt(mainStartIndex + 13); i++) {
					if(i > VRDebug.collapsedLogs.Count) break;

					correctLogs.Add(VRDebug.collapsedLogs[i]);
				}
			} else {
				for(int i = Mathf.FloorToInt(mainStartIndex); i < Mathf.CeilToInt(mainStartIndex + 13); i++) {
					if(i > VRDebug.logs.Count) break;

					correctLogs.Add(VRDebug.logs[i]);
				}
			}*/

			//get list of logs that are visible


			//hide the ones that shouldn't be visible


			//spawn the ones that should be


			//then reposition
		}

		bool shouldShow(Log input) {
			if(input.type.Equals(LogType.Log)) {
				return showLogs;
			 } else if(input.type.Equals(LogType.Warning)) {
			 	return showWarnings;
			 } else return showErrors;
		}

		bool idExists(int id, List<Log> logs) {
			for(int i = 0; i < logs.Count; i++) {
				if(logs[i].id == id) return true;
			}
			return false;
		}
		bool idExists(int id, List<LogMessage> logs) {
			for(int i = 0; i < logs.Count; i++) {
				if(logs[i].log.id == id) return true;
			}
			return false;
		}

		void DoRaycast() {
			RaycastHit hit;
			ConsoleCollider hitCollider = colliders.GetCollider(out hit);
			switch(hitCollider) {
			case ConsoleCollider.MainWindow:
				//if(!ViveInput.Right.Buttons.Trigger.Equals(ButtonState.Pressed)) break;
				//float yValue = colliders.colliders[0].transform.InverseTransformPoint(hit.point).y;
				float yParam = VRInput.GetDevice("ViveLeft").GetAxis("TouchpadY");
				float sign = Mathf.Sign(yParam);
				yParam = Mathf.Abs(yParam);
				yParam *= 1.42857f;
				yParam -= 0.42857f;
				yParam = Mathf.Clamp01(yParam);
				yParam *= yParam;
				yParam *= sign;

				if(Mathf.Abs(yParam) < 0.05f) yParam = 0f;

				mainStartIndex -= yParam * mainScrollSpeed * Time.deltaTime;
				mainStartIndex = Mathf.Clamp(mainStartIndex, 0f, Mathf.Max(0f, getNumToShow() - 13));
				atBottom = (mainStartIndex >= Mathf.Max(0f, getNumToShow() - 13) - 1f);

				if(VRInput.GetDevice("ViveLeft").GetButtonDown("Trigger")) {
					int indexOfHit = 12 - (int)Mathf.Max(0f, Mathf.RoundToInt((colliders.colliders[0].transform.InverseTransformPoint(hit.point).y + 0.5f) * 13f - 0.7f));
					//Debug.Log("index of hit: " + indexOfHit);
					indexOfHit = Mathf.RoundToInt(indexOfHit + mainStartIndex);
					if(selectedMessage != null) {
						selectedMessage.SetColor(colorA);
						selectedMessage.SetMessageColor(Color.black);
						selectedMessage = null;
					}

					if(collapse) {
						if(indexOfHit < collapsedLogMessages.Count) {
							selectedMessage = collapsedLogMessages[indexOfHit];
							selectedMessage.SetColor(selectedColor);
							selectedMessage.SetMessageColor(Color.white);
							selectedText.text = selectedMessage.log.message + "\n" + selectedMessage.log.stackTrace;
							selectedOffset = 0f;
						}
					} else {
						if(indexOfHit < logMessages.Count) {
							selectedMessage = logMessages[indexOfHit];
							selectedMessage.SetColor(selectedColor);
							selectedMessage.SetMessageColor(Color.white);
							selectedText.text = selectedMessage.log.message + "\n" + selectedMessage.log.stackTrace;
							selectedOffset = 0f;
						}
					}
				}
				break;
			case ConsoleCollider.SubWindow:
				yParam = VRInput.GetDevice("ViveLeft").GetAxis("TouchpadY");
				sign = Mathf.Sign(yParam);
				yParam = Mathf.Abs(yParam);
				yParam *= 1.42857f;
				yParam -= 0.42857f;
				yParam = Mathf.Clamp01(yParam);
				yParam *= yParam;
				yParam *= sign;

				if(Mathf.Abs(yParam) < 0.05f) yParam = 0f;


				selectedOffset -= yParam * mainScrollSpeed * 20f * Time.deltaTime;
				selectedOffset = Mathf.Clamp(selectedOffset, 0f, Mathf.Max(0f, selectedText.preferredHeight - 210f));

				//if(Time.frameCount % 30 == 0) Debug.Log("selectedText.preferredHeight: " + selectedText.preferredHeight + "\nselectedOffset: " + selectedOffset + "\nyParam: " + yParam);
				break;
			case ConsoleCollider.ClearButton:
				if(VRInput.GetDevice("ViveRight").GetButtonDown("Trigger")) {
					for(int i = 0; i < logMessages.Count; i++) {
						Destroy(logMessages[i].gameObject);
					}
					for(int i = 0; i < collapsedLogMessages.Count; i++) {
						Destroy(collapsedLogMessages[i].gameObject);
					}
					logMessages.Clear();
					collapsedLogMessages.Clear();
					mainStartIndex = 0;
					VRDebug.ClearConsole();
				}
				break;
			case ConsoleCollider.CollapseButton:
				if(VRInput.GetDevice("ViveRight").GetButtonDown("Trigger")) {
					collapse = !collapse;
					CollapseToggle.SetOn(collapse);
					if(collapse) {
						for(int i = 0; i < logMessages.Count; i++) logMessages[i].gameObject.SetActive(false);
						for(int i = 0; i < collapsedLogMessages.Count; i++) collapsedLogMessages[i].gameObject.SetActive(true);
					} else {
						for(int i = 0; i < collapsedLogMessages.Count; i++) collapsedLogMessages[i].gameObject.SetActive(false);
						for(int i = 0; i < logMessages.Count; i++) logMessages[i].gameObject.SetActive(true);
					}
				}
				break;
			case ConsoleCollider.StickToControllerButton:
				if(VRInput.GetDevice("ViveRight").GetButtonDown("Trigger")) {
					GetComponent<FollowTransform>().enabled = !GetComponent<FollowTransform>().enabled;
					StickToControllerToggle.SetOn(GetComponent<FollowTransform>().enabled);
				}
				break;
			case ConsoleCollider.LogToggleButton:
				if(VRInput.GetDevice("ViveRight").GetButtonDown("Trigger")) {
					LogToggle.Toggle();
				}
				break;
			case ConsoleCollider.WarningToggleButton:
				if(VRInput.GetDevice("ViveRight").GetButtonDown("Trigger")) {
					WarningToggle.Toggle();
				}
				break;
			case ConsoleCollider.ErrorToggleButton:
				if(VRInput.GetDevice("ViveRight").GetButtonDown("Trigger")) {
					ErrorToggle.Toggle();
				}
				break;
			}
		}

		int getNumToShow() {
			int numValid = 0;

			if(collapse) {
				if(showLogs) numValid += VRDebug.collapsedLogCount;
				if(showWarnings) numValid += VRDebug.collapsedWarningCount;
				if(showErrors) numValid += VRDebug.collapsedErrorCount;
			} else {
				if(showLogs) numValid += VRDebug.logCount;
				if(showWarnings) numValid += VRDebug.warningCount;
				if(showErrors) numValid += VRDebug.errorCount;
			}
			return numValid;
		}

		void UpdateBarSizes() {
			int numValid = getNumToShow();

			float targetHeight = Mathf.Lerp(30f, 940f, Mathf.InverseLerp(100f, 13f, numValid));

			float targetPositionA = Mathf.Lerp(0f, -(940f - targetHeight), Mathf.InverseLerp(0, getNumToShow() - 13, mainStartIndex));
			float targetPositionB = Mathf.Lerp(940f - targetHeight, 0f, Mathf.InverseLerp(0, getNumToShow() - 13, mainStartIndex));

			mainWindowScrollbar.offsetMax = new Vector2(-3f, targetPositionA);
			mainWindowScrollbar.offsetMin = new Vector2(3f, targetPositionB);

			float maxHeight = selectedText.preferredHeight;
			targetHeight = Mathf.Lerp(30f, 210f, Mathf.Clamp01(Mathf.InverseLerp(500f, 210f, maxHeight)));

			targetPositionA = Mathf.Lerp(0f, -(210f - targetHeight), Mathf.InverseLerp(0f, maxHeight - 210f, selectedOffset));
			targetPositionB = Mathf.Lerp(210f - targetHeight, 0f, Mathf.InverseLerp(0f, maxHeight - 210f, selectedOffset));

			selectedWindowScrollbar.offsetMax = new Vector2(-3f, targetPositionA);
			selectedWindowScrollbar.offsetMin = new Vector2(3f, targetPositionB);
		}

		void CreateLog(Log newLog, bool isCollapsed) {
			if(logMessages == null || collapsedLogMessages == null) {
				logMessages = new List<LogMessage>();
				collapsedLogMessages = new List<LogMessage>();
			}

			int index = isCollapsed ? VRDebug.collapsedLogs.IndexOf(newLog) : VRDebug.logs.IndexOf(newLog);

			GameObject newObj = Instantiate(LogPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			newObj.transform.SetParent(logParent);
			newObj.transform.localPosition = Vector3.zero;
			newObj.transform.localScale = Vector3.one;
			newObj.transform.localRotation = Quaternion.identity;
			//newObj.GetComponent<RectTransform>().offsetMin = new Vector3(0f, -70f * (index+1));
			//newObj.GetComponent<RectTransform>().offsetMax = new Vector3(0f, 70f - 70f * (index+1));

			if(isCollapsed) collapsedLogMessages.Add(newObj.GetComponent<LogMessage>());
			else logMessages.Add(newObj.GetComponent<LogMessage>());

			Sprite iconType;
			Color color;
			switch(newLog.type) {
			case LogType.Exception:
				iconType = Icons[2];
				break;
			case LogType.Error:
				iconType = Icons[2];
				break;
			case LogType.Assert:
				iconType = Icons[2];
				break;
			case LogType.Warning:
				iconType = Icons[1];
				break;
			 default:
			 	iconType = Icons[0];
			 	break;
			}

			color = index % 2 == 0 ? colorA : colorB;

			newObj.GetComponent<LogMessage>().Initialise(newLog, iconType, color, isCollapsed);

			if(atBottom) {
				mainStartIndex = Mathf.Max(0f, getNumToShow() - 13);
			}
		}
	}

}