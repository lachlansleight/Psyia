using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class DistanceBufferSetup : BufferSetup {

	protected override void CreateData() {
		DistanceStruct[] DistanceData = new DistanceStruct[Count];
		for(int i = 0; i < DistanceData.Length; i++) {

			DistanceData[i] = new DistanceStruct();
			DistanceData[i].Index = i;
			DistanceData[i].Distance = 0f;
		}

		MyGpuBuffer.SetData(DistanceData);
	}
}
