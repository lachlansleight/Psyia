using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PsyiaRTA_MetricList {
		private PsyiaRTA.MetricTimespan _Timespan;
		public PsyiaRTA.MetricTimespan Timespan {
			get {
				return _Timespan;
			} private set {
				_Timespan = value;
			}
		}

		private List<float> _Values;
		private List<float> Values {
			get {
				if(_Values == null) _Values = new List<float>();
				return _Values;
			} set {
				_Values = value;
			}
		}
		private float Sum;

		private float _Average;
		public float Average {
			get {
				return _Average;
			} private set {
				_Average = value;
			}
		}

		private float _ValueCount;
		public float ValueCount {
			get {
				if(_ValueCount == 0) _ValueCount = GetValueCount();
				return _ValueCount;
			} private set {
				_ValueCount = value;
			}
		}

		private float LastAddTime;

		public PsyiaRTA_MetricList(PsyiaRTA.MetricTimespan timespan) {
			Timespan = timespan;
			Sum = 0;
			Average = 0;
			_ValueCount = GetValueCount();
			LastAddTime = 0;
		}

		public int GetValueCount() {
			switch(Timespan) {
				case PsyiaRTA.MetricTimespan.OneFrame: return 1;
				case PsyiaRTA.MetricTimespan.FiveFrames: return 5;
				case PsyiaRTA.MetricTimespan.HalfSecond: return 45;
				case PsyiaRTA.MetricTimespan.SeveralSeconds: return 270;
				case PsyiaRTA.MetricTimespan.FifteenSeconds: return 1350;
				case PsyiaRTA.MetricTimespan.NinetySeconds: return 8100;
				case PsyiaRTA.MetricTimespan.TenMinutes: return 54000;
				case PsyiaRTA.MetricTimespan.SixtyMinutes: return 324000;
			}
			return 1;
		}

		public void AddValue(float Value) {
			//make sure we add enough frames (in case of frame loss)
			int FramesSinceLast = 1;
			if(LastAddTime == 0) {
				LastAddTime = Time.time;
			} else {
				float TimeSince = Time.time - LastAddTime;
				FramesSinceLast = Mathf.RoundToInt(TimeSince * 90f);
			}

			//add value, ensure we have the right number
			for(int i = 0; i < FramesSinceLast; i++) {
				Values.Add(Value);
				Sum += Value;
				if(Values.Count > ValueCount) {
					Sum -= Values[0];
					Values.RemoveAt(0);
				}
			}

			//recalculate average
			Average = Sum / (float)Values.Count;
			LastAddTime = Time.time;
		}

		public void Reset() {
			Values = new List<float>();
			Sum = 0f;
			Average = 0f;
			_ValueCount = GetValueCount();
			LastAddTime = 0f;
		}
	}