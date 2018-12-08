using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRP
{

	public class XrpPanelGeometry : MonoBehaviour
	{

		public Vector2 PanelSize;
		public float PanelDepth;
		
		private Transform _front;
		private Transform _back;
		private Transform _topRight;
		private Transform _topLeft;
		private Transform _bottomRight;
		private Transform _bottomLeft;
		private RectTransform _canvas;

		private void FetchGeometry()
		{
			_front = transform.Find("Geometry/FrontGlass");
			_back = transform.Find("Geometry/BackPanel");
			_topRight = transform.Find("Geometry/TopRight");
			_topLeft = transform.Find("Geometry/TopLeft");
			_bottomRight = transform.Find("Geometry/BottomRight");
			_bottomLeft = transform.Find("Geometry/BottomLeft");
			_canvas = transform.Find("Canvas").GetComponent<RectTransform>();
		}

		public void SetPanelSize(Vector2 size)
		{
			PanelSize = size;
			FixGeometry();
		}
		
		public void SetPanelDepth(float depth)
		{
			PanelDepth = depth;
			FixGeometry();
		}

		public void SetPanelDimensions(Vector2 size, float depth)
		{
			PanelSize = size;
			PanelDepth = depth;
			FixGeometry();
		}

		[ContextMenu("FixGeometry")]
		public void FixGeometry()
		{
			if (_front == null) FetchGeometry();

			_front.localScale = new Vector3(PanelSize.x, PanelSize.y, 0.005f);
			_back.localScale = new Vector3(PanelSize.x, PanelSize.y, 0.01f);

			_front.localPosition = new Vector3(0f, 0f, -0.0025f);
			_back.localPosition = new Vector3(0f, 0f, -PanelDepth + 0.005f);
			
			_canvas.offsetMin = 0.5f * new Vector2(-PanelSize.x / _canvas.localScale.x, -PanelSize.y / _canvas.localScale.y);
			_canvas.offsetMax = 0.5f * new Vector2(PanelSize.x / _canvas.localScale.x, PanelSize.y / _canvas.localScale.y);
			_canvas.localPosition = Vector3.zero;

			_topRight.localPosition = new Vector3(
				-PanelSize.x * 0.5f + 0.005f,
				PanelSize.y * 0.5f - 0.005f,
				((PanelDepth + 0.005f - 0.01f) / -2f) 
			);
			_topLeft.localPosition = new Vector3(
				PanelSize.x * 0.5f - 0.005f,
				PanelSize.y * 0.5f - 0.005f,
				((PanelDepth + 0.005f - 0.01f) / -2f) 
			);
			_bottomRight.localPosition = new Vector3(
				-PanelSize.x * 0.5f + 0.005f,
				-PanelSize.y * 0.5f + 0.005f,
				((PanelDepth + 0.005f - 0.01f) / -2f) 
			);
			_bottomLeft.localPosition = new Vector3(
				PanelSize.x * 0.5f - 0.005f,
				-PanelSize.y * 0.5f + 0.005f,
				((PanelDepth + 0.005f - 0.01f) / -2f) 
			);

			_topRight.localScale = _topLeft.localScale = _bottomRight.localScale = _bottomLeft.localScale = new Vector3(
				0.01f,
				PanelDepth - 0.005f - 0.01f,
				0.01f
			);
		}
		
	}

}