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

	


	public float MaxInstantLevelA = 0f;
	public float MaxAverageLevelA = 0f;
	
	public float MaxInstantLevelB = 0f;
	public float MaxAverageLevelB = 0f;

	public float MaxRmsRawA = 0f;
	public float MaxRmsRawB = 0f;

	[Header("Main outputs")]
	public bool AudioDetected;
	public float[] Fft;
	[Range(0f, 1f)] public float InstantLevel;
	[Range(0f, 1f)]public float AverageLevel;
	
	private float _twoSecondAverage;
	private List<float> _averageList;
	private float _twoSecondSum;

	public void Awake ()
	{
		_averageList = new List<float>();
	}
	
	public void Update () {
		if(capture.barData.Length <= 0) return;
		lock(capture.barData) {
			
			//Get RMS Volume
			var rmsVol = 0f;
            foreach (var t in capture.barData) {
	            rmsVol += t * t;
            }
			rmsVol /= capture.barData.Length;
			rmsVol = Mathf.Sqrt(rmsVol);
			
			//Update average
	        _averageList.Add(rmsVol);
	        _twoSecondSum += rmsVol;
	        if (_averageList.Count > 2f * 90f) {
		        _twoSecondSum -= _averageList[0];
		        _averageList.RemoveAt(0);
	        }
	        _twoSecondAverage = _twoSecondSum / _averageList.Count;

			//Update max values
	        if (_twoSecondAverage > MaxAverageLevelA) MaxAverageLevelA = _twoSecondAverage;
	        if (_twoSecondAverage > MaxAverageLevelB) MaxAverageLevelB = _twoSecondAverage;

			if (rmsVol > MaxRmsRawA) MaxRmsRawA = rmsVol;
			if (rmsVol > MaxRmsRawB) MaxRmsRawB = rmsVol;

			var rmsDifference = rmsVol - _twoSecondAverage;
			if (Mathf.Abs(rmsDifference) > MaxInstantLevelA) MaxInstantLevelA = Mathf.Abs(rmsVol);
			if (Mathf.Abs(rmsDifference) > MaxInstantLevelB) MaxInstantLevelB = Mathf.Abs(rmsVol);
	        
			//Calculate outputs
			AudioDetected = GetMaxAverageLevel() > 0.1f;
			Fft = new float[capture.barData.Length];
			for(var i = 0; i < Fft.Length; i++) {
				Fft[i] = capture.barData[i] / GetMaxRmsRaw();
			}
			InstantLevel = rmsDifference / (2f * GetMaxInstantLevel()) + 0.5f;
			AverageLevel = _twoSecondAverage / GetMaxAverageLevel();
        }
	}

	private float GetMaxInstantLevel()
	{
		var intervalIndex = Mathf.FloorToInt(Time.time / 60f) % 5;
		var intervalPosition = (Time.time / (60f * 5f)) % 1f;

		var multiplierA = 0f;
		var multiplierB = 0f;
		
		//all A for first interval
		if (intervalIndex == 0) {
			multiplierA = 1f;
			multiplierB = 0f;
		} else {
			//B fades
			if (intervalIndex % 2 == 0) {
				multiplierA = Mathf.Lerp(0f, 1f, Mathf.Clamp01(intervalPosition * 5f));
				multiplierB = Mathf.Lerp(1f, 0f, Mathf.Clamp01(intervalPosition * 5f));

				if (intervalPosition > 0.2f && intervalPosition < 0.21f) 
					MaxInstantLevelB = 0f;
				


				//A fades
			} else {
				multiplierA = Mathf.Lerp(1f, 0f, Mathf.Clamp01(intervalPosition * 5f));
				multiplierB = Mathf.Lerp(0f, 1f, Mathf.Clamp01(intervalPosition * 5f));

				if (intervalPosition > 0.2f && intervalPosition < 0.21f) 
					MaxInstantLevelA = 0f;
				

			}
		}

		return Mathf.Max(0.00001f, multiplierA * MaxInstantLevelA + multiplierB * MaxInstantLevelB);
	}
	
	private float GetMaxAverageLevel()
	{
		var intervalIndex = Mathf.FloorToInt(Time.time / 60f) % 5;
		var intervalPosition = (Time.time / (60f * 5f)) % 1f;

		var multiplierA = 0f;
		var multiplierB = 0f;
		
		//all A for first interval
		if (intervalIndex == 0) {
			multiplierA = 1f;
			multiplierB = 0f;
		} else {
			//B fades
			if (intervalIndex % 2 == 0) {
				multiplierA = Mathf.Lerp(0f, 1f, Mathf.Clamp01(intervalPosition * 5f));
				multiplierB = Mathf.Lerp(1f, 0f, Mathf.Clamp01(intervalPosition * 5f));

				if (intervalPosition > 0.2f && intervalPosition < 0.21f)
					MaxAverageLevelB = 0f;
				

				//A fades
			} else {
				multiplierA = Mathf.Lerp(1f, 0f, Mathf.Clamp01(intervalPosition * 5f));
				multiplierB = Mathf.Lerp(0f, 1f, Mathf.Clamp01(intervalPosition * 5f));

				if (intervalPosition > 0.2f && intervalPosition < 0.21f)
					MaxAverageLevelA = 0f;
				
			}
		}

		return Mathf.Max(0.00001f, multiplierA * MaxAverageLevelA + multiplierB * MaxAverageLevelB);
	}
	
	private float GetMaxRmsRaw()
	{
		var intervalIndex = Mathf.FloorToInt(Time.time / 60f) % 5;
		var intervalPosition = (Time.time / (60f * 5f)) % 1f;

		var multiplierA = 0f;
		var multiplierB = 0f;
		
		//all A for first interval
		if (intervalIndex == 0) {
			multiplierA = 1f;
			multiplierB = 0f;
		} else {
			//B fades
			if (intervalIndex % 2 == 0) {
				multiplierA = Mathf.Lerp(0f, 1f, Mathf.Clamp01(intervalPosition * 5f));
				multiplierB = Mathf.Lerp(1f, 0f, Mathf.Clamp01(intervalPosition * 5f));

				if (intervalPosition > 0.2f && intervalPosition < 0.21f) 
					MaxRmsRawB = 0f;
				


				//A fades
			} else {
				multiplierA = Mathf.Lerp(1f, 0f, Mathf.Clamp01(intervalPosition * 5f));
				multiplierB = Mathf.Lerp(0f, 1f, Mathf.Clamp01(intervalPosition * 5f));

				if (intervalPosition > 0.2f && intervalPosition < 0.21f) 
					MaxRmsRawA = 0f;
				

			}
		}

		return Mathf.Max(0.0001f, multiplierA * MaxRmsRawA + multiplierB * MaxRmsRawB);
	}
}
