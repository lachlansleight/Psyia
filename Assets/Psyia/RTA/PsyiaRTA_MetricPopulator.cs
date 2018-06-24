using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class PsyiaRTA_MetricPopulator : MonoBehaviour {

	public GameObject TargetObject;
	public Component TargetComponent;
	public bool UsingProperty;
	public FieldInfo TargetField;
	public PropertyInfo TargetProperty;
	public string FieldName;
	public string PropertyName;
	public bool IsVector;
	public int VectorIndex;

	public PsyiaRTA_Metric Metric;

	void Awake() {
		if(UsingProperty) TargetProperty = TargetComponent.GetType().GetProperty(PropertyName);
		else TargetField = TargetComponent.GetType().GetField(FieldName);
	}

	// Use this for initialization
	void Start () {
		if(Metric != null) {
			Metric.ResetLists();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if((UsingProperty && TargetProperty == null) || (!UsingProperty && TargetField == null) || Metric == null) {
			if((UsingProperty && TargetProperty == null)) Debug.Log("Not adding value!");
			return;
		}

		Metric.AddValue(UsingProperty ? GetPropertyValue() : GetFieldValue());
	}

	float GetPropertyValue() {
		if(TargetProperty.PropertyType == typeof(int) || TargetProperty.PropertyType == typeof(float)) {
			return (float)TargetProperty.GetValue(TargetComponent, null);
		} else if(TargetProperty.PropertyType == typeof(Vector2)) {
			VectorIndex = (int)Mathf.Clamp(VectorIndex, 0, 1);
			return ((Vector2)TargetProperty.GetValue(TargetComponent, null))[VectorIndex];
		} else if(TargetProperty.PropertyType == typeof(Vector3)) {
			VectorIndex = (int)Mathf.Clamp(VectorIndex, 0, 2);
			return ((Vector3)TargetProperty.GetValue(TargetComponent, null))[VectorIndex];
		} else if(TargetProperty.PropertyType == typeof(Vector4)) {
			VectorIndex = (int)Mathf.Clamp(VectorIndex, 0, 3);
			return ((Vector4)TargetProperty.GetValue(TargetComponent, null))[VectorIndex];
		}
		return 0f;
	}

	float GetFieldValue() {
		if(TargetField.FieldType == typeof(int) || TargetField.FieldType == typeof(float)) {
			return (float)TargetField.GetValue(TargetComponent);
		} else if(TargetField.FieldType == typeof(Vector2)) {
			VectorIndex = (int)Mathf.Clamp(VectorIndex, 0, 1);
			return ((Vector2)TargetField.GetValue(TargetComponent))[VectorIndex];
		} else if(TargetField.FieldType == typeof(Vector3)) {
			VectorIndex = (int)Mathf.Clamp(VectorIndex, 0, 2);
			return ((Vector3)TargetField.GetValue(TargetComponent))[VectorIndex];
		} else if(TargetField.FieldType == typeof(Vector4)) {
			VectorIndex = (int)Mathf.Clamp(VectorIndex, 0, 3);
			return ((Vector4)TargetField.GetValue(TargetComponent))[VectorIndex];
		}
		return 0f;
	}
}
