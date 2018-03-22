using UnityEngine;
using System.Collections;

public class AudioData : MonoBehaviour {

	public SoundCapture capture;
	[Range(0f, 1f)]public float bassVol;
	[Range(0f, 1f)]public float midVol;
	[Range(0f, 1f)]public float trebleVol;
	[Range(0f, 1f)]public float avgVol;

	[Range(0f, 10f)]public float globalMult = 1f;

	public float[] fft;

	public float maxSeenVolume = 0.0000001f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(capture.barData.Length <= 0) return;
		fft = new float[capture.barData.Length];
		lock(capture.barData)
        {
            for (int i = 0; i < capture.barData.Length; i++)
            {
				

				avgVol += capture.barData[i];

                if(i < capture.barData.Length / 3) {
                	bassVol += capture.barData[i];
                } else if(i < 2 * capture.barData.Length / 3) {
					midVol += capture.barData[i];
                } else {
					trebleVol += capture.barData[i];
                }
            }

			avgVol /= (float)capture.barData.Length;
			bassVol /= (float)(capture.barData.Length / 3);
			midVol /= (float)(capture.barData.Length / 3);
			trebleVol /= (float)(capture.barData.Length / 3);

			/*
			avgVol *= globalMult;
			midVol *= globalMult;
			trebleVol *= globalMult;
			bassVol *= globalMult;
			*/

			if(avgVol > maxSeenVolume) maxSeenVolume = avgVol;

			avgVol /= maxSeenVolume;
			bassVol /= maxSeenVolume;
			midVol /= maxSeenVolume;
			trebleVol /= maxSeenVolume;

			for(int i = 0; i < fft.Length; i++) {
				fft[i] = capture.barData[i] / maxSeenVolume;
			}
        }
	}
}
