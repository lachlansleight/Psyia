using System.Collections;
using UnityEngine;

namespace XRP
{
	[RequireComponent(typeof(Collider))]
	public class XrpControl : MonoBehaviour
	{
		

		private State _currentState;
		public State CurrentState
		{
			get { return _currentState; }
			set
			{
				if (value != _currentState) OnStateChange?.Invoke(value);

				_currentState = value;
			}
		}
		
		[HideInInspector] public XrpPointer ActivePointer;
		[HideInInspector] public float ClosestPointerDistance;
		[HideInInspector] public XrpPanel Panel;


		public delegate void StateChangeDelegate(State newState);
		public delegate void EmptyDelegate();
		public delegate void FloatDelegate(float value);
		public delegate void IntDelegate(int value);
		public delegate void BoolDelegate(bool value);

		public StateChangeDelegate OnStateChange;
		
		public bool ThrowEventOnStart = false;

		protected Transform FadePanel;
		private Material _fadePanelMat;
		private LineRenderer _line;
		private BoxCollider _boxCollider;
		private Renderer _debugRenderer;
		private Transform _pointerIndicator;
		private float _pointerIndicatorTargetSize = 0f;
		private float _pointerIndicatorCurrentSize = 0f;
		private Vector3 _pointerIndicatorScale;
		

		public virtual void Awake()
		{
			CurrentState = State.Inactive;
			_debugRenderer = transform.Find("Geometry/Main").GetComponent<Renderer>();
			FadePanel = transform.Find("ActiveGeometry/FadePanel");
			_fadePanelMat = FadePanel.GetComponent<Renderer>().material;
			_line = transform.Find("ActiveGeometry/Line").GetComponent<LineRenderer>();
			_pointerIndicator = transform.Find("ActiveGeometry/PointerIndicator");
			_boxCollider = GetComponent<BoxCollider>();
		}

		public virtual void Start() { }

		public virtual void Update()
		{
			if (_debugRenderer != null) ShowDebugColor();

			_pointerIndicatorCurrentSize = Mathf.Lerp(_pointerIndicatorCurrentSize, _pointerIndicatorTargetSize, 0.1f);
			_pointerIndicator.localScale = _pointerIndicatorCurrentSize * new Vector3(
				1f / transform.lossyScale.x,
				1f / transform.lossyScale.y,
				1f / transform.lossyScale.z
			);
			
			if (ActivePointer == null) return;
			
			if (CurrentState != State.Press && CurrentState != State.Disabled) CheckForPress();
			if (CurrentState == State.Press) {
				DoPress();
			}
		}
		
		public virtual void StartHover()
		{
			CurrentState = State.Hover;
			AudioSource.PlayClipAtPoint(Panel.HoverClip, transform.position, 0.1f);
		}

		public virtual void StopHover()
		{
			CurrentState = State.Inactive;
		}

		public virtual void StartTouch(XrpPointer pointer)
		{
			CurrentState = State.Touch;
			ActivePointer = pointer;
		}

		public virtual void StopTouch()
		{
			CurrentState = State.Hover;
			ActivePointer = null;
		}

		public virtual void StartPress()
		{
			CurrentState = State.Press;
			
			_line.positionCount = 2;
			_line.enabled = true;
			FadePanel.gameObject.SetActive(true);
			FadePanel.localPosition = Vector3.zero;
			_fadePanelMat.color = new Color(1f, 1f, 1f, 0f);
			_line.startColor = _line.endColor = _fadePanelMat.color;
			_pointerIndicatorTargetSize = Panel.PointerSize;
		}
		
		public virtual void StopPress()
		{
			CurrentState = State.Hover;
			ActivePointer = null;
			
			_line.positionCount = 0;
			_line.enabled = false;
			FadePanel.gameObject.SetActive(false);
			_pointerIndicatorTargetSize = 0f;
		}

		public virtual Vector3 GetDistance(Vector3 worldPoint)
		{
			var localPoint = transform.InverseTransformPoint(worldPoint);
			var displacement = localPoint;
			
			//we need the distance to the rectangular bounds, not the center
			var pointerDisplacement = new Vector3();
			if (displacement.x > _boxCollider.size.x * -0.5f && displacement.x < _boxCollider.size.x * 0.5f)
				pointerDisplacement.x = 0f;
			else if (displacement.x < _boxCollider.size.x * -0.5f)
				pointerDisplacement.x = displacement.x - _boxCollider.size.x * -0.5f;
			else if (displacement.x > transform.localScale.x * 0.5f)
				pointerDisplacement.x = displacement.x - _boxCollider.size.x * 0.5f;
			
			if (displacement.y > _boxCollider.size.y * -0.5f && displacement.y < _boxCollider.size.y * 0.5f)
				pointerDisplacement.y = 0f;
			else if (displacement.y < _boxCollider.size.y * -0.5f)
				pointerDisplacement.y = displacement.y - _boxCollider.size.y * -0.5f;
			else if (displacement.y > _boxCollider.size.y * 0.5f)
				pointerDisplacement.y = displacement.y - _boxCollider.size.y * 0.5f;
			
			if (displacement.z > _boxCollider.size.z * -0.5f && displacement.z < _boxCollider.size.z * 0.5f)
				pointerDisplacement.z = 0f;
			else if (displacement.z < _boxCollider.size.z * -0.5f)
				pointerDisplacement.z = displacement.z - _boxCollider.size.z * -0.5f;
			else if (displacement.z > _boxCollider.size.z * 0.5f)
				pointerDisplacement.z = displacement.z - _boxCollider.size.z * 0.5f;

			pointerDisplacement.x *= transform.lossyScale.x;
			pointerDisplacement.y *= transform.lossyScale.y;
			pointerDisplacement.z *= transform.lossyScale.z;
			
			return pointerDisplacement;
		}

