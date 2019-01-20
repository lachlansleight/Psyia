using System.Collections;
using System.Collections.Generic;
using UCTK;
using UnityEngine;

public class TestRendererNull : MonoBehaviour
{
	public ComputeRenderer Renderer;

	public string name;
	
	public void Start()
	{
		if (Renderer == null) name = "null!";
		else name = Renderer.gameObject.name;
	}

	public void Update()
	{
		if (Renderer == null) name = "null!";
		else name = Renderer.gameObject.name;
	}
}
