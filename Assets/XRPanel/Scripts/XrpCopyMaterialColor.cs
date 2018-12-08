using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRP
{

	[RequireComponent(typeof(Renderer))]
	public class XrpCopyMaterialColor : MonoBehaviour
	{

		public Renderer Target;
		private Renderer _renderer;

		public void Awake()
		{
			_renderer = GetComponent<Renderer>();
			if (Target == null) Target = transform.parent.GetComponent<Renderer>();
		}

		public void Update()
		{
			_renderer.material.color = Target.material.color;
		}
	}

}