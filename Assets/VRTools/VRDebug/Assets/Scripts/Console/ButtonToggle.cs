using UnityEngine;
using System.Collections;

namespace VRTools {

	public class ButtonToggle : MonoBehaviour {

		public Sprite offSprite;
		public Sprite onSprite;
		public UnityEngine.UI.Image background;

		public void SetOn(bool newOn) {
			background.sprite = newOn ? onSprite : offSprite;
		}
	}

}