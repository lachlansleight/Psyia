using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using XRP;

public class TouchSphere : MonoBehaviour
{
	public int UsageCountRequirement = 0;
	public Color Color;
	
	protected float DefaultScale;

	public virtual void Awake()
	{
		DefaultScale = transform.localScale.x;
	}
	
	public void LerpToScale(float targetScalePercentage, float duration, bool disableAtEnd = false)
	{
		//Debug.Log($"{gameObject.name} lerping to {(int)(targetScalePercentage*100f)} over {duration:0.00} seconds, disableAtEnd={disableAtEnd}");
		if (duration <= 0f) {
			SetScale(targetScalePercentage * DefaultScale);
			if (disableAtEnd) gameObject.SetActive(false);
			return;
		}
		
		StartCoroutine(LerpToScaleRoutine(targetScalePercentage, duration, disableAtEnd));
	}
	
	public IEnumerator LerpToScaleRoutine(float targetScalePercentage, float duration, bool disableAtEnd = false)
	{
		var from = transform.localScale.x;
		var to = targetScalePercentage * DefaultScale;
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / duration) {
			SetScale(Mathf.Lerp(from, to, LerpCubic(i)));
			yield return null;
		}

		SetScale(to);

		if (disableAtEnd) gameObject.SetActive(false);
	}

	protected virtual void SetScale(float value)
	{
		transform.localScale = Vector3.one * value;
	}
	
	private float LerpCubic(float t)
	{
		if (t < 0.5f) return 4f * t * t * t;
		t--;
		return 4f * t * t * t + 1f;
	}
}
