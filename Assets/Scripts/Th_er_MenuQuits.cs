using UnityEngine;
using System.Collections;

public class Th_er_MenuQuits : MonoBehaviour {
	
	void Update () {
		if(VRTools.VRInput.GetDevice("ViveLeft").GetButtonDown("Menu") || VRTools.VRInput.GetDevice("ViveRight").GetButtonDown("Menu")) Application.Quit();
	}
}
