using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XRP;

public class AudioVisualisationButton : MonoBehaviour
{

	public SliderAudioHook AudioHook;

	public Sprite[] Icons;
	private Image _image;
	private XrpButton _xrpButton;
	
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
		_image.sprite = Icons[(int) AudioHook.Source];
	}

	private void HandleClick()
	{
		var value = (int) AudioHook.Source;
		value++;
		if (value >= 3) value = 0;
		AudioHook.Source = (SliderAudioHook.AudioDataSource) value;
	}
}
