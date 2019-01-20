using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPages : MonoBehaviour
{

	public int StartIndex = 0;
	public GameObject[] Pages;

	public void GoToPage(int index)
	{
		if (index < 0) index = 0;
		if (index >= Pages.Length) index = Pages.Length - 1;

		for (var i = 0; i < Pages.Length; i++) {
			Pages[i].SetActive(i == index);
		}
	}
}
