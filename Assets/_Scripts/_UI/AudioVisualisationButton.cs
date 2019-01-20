using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XRP;

public class AudioVisualisationButton : MonoBehaviour
{

	public SliderAudioHook AudioHook;

	public Sprite[] Icons;
	private Image _image;
	private XrpButton _xrpButton;

	public UnityIntEvent OnValueChanged;
	
	public void Awake()
	{
		_image = GetComponentInChildren<Image>();
		_xrpButton = GetComponent<XrpButton>();
	}

	public void OnEnable()
	{
		_xrpButton.OnClickEvent.AddListener(HandleClick);
	}

	public void OnDisable()
	{
		_xrpButton.OnClickEvent.RemoveListener(HandleClick);
	}

	public void Update()
	{
		_image.sprite = Icons[(int) AudioHook.DataSource];
	}

	private void HandleClick()
	{
		var value = (int) AudioHook.DataSource;
		value++;
		if (value > 2) value = 0;
		
		OnValueChanged.Invoke(value);
	}

	public void SetSource(int value)
	{
		var preValue = (int) AudioHook.DataSource;
		if (value < 0) value = 0;
		if (value > 2) value = 2;

		if (preValue != value) OnValueChanged.Invoke(value);
	}
	
	
}
