using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRP
{

	public class XrpControlGeometry : MonoBehaviour
	{

		protected Transform _main;
		private Transform _right;
		private Transform _left;
		private Transform _top;
		private Transform _bottom;
		private Transform _pointerIndicator;

		public virtual void Awake()
		{
			FixGeometry();
		}

		protected virtual void FetchGeometry()
		{
			_main = transform.Find("Main");
			_right = transform.Find("Right");
			_left = transform.Find("Left");
			_top = transform.Find("Top");
			_bottom = transform.Find("Bottom");
		}

		[ContextMenu("FixGeometry")]
		public virtual void FixGeometry()
		{
			if (_main == null) FetchGeometry();

			var parentSize = transform.parent.localScale;

			//find shortest side - that side x 0.05 = width of border geometry and width of padding
			var shortest = Mathf.Min(parentSize.x, parentSize.y);
			var sizeX = 0.05f * shortest / parentSize.x;
			var sizeY = 0.05f * shortest / parentSize.y;
			var sizeZ = 0.05f * shortest / parentSize.z;

			_right.transform.localScale = _left.transform.localScale = new Vector3(sizeX, 1f, sizeZ);
			_top.transform.localScale = _bottom.transform.localScale = new Vector3(1f - 2f * sizeX, sizeY, sizeZ);

			_right.transform.localPosition = new Vector3(0.5f - (sizeX * 0.5f), 0f, 0f);
			_left.transform.localPosition = new Vector3(-0.5f + (sizeX * 0.5f), 0f, 0f);
			_top.transform.localPosition = new Vector3(0f, 0.5f - (sizeY * 0.5f), 0f);
			_bottom.transform.localPosition = new Vector3(0f, -0.5f + (sizeY * 0.5f), 0f);

			_main.transform.localScale = new Vector3(1f - 4f * sizeX, 1f - 4f * sizeY, 2f * sizeZ);
		}
	}

}