using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class ParticleBufferSetup : BufferSetup {

	protected override void CreateData() {
		ComputeStruct[] Data = new ComputeStruct[Count];
		for(int i = 0; i < Data.Length; i++) {
			Data[i].pos = Random.insideUnitSphere * 0.1f + new Vector3(0, 1, 0);
			Data[i].velocity = Vector3.zero;
			Data[i].color = Color.Lerp(Color.red, Color.blue, Random.Range(0f, 1f));
			Data[i].color.a = 1f;
			Data[i].isAlive = 0;
		}

		MyGpuBuffer.SetData(Data);
	}
}
