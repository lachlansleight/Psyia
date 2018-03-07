using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridExample : MonoBehaviour {

	public Vector3 GridMinPos = new Vector3(-10f, 0f, -10f);
	public Vector3 GridMaxPos = new Vector3(10f, 10f, 10f);
	public Vector3 GridCount = new Vector3(5f, 5f, 5f);

	Vector3[] Grid;

	public Transform Target;

	// Use this for initialization
	void Start () {
		Grid = new Vector3[(int)(GridCount.x * GridCount.y * GridCount.z)];

		for(int i = 0; i < GridCount.x; i++) {
			for(int j = 0; j < GridCount.y; j++) {
				for(int k = 0; k < GridCount.z; k++) {
					Grid[IndexFromSubindices(i, j, k)] = PositionFromIndex(i, j, k);

					GameObject NewObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
					NewObj.transform.localScale = Vector3.one * 0.2f;
					NewObj.transform.position = Grid[IndexFromSubindices(i, j, k)];
				}
			}
		}
	}

	int IndexFromSubindices(int x, int y, int z) {
		return Mathf.FloorToInt(x * GridCount.y * GridCount.z) + Mathf.FloorToInt(y * GridCount.z) + z;
	}

	Vector3 PositionFromIndex(int i, int j, int k) {
		return new Vector3(
			(((float)i / GridCount.x) - 0.5f) * (GridMaxPos.x - GridMinPos.x),
			(((float)j / GridCount.y) - 0f  ) * (GridMaxPos.y - GridMinPos.y),
			(((float)k / GridCount.z) - 0.5f) * (GridMaxPos.z - GridMinPos.z)
		);
	}

	int IndexFromPosition(Vector3 Position) {

		Vector3Int IndexVector = new Vector3Int(
			Mathf.FloorToInt(Mathf.Lerp(0, GridCount.x - 1, Mathf.Clamp01(Mathf.InverseLerp(GridMinPos.x, GridMaxPos.x, Position.x)))),
			Mathf.FloorToInt(Mathf.Lerp(0, GridCount.y - 1, Mathf.Clamp01(Mathf.InverseLerp(GridMinPos.y, GridMaxPos.y, Position.y)))),
			Mathf.FloorToInt(Mathf.Lerp(0, GridCount.z - 1, Mathf.Clamp01(Mathf.InverseLerp(GridMinPos.z, GridMaxPos.z, Position.z))))
		);

		return (int)(IndexVector.x * GridCount.y * GridCount.z + IndexVector.y * GridCount.z + IndexVector.z);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine(Target.position, Grid[IndexFromPosition(Target.position)], Color.green);
	}
}