		protected virtual void DoPress()
		{
			var pointerPos = ActivePointer.transform.position;
			var localPos = transform.InverseTransformPoint(pointerPos);
			var invertedLocalPos = new Vector3(localPos.x, localPos.y, -localPos.z) { z = 0f };

			_pointerIndicator.transform.localPosition = invertedLocalPos;

			if (localPos.z > Panel.PressThresholdDistance) {
				StopPress();
				return;
			}

			var fadePanelOpacityTemp = 0f;
			var lineOpacityTemp = 1f;

			var opacityLerpFactor = Mathf.InverseLerp(0f, -Panel.PressMaxDistance, localPos.z);
			var fadePanelOpacity = fadePanelOpacityTemp * Mathf.Lerp(0f, 1f, opacityLerpFactor);
			var lineOpacity = lineOpacityTemp * Mathf.Lerp(0f, 1f, opacityLerpFactor);
			
			FadePanel.localPosition = new Vector3(0f, 0f, -localPos.z);
			_fadePanelMat.color = new Color(1f, 1f, 1f, fadePanelOpacity);
			_line.startColor = _line.endColor = new Color(1f, 1f, 1f, lineOpacity);
			_line.positionCount = 2;

			/*
			localPos.x = Mathf.Clamp(localPos.x, -0.5f, 0.5f);
			localPos.y = Mathf.Clamp(localPos.y, -0.5f, 0.5f);
			invertedLocalPos.x = localPos.x;
			invertedLocalPos.y = localPos.y;
			*/
			var vertices = new[]
			{
				localPos,
				invertedLocalPos
			};
			_line.SetPositions(vertices);
		}

		protected void PopFadePanel()
		{
			var indicator = Instantiate(FadePanel.gameObject);
			indicator.transform.parent = FadePanel.parent;
			indicator.transform.localPosition = FadePanel.localPosition;
			indicator.transform.localRotation = FadePanel.localRotation;
			indicator.transform.localScale = FadePanel.localScale;
			indicator.AddComponent<OneShotter>().StartCoroutine(FadeIndicator(indicator, 0.4f));
		}
		
		private void CheckForPress()
		{
			var pointerPos = ActivePointer.transform.position;
			var localPos = transform.InverseTransformPoint(pointerPos);
			if (localPos.z < -Panel.PressThresholdDistance) {
				StartPress();
			}
		}

		[ContextMenu("FixGeometry")]
		public void FixGeometry()
		{
			GetComponentInChildren<XrpControlGeometry>().FixGeometry();
		}
		
		private void ShowDebugColor()
		{
			switch (CurrentState) {
				case State.Disabled:
					_debugRenderer.material.color = Color.Lerp(_debugRenderer.material.color, Color.red, 0.2f);
					break;
				case State.Inactive:
					_debugRenderer.material.color = Color.Lerp(_debugRenderer.material.color, Color.black, 0.2f);
					break;
				case State.Hover:
					var hoverColor = Color.gray;
					//i.e. we are within hover range
					if (ClosestPointerDistance > 0f) {
						hoverColor = Color.Lerp(
							Color.Lerp(Color.black, Color.gray, 0.2f),
							Color.Lerp(Color.gray, Color.white, 0.8f),
							Mathf.InverseLerp(Panel.HoverDistance, Panel.TouchDistance, ClosestPointerDistance)
						);
					}

					_debugRenderer.material.color = Color.Lerp(_debugRenderer.material.color, hoverColor, 0.2f);
					break;
				case State.Touch:
					_debugRenderer.material.color = Color.Lerp(_debugRenderer.material.color, Color.white, 0.2f);
					break;
				case State.Press:
					_debugRenderer.material.color = Color.Lerp(_debugRenderer.material.color, Color.cyan, 0.2f);
					break;
				default:
					Debug.LogError("Unexpected state unhandled");
					break;
			}
		}
		
		private IEnumerator FadeIndicator(GameObject indicator, float duration)
		{
			var indicatorMaterial = indicator.GetComponent<Renderer>().material;
			
			var startColor = indicatorMaterial.color;
			var endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

			var startScale = indicator.transform.localScale;
			var endScale = indicator.transform.localScale * 1.2f;

			for (var i = 0f; i < duration; i += Time.deltaTime) {
				var iL = (i - 1f) * (i - 1f) * (i - 1f) * (i - 1f) * (i - 1f) + 1f;
				indicator.transform.localScale = Vector3.Lerp(startScale, endScale, iL);
				indicatorMaterial.color = Color.Lerp(startColor, endColor, iL);
				yield return null;
			}

			Destroy(indicatorMaterial);
			Destroy(indicator);
		}
	}
}