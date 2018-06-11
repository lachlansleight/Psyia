using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsyiaRTA : MonoBehaviour {

	public enum MetricTimespan {
		OneFrame,
		FiveFrames,
		HalfSecond,
		SeveralSeconds,
		FifteenSeconds,
		NinetySeconds,
		TenMinutes,
		SixtyMinutes
	}

	public static System.Type[] AllowedTypes {
		get {
			return new System.Type[] { typeof(int), typeof(float), typeof(Vector2), typeof(Vector3) };
		}
	}

	private static PsyiaRTA _Instance;
	public static PsyiaRTA Instance {
		get {
			if(_Instance == null) {
				_Instance = GameObject.FindObjectOfType<PsyiaRTA>();
			}
			return _Instance;
		}
	}

	public PsyiaRTA_State ActiveState;
	public PsyiaRTA_State[] States;
	
	[Range(0, 3f)] public float TriggerDiscrepancyThreshold = 1f;

	void Update() {
		if(ActiveState == null) {
			ActiveState = States[0];
			ActiveState.OnStateActivate.Invoke();
		}

		float MaxDiscrepancy = 0f;
		int MaxIndex = -1;
		for(int i = 0; i < States.Length; i++) {
			if(States[i] == ActiveState) continue;

			float TriggerDiscrepancy = States[i].Key.TriggerValue - ActiveState.Key.TriggerValue;
			if(TriggerDiscrepancy> TriggerDiscrepancyThreshold && TriggerDiscrepancy > MaxDiscrepancy) {
				MaxDiscrepancy = TriggerDiscrepancy;
				MaxIndex = i;
			}
		}

		if(MaxIndex != -1) {
			ActiveState.OnStateDeactivate.Invoke();
			ActiveState = States[MaxIndex];
			ActiveState.OnStateActivate.Invoke();
		}


		ActiveState.OnLerpValueChanged.Invoke(ActiveState.Key.LerpValue);
	}
}
