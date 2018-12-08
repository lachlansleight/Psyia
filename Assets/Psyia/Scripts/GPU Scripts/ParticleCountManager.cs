using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCTK;

public class ParticleCountManager : MonoBehaviour {

	//this is multiplied by 1024
	[Range(1, 1024)] public int ParticleCountFactor = 512;

	public BufferSetup ParticleBuffer;
	public BufferSetup DistanceBuffer;
	public BufferSetupWithDispatch DeadList;

	public void ApplyParticleCount() {
		ParticleBuffer.Setup(ParticleCountFactor * 1024);
		DistanceBuffer.Setup(ParticleCountFactor * 1024);
		DeadList.ThreadGroups.x = ParticleCountFactor;
		DeadList.Setup(ParticleCountFactor * 1024);
	}
}
