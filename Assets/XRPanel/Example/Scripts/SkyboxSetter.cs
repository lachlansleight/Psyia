using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRP.Example
{

	public class SkyboxSetter : MonoBehaviour
	{

		public Material[] Skyboxes;
		public int DefaultIndex;

		public void Awake()
		{
			RenderSettings.skybox = Skyboxes[DefaultIndex];
		}
		
		public void SetSkybox(int index)
		{
			if (index < 0) index = 0;
			if (index >= Skyboxes.Length) index = Skyboxes.Length - 1;

			RenderSettings.skybox = Skyboxes[index];
		}
	}

}