using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRP.Example
{

	public class TestSpawner : MonoBehaviour
	{

		public Color[] Colors;

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		public void SpawnCube(int colorIndex)
		{
			var outputColor = Colors[Mathf.FloorToInt(Mathf.Repeat(colorIndex, Colors.Length))];
			var newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			newObj.transform.rotation = Random.rotation;
			newObj.transform.position = transform.position;
			newObj.transform.localScale = new Vector3(
				                              Random.Range(0.5f, 1.5f),
				                              Random.Range(0.5f, 1.5f),
				                              Random.Range(0.5f, 1.5f)
			                              ) * 0.2f;
			newObj.AddComponent<Rigidbody>();
			newObj.GetComponent<Renderer>().material.color = outputColor;
		}
	}

}