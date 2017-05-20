using UnityEngine;
using System.Collections;

public class BaseTouchToggle : MonoBehaviour {

	public Transform baseA;
	public Transform baseB;
	public float requiredDistance = 0.05f;
	public float requiredTime = 0.2f;
	public UIPanelMechanism panel;

	float currentTime = 0f;

	bool ready = true;
	
	// Update is called once per frame
	void Update () {
		float distance = (baseA.position - baseB.position).magnitude;
		if(distance < requiredDistance && ready) {
			currentTime += Time.deltaTime;
			VRTools.VRDebug.DrawLine(baseA.position, baseB.position, Color.white, true, Mathf.Lerp(0.0001f, 0.005f, Mathf.InverseLerp(0f, requiredTime, currentTime)));
		} else {
			currentTime = 0f;
		}

		if(currentTime > requiredTime) {
			ready = false;
			StartCoroutine(PulseLine(baseA.position, baseB.position));
			panel.Pop();
		}
	}

	IEnumerator PulseLine(Vector3 posA, Vector3 posB) {
		for(float i = 0; i < 1f; i += Time.deltaTime / 0.8f) {
			VRTools.VRDebug.DrawLine(posA, posB, Color.Lerp(Color.white, Color.black, i), true, Mathf.Lerp(0.005f, 0.02f, -1 * (i - 1) * (i - 1) + 1f));
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		ready = true;
	}
}
