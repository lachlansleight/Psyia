using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioBarsUI : MonoBehaviour {

	public AudioData audioData;

	public int noBars = 60;
	public float barWidth;
	public float totalWidth = 315.3f;
	public float totalHeight = 190f;
	public float barGap = 0.5f;

	RectTransform[] bars;

	// Use this for initialization
	void Awake () {
		if(bars != null) return;

		bars = new RectTransform[noBars];
		bars[0] = transform.GetChild(0).GetComponent<RectTransform>();

		barWidth = totalWidth / (float)bars.Length;

		for(int i = 1; i < bars.Length; i++) {
			GameObject newObj = Instantiate(bars[0].gameObject) as GameObject;
			bars[i] = newObj.GetComponent<RectTransform>();
			bars[i].SetParent(transform);
			bars[i].localScale = Vector3.one;
			bars[i].localRotation = Quaternion.identity;
			bars[i].localPosition = Vector3.zero;
		}

		for(int i = 0; i < bars.Length; i++) {
			float iT = (float)i / (float)bars.Length;
			bars[i].offsetMin = new Vector2(iT * barWidth + (barWidth * 0.5f), 0f);
			bars[i].offsetMax = new Vector2((1f - iT) * barWidth - (barWidth * 0.5f), totalHeight);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(bars == null) return;
		for(int i = 0; i < bars.Length; i++) {
			try {
				float iT = (float) i / (float) bars.Length;
				bars[i].offsetMin = new Vector2(iT * totalWidth - (totalWidth * 0.5f) + (barGap * 0.5f), 0f);
				bars[i].offsetMax = new Vector2(iT * totalWidth - (totalWidth * 0.5f) + barWidth - (barGap * 0.5f),
					Mathf.Clamp((audioData.fft[i] * totalHeight * 0.3f - totalHeight), -totalHeight, 0f));
			} catch(System.IndexOutOfRangeException e)
			{
				//all good
			}
		}
	}
}
