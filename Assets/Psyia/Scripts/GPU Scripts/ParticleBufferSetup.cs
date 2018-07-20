using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCTK;

public class ParticleBufferSetup : BufferSetup {

	protected override void CreateData() {
		ParticleData[] Data = new ParticleData[Count];
		for(int i = 0; i < Data.Length; i++) {
			Data[i].Position = Random.insideUnitSphere * 0.1f + new Vector3(0, 1, 0);
			Data[i].Velocity = Vector3.zero;
			Data[i].Color = Color.Lerp(Color.red, Color.blue, Random.Range(0f, 1f));
			Data[i].Color.w = 1f;
			Data[i].IsAlive = 0;
			Data[i].Size = 1f;
			Data[i].Padding = Vector3.zero;
		}

		MyGpuBuffer.SetData(Data);
	}
}