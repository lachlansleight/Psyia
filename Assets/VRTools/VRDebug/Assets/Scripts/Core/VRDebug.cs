using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRTools {

	/// <summary>
	/// Main debug class
	/// </summary>
	public class VRDebug : MonoBehaviour {

		static Mesh mesh;
		static Material lineMat;
		static Material lineDepthMat;
		static List<WorldDebugLine> lines;
		static int logID;
		public static List<Log> logs;
		public static List<Log> collapsedLogs;
		public static int errorCount;
		public static int warningCount;
		public static int logCount;
		public static int collapsedErrorCount;
		public static int collapsedWarningCount;
		public static int collapsedLogCount;

		static Console console;
		static ConsoleMin minConsole;

		public delegate void LogAction(Log newLog, bool collapsed);
		public static event LogAction OnNewLog;
		public static event LogAction OnNewCollapsedLog;

		void OnEnable() {
			logs = new List<Log>();
			collapsedLogs = new List<Log>();
			Application.logMessageReceived += HandleLog;

			GameObject tempCylinder = GameObject.CreatePrimitive(PrimitiveType.Cube);
			mesh = new Mesh();
			Mesh copyMesh = tempCylinder.GetComponent<MeshFilter>().mesh;
			mesh.vertices = copyMesh.vertices;
			mesh.normals = copyMesh.normals;
			mesh.uv = copyMesh.uv;
			mesh.triangles = copyMesh.triangles;
			Destroy(tempCylinder);

			lines = new List<WorldDebugLine>();

			lineMat = new Material(Resources.Load("VRLineMat") as Material);
			lineDepthMat = new Material(Resources.Load("VRLineDepthMat") as Material);
		}

		public static void ToggleConsole(bool min) {
			if(min) {
				if(minConsole == null) {
					GameObject newObj = Instantiate(Resources.Load("VRDebugConsoleMin"), Vector3.zero, Quaternion.identity) as GameObject;
					minConsole = newObj.GetComponent<ConsoleMin>();
				} else {
					minConsole.gameObject.SetActive(!minConsole.gameObject.activeSelf);
				}

				if(console == null) {
					//do nothing
				} else {
					console.gameObject.SetActive(false);
				}
			} else {
				if(console == null) {
					GameObject newObj = Instantiate(Resources.Load("VRDebugConsole"), Vector3.zero, Quaternion.identity) as GameObject;
					console = newObj.GetComponent<Console>();
				} else {
					console.gameObject.SetActive(!console.gameObject.activeSelf);
				}

				if(minConsole == null) {
					//do nothing
				} else {
					minConsole.gameObject.SetActive(false);
				}
			}
		}

		void HandleLog (string message, string stackTrace, LogType type) {
			if(logs == null) logs = new List<Log>();
			if(collapsedLogs == null) collapsedLogs = new List<Log>();

			Log newLog = new Log();
			newLog.id = logID;
			newLog.message = message;
			newLog.stackTrace = stackTrace;
			newLog.type = type;
			newLog.time = Time.time;
			newLog.realtimeSinceStartup = Time.realtimeSinceStartup;
			newLog.frameCount = Time.frameCount;
			newLog.count = 1;

			logID++;

			if(type.Equals(LogType.Log)) logCount++;
			else if(type.Equals(LogType.Warning)) warningCount++;
			else errorCount++;

			logs.Add(newLog);
			if(OnNewLog != null) OnNewLog(newLog, false);

			bool found = false;
			for(int i = 0; i < collapsedLogs.Count; i++) {
				if(collapsedLogs[i].message == newLog.message && collapsedLogs[i].stackTrace == newLog.stackTrace) {
					collapsedLogs[i] = incrementLogCount(collapsedLogs[i]);
					found = true;
					break;
				}
			}

			if(!found) {
				if(type.Equals(LogType.Log)) collapsedLogCount++;
				else if(type.Equals(LogType.Warning)) collapsedWarningCount++;
				else collapsedErrorCount++;
				collapsedLogs.Add(newLog);
			}

			if(OnNewCollapsedLog != null) OnNewCollapsedLog(newLog, true);
		}

		Log incrementLogCount(Log source) {
			Log newLog = new Log();
			newLog.id = source.id;
			newLog.message = source.message;
			newLog.stackTrace = source.stackTrace;
			newLog.type = source.type;
			newLog.time = source.time;
			newLog.realtimeSinceStartup = source.realtimeSinceStartup;
			newLog.frameCount = source.frameCount;
			newLog.count = source.count + 1;
			return newLog;
		}

		void Update() {
			for(int i = 0; i < lines.Count; i++) {
				if(Time.time > lines[i].stopTime) {
					Destroy(lines[i].material);
					lines.RemoveAt(i);
					continue;
				}
				Graphics.DrawMesh(mesh, lines[i].matrix, lines[i].material, 0);
			}

			//if(VRInput.GetDevice("ViveLeft").GetButtonDown("Grip")) VRDebug.ToggleConsole();
			if(VRInput.HasDevice("ViveLeft")) {
				if(VRInput.GetDevice("ViveLeft").GetButtonDown("Grip") && VRInput.GetDevice("ViveLeft").GetButton("Menu")) {
					VRDebug.ToggleConsole(false);
				} else if(VRInput.GetDevice("ViveLeft").GetButtonDown("Menu") && VRInput.GetDevice("ViveLeft").GetButton("Grip")) {
					VRDebug.ToggleConsole(true);
				}
			}

		}
		public static void DrawRay(Vector3 start, Vector3 dir) {
			DrawLine(start, start + dir);
		}
		public static void DrawRay(Vector3 start, Vector3 dir, Color color) {
			DrawLine(start, start + dir, color);
		}
		public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration) {
			DrawLine(start, start + dir, color, duration);
		}
		public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest) {
			DrawLine(start, start + dir, color, duration, depthTest);
		}
		public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest, float radius) {
			DrawLine(start, start + dir, color, duration, depthTest, radius);
		}


		public static void DrawLine(Vector3 start, Vector3 end) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation;
			if((end - start).magnitude == 0) rotation = Quaternion.identity;
			else rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(0.005f, 0.005f, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = 0;
			newLine.material = new Material(lineMat);
			newLine.material.color = Color.white;

			Graphics.DrawMesh(mesh, newLine.matrix, newLine.material, 0);
		}
		public static void DrawLine(Vector3 start, Vector3 end, Color color) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation;
			if((end - start).magnitude == 0) rotation = Quaternion.identity;
			else rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(0.005f, 0.005f, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = 0;
			newLine.material = new Material(lineMat);
			newLine.material.color = color;

			Graphics.DrawMesh(mesh, newLine.matrix, newLine.material, 0);
		}
		public static void DrawLine(Vector3 start, Vector3 end, Color color, bool depthTest, float radius) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation;
			if((end - start).magnitude == 0) rotation = Quaternion.identity;
			else rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(radius, radius, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = 0;
			newLine.material = depthTest ? new Material(lineDepthMat) : new Material(lineMat);
			newLine.material.color = color;

			Graphics.DrawMesh(mesh, newLine.matrix, newLine.material, 0);
		}
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float time) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(0.005f, 0.005f, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = Time.time + time;
			newLine.material = new Material(lineMat);
			newLine.material.color = color;
			lines.Add(newLine);
		}
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float time, bool depthTest) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(0.005f, 0.005f, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = Time.time + time;
			newLine.material = depthTest ? new Material(lineDepthMat) : new Material(lineMat);
			newLine.material.color = color;
			lines.Add(newLine);
		}
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float time, bool depthTest, float radius) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(radius, radius, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = Time.time + time;
			newLine.material = depthTest ? new Material(lineDepthMat) : new Material(lineMat);
			newLine.material.color = color;
			lines.Add(newLine);
		}


		public static void Log(object message) {
			Debug.Log(message);
		}
		public static void Log(object message, Object context) {
			Debug.Log(message, context);
		}

		public static void ClearConsole() {
			logs.Clear();
			collapsedLogs.Clear();
			errorCount = 0;
			warningCount = 0;
			logCount = 0;
			collapsedErrorCount = 0;
			collapsedWarningCount = 0;
			collapsedLogCount = 0;
		}
	}

}
