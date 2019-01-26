using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPosition : MonoBehaviour
{

	public Transform target;
	
	public void Start()
	{
		
	}

	public void Update()
	{
		transform.position = target.position;
	}
}
