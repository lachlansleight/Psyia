using UnityEngine;
using System.Collections;

public class TouchToBeginSphere : MonoBehaviour {

	bool triggered = false;
	public Dispatcher dispatcher;
	public GameObject touchToBeginCanvas;
	public Renderer touchToBeginRenderer;
	public float waitTime = 2f;
	public float growTime = 5.5f;
	public bool tutorialScene = false;

	// Use this for initialization
	void Start () {
		transform.localScale = Vector3.zero;
		StartCoroutine(LerpUp());
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0f, 20f * Time.deltaTime, 0f, Space.World);
		if(!triggered) return;
		if((VRTools.VRInput.GetDevice("ViveLeft").position - transform.position).magnitude < 0.025f) {
			if(tutorialScene) {
				UnityEngine.SceneManagement.SceneManager.LoadScene("Stars_Test");
			} else {
				dispatcher.StartCoroutine(dispatcher.burstOut());
				touchToBeginCanvas.SetActive(false);
				gameObject.SetActive(false);
				triggered = false;
				GameObject.Find("Music").GetComponent<AudioSource>().Play();
			}
		} else if((VRTools.VRInput.GetDevice("ViveRight").position - transform.position).magnitude < 0.025f) {
			if(tutorialScene) {
				UnityEngine.SceneManagement.SceneManager.LoadScene("Stars_Test");
			} else {
				dispatcher.StartCoroutine(dispatcher.burstOut());
				touchToBeginCanvas.SetActive(false);
				gameObject.SetActive(false);
				triggered = false;
				GameObject.Find("Music").GetComponent<AudioSource>().Play();
			}
		} else if(Input.GetKeyDown(KeyCode.Space)) {
			if(tutorialScene) {
				UnityEngine.SceneManagement.SceneManager.LoadScene("Stars_Test");
			} else {
				dispatcher.StartCoroutine(dispatcher.burstOut());
				touchToBeginCanvas.SetActive(false);
				gameObject.SetActive(false);
				triggered = false;
				GameObject.Find("Music").GetComponent<AudioSource>().Play();
			}
		}
	}

	IEnumerator LerpUp() {
		touchToBeginCanvas.SetActive(false);

		yield return new WaitForSeconds(waitTime);
		for(float i = 0; i < 1f; i += Time.deltaTime / growTime) {
			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 0.05f, i);
			yield return null;
		}
		
		triggered = true;

		yield return new WaitForSeconds(0.5f);

		touchToBeginCanvas.SetActive(true);

		for(float i = 0; i < 1f; i += Time.deltaTime / 0.5f) {
			touchToBeginRenderer.sharedMaterial.SetColor("_Color", Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, i));
			yield return null;
		}
	}
}
