using UnityEngine;
using System.Collections;

namespace VRTools {

	[System.Serializable]
	public struct Log {
		public int id;
		public string message;
		public string stackTrace;
		public LogType type;
		public float realtimeSinceStartup;
		public float time;
		public int frameCount;
		public int count;
	}

	public struct WorldDebugLine {
		public Matrix4x4 matrix;
		public float stopTime;
		public Material material;
	}

	public enum LogStyle {
		Error,
		Warning,
		Log
	}

	public enum ConsoleCollider {
		None,
		Background,
		MainWindow,
		SubWindow,
		ClearButton,
		CollapseButton,
		LogToggleButton,
		WarningToggleButton,
		ErrorToggleButton,
		StickToControllerButton
	}
}