using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class XrpCanvasAlphaCopier : MonoBehaviour
{

	public Renderer Target;
	private CanvasGroup _canvasGroup;

	public void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		if (Target == null) Target = transform.parent.GetComponent<Renderer>();
	}
	
	public void Update ()
	{
		_canvasGroup.alpha = Target.material.color.a;
	}
}
