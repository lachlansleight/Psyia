using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicDisplay : MonoBehaviour
{

	public PsyiaMusic Music;
	public AudioData AudioData;
	public Text OutputText;
	
	public void Start()
	{
		
	}

	public void Update()
	{
		if (AudioData.AverageLevel > 0.2f && !Music.IsPlaying) {
			OutputText.text = "Now Playing:\nListening to device audio";
		} else {
			if (Music.IsPlaying) {
				OutputText.text = "Now Playing:\n" + Music.GetCurrentSongName();
			} else {
				OutputText.text = "Now Playing:\n" + Music.GetCurrentSongName() + " (paused)";
			}
		}
	}
}
