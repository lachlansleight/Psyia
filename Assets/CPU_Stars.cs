using UnityEngine;
using System.Collections;
using VRTools;

public class CPU_Stars : MonoBehaviour {

	public CPU_Star[] stars;
	public int count = 100;
	public Object CPU_Star_Prefab;

	Coroutine AddRoutine;

	// Use this for initialization
	void Start () {
		
		//PsyiaSettings.FirstTime = true;
		if(!PsyiaSettings.FirstTime) Destroy(gameObject);

		stars = new CPU_Star[count];
		for(int i = 0; i < stars.Length; i++) {
			GameObject newStar = Instantiate(CPU_Star_Prefab) as GameObject;
			newStar.name = "CPU_Star_" + i;
			newStar.transform.position = Random.insideUnitSphere * 0.1f + new Vector3(0, 1.2f, 0f);
			newStar.transform.parent = transform;
			stars[i] = newStar.GetComponent<CPU_Star>();
			stars[i].Initialise();
		}

		AddRoutine = StartCoroutine(AddSpheres());
	}
	
	// Update is called once per frame
	void Update () {
		VRInputDevice leftDevice = VRInput.GetDevice("ViveLeft");
		VRInputDevice rightDevice = VRInput.GetDevice("ViveRight");

		int numGrips = 0;
			if(leftDevice.GetButton("Grip")) numGrips++;
			if(rightDevice.GetButton("Grip")) numGrips++;

		for(int i = 0; i < stars.Length; i++) {
			stars[i].time = Time.time;

			stars[i].controllerPositionL = leftDevice.position;
			stars[i].controllerForwardL = leftDevice.forward;
			stars[i].controllerChargeL = leftDevice.GetButton("Touchpad") ? -0.1f : leftDevice.GetAxis("Trigger");

			stars[i].controllerPositionR = rightDevice.position;
			stars[i].controllerForwardR = rightDevice.forward;
			stars[i].controllerChargeR = rightDevice.GetButton("Touchpad") ? -0.1f : rightDevice.GetAxis("Trigger");

			if(numGrips == 0) stars[i].timeScale = Mathf.Lerp(stars[i].timeScale, 1f, 0.05f);
			else if(numGrips == 1) stars[i].timeScale = Mathf.Lerp(stars[i].timeScale, 0.1f, 0.05f);
			else if(numGrips == 2) stars[i].timeScale = Mathf.Lerp(stars[i].timeScale, 0f, 0.05f);
		}
	}

	public void End() {
		StopCoroutine(AddRoutine);
		StartCoroutine(KillStars());
	}

	IEnumerator KillStars() {
		int killCount = 0;
		for(int i = 0; i < stars.Length; i++) {
			stars[i].Kill((float)i * 0.2f);
			killCount++;
		}
		int storedCount = killCount;

		while(killCount > 0) {
			killCount = 0;

			for(int i = 0; i < stars.Length; i++) {
				if(stars[i].invisible) killCount++;
			}

			yield return null;
		}
	}

	IEnumerator AddSpheres() {
		yield return new WaitForSeconds(5f);
		float totalTime = 30f;
		for(float i = 0; i < 1f; i += Time.deltaTime / totalTime) {
			int currentIndex = Mathf.FloorToInt(i * count - 1);
			for(int j = 0; j < currentIndex; j++) {
				if(!stars[j].myRenderer.enabled) stars[j].PopIn();
			}

			yield return null;
		}
	}
}
