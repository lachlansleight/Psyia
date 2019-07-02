using UnityEngine;
using System.Collections;
using XRP;

public class ModeSettingsPanel : MonoBehaviour
{

	public string PlayerPrefsPrefix;
	public XrpToggle InfiniteModeToggle;

	public void Awake()
	{
		
	}

	public void OnEnable()
	{
		if (PlayerPrefs.HasKey(PlayerPrefsPrefix + "_InfiniteMode") &&
		    PlayerPrefs.GetInt(PlayerPrefsPrefix + "_InfiniteMode") == 1) {
			InfiniteModeToggle.CurrentValue = true;
			InfiniteModeToggle.Bang();
		}
	}

	public void SetInfiniteMode(bool value)
	{
		PlayerPrefs.SetInt(PlayerPrefsPrefix + "_InfiniteMode", value ? 1 : 0);
	}

	public void SetUsePcAudio(bool value)
	{
		PlayerPrefs.SetInt(PlayerPrefsPrefix + "_UsePcAudio", value ? 1 : 0);
	}
	
	public void LerpToScale(float targetScalePercentage, float duration, bool disableAtEnd = false)
	{
		//Debug.Log(gameObject.name + " lerping to " + targetScalePercentage + ", enabled is " + gameObject.activeSelf + ", disableAtEnd is " + disableAtEnd);
		if (!gameObject.activeSelf && targetScalePercentage > 0f) gameObject.SetActive(true);
		StartCoroutine(LerpToScaleRoutine(targetScalePercentage, duration, disableAtEnd));
	}

	public IEnumerator LerpToScaleRoutine(float targetScalePercentage, float duration, bool disableAtEnd = false)
	{
		var from = transform.localScale.x;
		var to = targetScalePercentage * 1f;
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / duration) {
			transform.localScale = Vector3.one * Mathf.Lerp(from, to, LerpCubic(i));
			yield return null;
		}

		transform.localScale = Vector3.one * to;

		if (disableAtEnd) gameObject.SetActive(false);
	}
	
	private float LerpCubic(float t)
	{
		if (t < 0.5f) return 4f * t * t * t;
		t--;
		return 4f * t * t * t + 1f;
	}
}