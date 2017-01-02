using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct AudioSample {
	public float level;
	public float time;
}

public enum AudioBin {
	Now,
	OneSecond,
	TwoSeconds,
	FiveSeconds,
	TenSeconds,
	ThirtySeconds,
	OneMinute,
	TwoMinutes,
	FiveMinutes,
	TenMinutes
}

public class AudioBins : MonoBehaviour {

	public List<AudioSample> values;
	public AudioData audioInput;
	public float totalBufferTime = 600f;

	AudioBin[] BinNames;
	BufferAverager[] Averagers;
	public Dictionary<AudioBin, BufferAverager> Bins;

	public Transform testTransform;
	public Transform testTransformB;
	public Material testMaterial;

	// Use this for initialization
	void Start () {
		values = new List<AudioSample>();
		
		
		BinNames = new AudioBin[] {
			AudioBin.Now,
			AudioBin.OneSecond,
			AudioBin.TwoSeconds,
			AudioBin.FiveSeconds,
			AudioBin.TenSeconds,
			AudioBin.ThirtySeconds,
			AudioBin.OneMinute,
			AudioBin.TwoMinutes,
			AudioBin.FiveMinutes,
			AudioBin.TenMinutes
		};
		Averagers = new BufferAverager[BinNames.Length];
		Bins = new Dictionary<AudioBin, BufferAverager>();
		
		for(int i = 0; i < Averagers.Length; i++) {
			Averagers[i] = new BufferAverager(BinNames[i]);
			Bins.Add(BinNames[i], Averagers[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		AudioSample newSample = new global::AudioSample();
		newSample.time = Time.time;
		newSample.level = audioInput.avgVol;

		values.Insert(0, newSample);

		//remove any values that are older than the totalBufferTime (default 10 minutes)
		while(Time.time - values[values.Count - 1].time > totalBufferTime) {
			values.RemoveAt(values.Count - 1);
		}

		Bins[AudioBin.Now].average = newSample.level;

		for(int i = 1; i < BinNames.Length; i++) {
			Bins[BinNames[i]].Reset();
		}

		for(int i = 0; i < values.Count; i++) {
			for(int j = 1; j < BinNames.Length; j++) {

				if(Time.time - values[i].time < Bins[BinNames[j]].length) {
					Bins[BinNames[j]].Add(values[i].level);
				}

			}
		}
		Debug.Log(Averagers[1].sum);	
		
		for(int i = 1; i < BinNames.Length; i++) {
			Bins[BinNames[i]].GetAverage();
		}

		string tempstring = "BinType\t\tCount\t\tSum\t\tAverage";
		for(int i = 1; i < Averagers.Length; i++) {
			tempstring += "\n" + Averagers[i].binType.ToString() + "\t\t" + Averagers[i].count + "\t\t" + Averagers[i].sum + "\t\t" + Averagers[i].average;
		}
	}

	public static float GetBinLength(AudioBin type) {
		float length = 1f;
		switch(type) {
		case AudioBin.Now:
			length = (1f / 90f);
			break;
		case AudioBin.OneSecond:
			length = 1f;
			break;
		case AudioBin.TwoSeconds:
			length = 2f;
			break;
		case AudioBin.FiveSeconds:
			length = 5f;
			break;
		case AudioBin.TenSeconds:
			length = 10f;
			break;
		case AudioBin.ThirtySeconds:
			length = 30f;
			break;
		case AudioBin.OneMinute:
			length = 60f;
			break;
		case AudioBin.TwoMinutes:
			length = 120f;
			break;
		case AudioBin.FiveMinutes:
			length = 300f;
			break;
		case AudioBin.TenMinutes:
			length = 600f;
			break;
		}

		return length;
	}
}

[System.Serializable]
public class BufferAverager {
	public AudioBin binType;
	public float length;
	public int count;
	public float sum;
	public float average;

	public List<float> values;

	public BufferAverager(AudioBin inputBinType) {
		binType = inputBinType;
		length = AudioBins.GetBinLength(inputBinType);
	}

	public void Reset() {
		sum = 0;
		count = 0;
	}

	public void Add(float newValue) {
		count++;
		sum += newValue;
	}

	public void GetAverage() {
		if(count == 0) average = 0;
		else average = sum / (float)count;
	}
}
