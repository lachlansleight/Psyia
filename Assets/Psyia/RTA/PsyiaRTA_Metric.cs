using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PsyiaRTA_Metric.asset", menuName = "ScriptableObjects/PsyiaRTA_Metric", order = 1)]
public class PsyiaRTA_Metric : ScriptableObject {
	
    private PsyiaRTA_MetricList[] _Lists;
    public PsyiaRTA_MetricList[] Lists {
        get {
            if(_Lists == null || _Lists.Length == 0) {
                int Count = System.Enum.GetNames(typeof(PsyiaRTA.MetricTimespan)).Length;
                _Lists = new PsyiaRTA_MetricList[Count];
                for(int i = 0; i < Count; i++) {
                    _Lists[i] = new PsyiaRTA_MetricList((PsyiaRTA.MetricTimespan)i);
                }
            }
            return _Lists;
        } private set {
            _Lists = value;
        }
    }

    public void ResetLists() {
        foreach(PsyiaRTA_MetricList list in Lists) {
            list.Reset();
        }
    }

    public void AddValue(float NewValue) {
        Debug.Log("Adding value " + NewValue);
        foreach(PsyiaRTA_MetricList list in Lists) {
            list.AddValue(NewValue);
        }
    }

    public PsyiaRTA_MetricList GetListByTimespan(PsyiaRTA.MetricTimespan timespan) {
        if(Lists == null) {
            Debug.LogError("List array is null");
            return null;
        }
        for(int i = 0; i < Lists.Length; i++) {
            if(Lists[i].Timespan == timespan) return Lists[i];
        }

        Debug.LogError("Didn't find a list with timespan " + timespan);
        return null;
    }

}
