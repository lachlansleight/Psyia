using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class StartSpawner : MonoBehaviour {

	public bool DoSpawn = true;
	public int SpawnCount;

	public Vector3 SpawnCenterPosition;
	public float SpawnRadius;

	public GpuAppendBuffer DeadList;
	public ComputeDispatcher SpawnDispatcher;

	void Start () {
		StartCoroutine(SpawnAfterFrame());
	}

	public void Spawn() {
		if(!DoSpawn) return;

		if(SpawnCount < 0) {
			SpawnCount = ((DeadList.CurrentCount) / 1024) + SpawnCount;
		}

		if(SpawnCount < 1) return;

		SpawnDispatcher.Shader.SetFloat("SpawnRadius", SpawnRadius);
		SpawnDispatcher.Shader.SetFloat("SpawnVelocityScatter", 0.001f);
		SpawnDispatcher.Shader.SetFloat("SpawnInheritVelocity", 0f);
		SpawnDispatcher.Shader.SetVector("SpawnPosition", SpawnCenterPosition);
		SpawnDispatcher.Shader.SetVector("LastSpawnPosition", SpawnCenterPosition);

		if(DeadList.CurrentCount < SpawnCount) {
			SpawnCount = DeadList.CurrentCount;
		}

		SpawnDispatcher.Dispatch(1024, SpawnCount, 1);
	}
	
	IEnumerator SpawnAfterFrame() {
		yield return null;
		Spawn();
	}
}
