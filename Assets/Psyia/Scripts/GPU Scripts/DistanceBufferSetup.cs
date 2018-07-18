using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class DistanceBufferSetup : BufferSetup {

	protected override void CreateData() {
		DistanceData[] DistanceData = new DistanceData[Count];
		for(int i = 0; i < DistanceData.Length; i++) {

			DistanceData[i] = new DistanceData();
			DistanceData[i].Index = i;
			DistanceData[i].Distance = 0f;
		}

		MyGpuBuffer.SetData(DistanceData);
	}
}