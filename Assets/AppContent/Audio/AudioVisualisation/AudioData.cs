using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AudioData : MonoBehaviour
{

	private static AudioData _instance;

	public static AudioData Instance
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType<AudioData>();
			return _instance;
		}
	}
	
	public SoundCapture capture;
	[Range(0f, 1f)]public float bassVol;
	[Range(0f, 1f)]public float midVol;
	[Range(0f, 1f)]public float trebleVol;
	[Range(0f, 1f)]public float avgVol;

	[Range(0f, 10f)]public float globalMult = 1f;

	[Range(0f, 1f)] public float TwoSecondAverage = 0f;
	private List<float> _averageList;
	private float _twoSecondSum = 0;

	public float[] fft;

	public float maxSeenVolume = 0.0000001f;

	// Use this for initialization
	void Start ()
	{
		_averageList = new List<float>();
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

	        _averageList.Add(avgVol);
	        _twoSecondSum += avgVol;

	        if (_averageList.Count > 2f * 90f) {
		        _twoSecondSum -= _averageList[0];
		        _averageList.RemoveAt(0);
	        }

	        TwoSecondAverage = _twoSecondSum / _averageList.Count;

			for(int i = 0; i < fft.Length; i++) {
				fft[i] = capture.barData[i] / maxSeenVolume;
			}
        }
	}
}
