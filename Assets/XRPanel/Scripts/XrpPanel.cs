using UnityEngine;

namespace XRP
{
	public class XrpPanel : MonoBehaviour
	{
		private XrpPointer[] _pointers;
		private XrpControl[] _controls;

		public float PointerSize = 0.015f;
		public float HoverDistance = 0.1f;
		public float TouchDistance = 0.02f;
		public float PressThresholdDistance = 0.01f;
		public float PressMaxDistance = 0.4f;

		public AudioClip HoverClip;
		public AudioClip PressClip;

		private float[] _hoverCandidates;
		private int[] _touchCandidates;

		public void Awake()
		{
			_controls = transform.GetComponentsInChildren<XrpControl>();
			foreach (var control in _controls) control.Panel = this;
			_hoverCandidates = new float[_controls.Length];
			_touchCandidates = new int[_controls.Length];
			for (var i = 0; i < _controls.Length; i++) _hoverCandidates[i] = _touchCandidates[i] = -1;
			
			_pointers = FindObjectsOfType<XrpPointer>();
		}

		public void Update()
		{
			
			for (var i = 0; i < _pointers.Length; i++) {
				for (var j = 0; j < _controls.Length; j++) {
					var displacement = _controls[j].GetDistance(_pointers[i].transform.position);

					if(_hoverCandidates[j] < 0) _hoverCandidates[j] = displacement.magnitude <= HoverDistance ? displacement.magnitude : -1f;
					else if (displacement.magnitude < _hoverCandidates[j]) _hoverCandidates[j] = displacement.magnitude;

					if(_touchCandidates[j] < 0) _touchCandidates[j] = displacement.magnitude <= TouchDistance ? i : -1;

				}
			}
			
			for (var i = 0; i < _controls.Length; i++) {
				var state = _controls[i].CurrentState;
				
				if (_touchCandidates[i] >= 0) {
					if (
						(state == State.Inactive || state == State.Hover) && 
						_controls[i].ActivePointer == null
					) _controls[i].StartTouch(_pointers[_touchCandidates[i]]);
				} else {
					if (state == State.Touch) _controls[i].StopTouch();
					
					if (_hoverCandidates[i] >= 0) {
						_controls[i].ClosestPointerDistance = _hoverCandidates[i];
						
						if (state == State.Inactive) _controls[i].StartHover();
					} else {
						if (state == State.Hover) _controls[i].StopHover();
					}
				}

				_touchCandidates[i] = -1;
				_hoverCandidates[i] = -1;
			}
		}
	}
}