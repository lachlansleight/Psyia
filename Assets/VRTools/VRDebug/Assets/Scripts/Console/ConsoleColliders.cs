using UnityEngine;
using System.Collections;

namespace VRTools {

	public class ConsoleColliders : MonoBehaviour {

		public Collider background;
		public Collider[] colliders;
		public ConsoleCollider[] colliderValues;

		// Use this for initialization
		void Start () {
		
		}

		void Update() {
			if(!VRInput.GetDevice("ViveRight").isTracked) return;

			RaycastHit tempHit;
			if(background.Raycast(new Ray(VRInput.GetDevice("ViveRight").position, VRInput.GetDevice("ViveRight").forward), out tempHit, Mathf.Infinity)) {
				VRDebug.DrawLine(VRInput.GetDevice("ViveRight").position, tempHit.point, Color.black, true, 0.0015f);
			}
		}
		
		public ConsoleCollider GetCollider(out RaycastHit hit) {
			hit = new RaycastHit();

			if(!VRInput.GetDevice("ViveRight").isTracked) return ConsoleCollider.None;
			
			bool found = false;
			int foundIndex = -1;
			for(int i = 0; i < colliders.Length; i++) {
				if(colliders[i].Raycast(new Ray(VRInput.GetDevice("ViveRight").position, VRInput.GetDevice("ViveRight").forward), out hit, Mathf.Infinity)) {
					found = true;
					foundIndex = i;
					break;
				}
			}

			if(found) {
				return colliderValues[foundIndex];
			} else {
				if(background.Raycast(new Ray(VRInput.GetDevice("ViveRight").position, VRInput.GetDevice("ViveRight").forward), out hit, Mathf.Infinity)) {
					return ConsoleCollider.Background;
				} else {
					return ConsoleCollider.None;
				}
			}
		}
	}

}