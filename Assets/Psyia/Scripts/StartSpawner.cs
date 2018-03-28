using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class StartSpawner : MonoBehaviour {

	public int SpawnCount;

	public Vector3 SpawnCenterPosition;
	public float SpawnRadius;

	public GpuAppendBuffer DeadList;
	public ComputeDispatcher SpawnDispatcher;

	void Start () {
		StartCoroutine(SpawnAfterFrame());
	}

	public void Spawn() {
		if(SpawnCount < 0) {
			SpawnCount = DeadList.CurrentCount;
		}

		if(SpawnCount % 1024 != 0) {
			SpawnCount -= SpawnCount % 1024;
		}

		if(SpawnCount < 1024) {
			SpawnCount = 0;
			return;
		}

		SpawnDispatcher.Shader.SetFloat("SpawnRadius", SpawnRadius);
		SpawnDispatcher.Shader.SetFloat("SpawnVelocityScatter", 0.001f);
		SpawnDispatcher.Shader.SetFloat("SpawnInheritVelocity", 0f);
		SpawnDispatcher.Shader.SetVector("SpawnPosition", SpawnCenterPosition);
		SpawnDispatcher.Shader.SetVector("LastSpawnPosition", SpawnCenterPosition);

		if(DeadList.CurrentCount < SpawnCount) {
			SpawnCount = DeadList.CurrentCount;
		}

		SpawnDispatcher.Dispatch(1024, SpawnCount / 1024, 1);
	}
	
	IEnumerator SpawnAfterFrame() {
		yield return null;
		Spawn();
	}
}
