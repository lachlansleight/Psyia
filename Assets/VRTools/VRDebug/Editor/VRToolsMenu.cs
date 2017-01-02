using UnityEngine;
using UnityEditor;
using System.Collections;

namespace VRTools {

	public class VRToolsMenu {

		[MenuItem("GameObject/Create Other/VR Debug")]
		static void CreateVRDebug() {
			if(GameObject.FindObjectsOfType<VRTools.VRDebug>().Length > 0) {
				Debug.LogWarning("VR Debug Object appears to already exist, cancelling operation");
				return;
			}
			GameObject newObject = new GameObject("VR Debug");
			newObject.AddComponent<VRTools.VRDebug>();
			ViveInput viveInput = newObject.AddComponent<ViveInput>();
			GameObject parent = GameObject.FindObjectOfType<SteamVR_TrackedObject>().transform.parent.gameObject;
			viveInput.Left = parent.transform.GetChild(0).GetComponent<SteamVR_TrackedObject>();
			viveInput.Right = parent.transform.GetChild(1).GetComponent<SteamVR_TrackedObject>();
			#if UNITY_5_4_OR_NEWER
			viveInput.hmd = parent.transform.GetChild(2).transform;
			#else 
			viveInput.hmd = parent.transform.GetChild(2).GetComponent<SteamVR_TrackedObject>();
			#endif
			Undo.RegisterCreatedObjectUndo(newObject, "Create VR Debug Object");
		}
	}

}