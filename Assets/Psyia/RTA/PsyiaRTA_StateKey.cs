using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PsyiaRTA_StateKey.asset", menuName = "ScriptableObjects/PsyiaRTA_StateKey", order = 1)]
public class PsyiaRTA_StateKey : ScriptableObject {

	[System.Serializable]
	public class PsyiaRTA_MetricRange {
		public PsyiaRTA_Metric Metric;
		public PsyiaRTA.MetricTimespan Timespan;
		public float TriggerRangeMin;
		public float TriggerRangeMax;
		public float LerpRangeMin;
		public float LerpRangeMax;
		public float TriggerValue;
		public float LerpValue;

		public float GetTriggerValue() {
			float Average = Metric.GetListByTimespan(Timespan).Average;
			float ProximityRangeMin = TriggerRangeMin - Mathf.Abs(LerpRangeMin - TriggerRangeMin);
			float ProximityRangeMax = TriggerRangeMax + Mathf.Abs(LerpRangeMax - TriggerRangeMax);
			float MidPoint = Mathf.Lerp(LerpRangeMin, LerpRangeMax, 0.5f);
			if(Average < ProximityRangeMin) return 0f;
			else if(Average < TriggerRangeMin) return Mathf.InverseLerp(ProximityRangeMin, TriggerRangeMin, Average);
			else if(Average < MidPoint) return Mathf.InverseLerp(TriggerRangeMin, MidPoint, Average) + 1f;
			else if(Average < TriggerRangeMax) return 1f - Mathf.InverseLerp(MidPoint, TriggerRangeMax, Average) + 1f;
			else if(Average < ProximityRangeMax) return 1f - Mathf.InverseLerp(TriggerRangeMax, ProximityRangeMax, Average);
			else return 0f;
		}

		public float GetLerpValue() {
			float Average = Metric.GetListByTimespan(Timespan).Average;
			return Mathf.InverseLerp(LerpRangeMin, LerpRangeMax, Average);
		}
	}

	public PsyiaRTA_MetricRange[] Metrics;
	public float TriggerValue;
	public float LerpValue;

	public void CalculateValues() {
		float TriggerSum = 0f;
		float LerpSum = 0f;
		float Divisor = 0f;
		foreach(PsyiaRTA_MetricRange metric in Metrics) {
			TriggerSum += metric.GetTriggerValue();
			LerpSum += metric.GetLerpValue();
			Divisor += 1f;
		}
		TriggerValue = TriggerSum / Divisor;
		LerpValue = LerpSum / Divisor;
	}

	public void RemoveMetric(int index) {
		if(index < 0 || index >= Metrics.Length) {
			return;
		}
		PsyiaRTA_MetricRange[] NewMetrics = new PsyiaRTA_MetricRange[Metrics.Length - 1];
		int Offset = 0;
		for(int i = 0; i < Metrics.Length; i++) {
			if(i == index) Offset++;
			if(i+Offset >= NewMetrics.Length) {
				break;
			}
			NewMetrics[i+Offset] = Metrics[i];
		}
		Metrics = NewMetrics;
	}

	public void AddMetric() {
		PsyiaRTA_MetricRange[] NewMetrics = new PsyiaRTA_MetricRange[Metrics.Length + 1];
		for(int i = 0; i < Metrics.Length; i++) {
			NewMetrics[i] = Metrics[i];
		}
		NewMetrics[NewMetrics.Length - 1] = new PsyiaRTA_MetricRange();
		Metrics = NewMetrics;
	}
}
