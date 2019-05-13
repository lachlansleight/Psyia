using UnityEngine;

namespace XRP
{
	public class XrpPointer : MonoBehaviour
	{

		public float HoverDistance = 0.1f;
		public float DefaultScale = 0.01f;
		public float HoverScale = 0.02f;

		public bool Hovering;

		[HideInInspector]
		public Vector3 LastPosition;

		public virtual void OnEnable()
		{
			var panels = FindObjectsOfType<XrpPanel>();
			foreach(var panel in panels) panel.AddPointer(this);
		}

		public virtual void OnDisable()
		{
			var panels = FindObjectsOfType<XrpPanel>();
			foreach(var panel in panels) panel.RemovePointer(this);
		}
		
		public virtual void Update()
		{
			transform.localScale =
				Vector3.one * Mathf.Lerp(transform.localScale.x, Hovering ? HoverScale : DefaultScale, 0.2f);
			
		}

		public virtual void LateUpdate()
		{
			LastPosition = transform.position;
		}

	}
}