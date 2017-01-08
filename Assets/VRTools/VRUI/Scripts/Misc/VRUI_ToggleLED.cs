using UnityEngine;
using System.Collections;

namespace VRTools {
	namespace UI {

		public class VRUI_ToggleLED : MonoBehaviour {

			public VRUI_Toggle toggle;

			public Color offColor;
			public Color onColor;

			Material myMat;

			// Use this for initialization
			void Start () {
				myMat = GetComponent<Renderer>().material;
			}
	
			// Update is called once per frame
			void Update () {
				myMat.SetColor("_EmissionColor", toggle.Toggled ? onColor : offColor);
			}
		}
	}
}