using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRP.Example
{

	public class Lights : MonoBehaviour
	{

		public Material OnMat;
		public Material OffMat;
		private Renderer[] _lights;

		public int OnCount { get; set; }

		public void Start()
		{
			_lights = new Renderer[transform.childCount];
			for (var i = 0; i < transform.childCount; i++) {
				_lights[i] = transform.GetChild(i).GetComponent<Renderer>();
				_lights[i].sharedMaterial = OffMat;
			}
		}

		public void Update()
		{
			for (var i = 0; i < transform.childCount; i++) {
				_lights[i].sharedMaterial = i < OnCount ? OnMat : OffMat;
			}
		}
	}

}