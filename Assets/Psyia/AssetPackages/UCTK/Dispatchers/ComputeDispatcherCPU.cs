using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCTK {

	public class ComputeDispatcherCPU : ComputeDispatcher {

		public int ThreadsX = 1;
		public int ThreadsY = 1;
		public int ThreadsZ = 1;

		public override void Dispatch() {
			for(int gX = 0; gX < ThreadGroupsX; gX++) {
				for(int gY = 0; gY < ThreadGroupsY; gY++) {
					for(int gZ = 0; gZ < ThreadGroupsZ; gZ++) {
						for(int tX = 0; tX < ThreadsX; tX++) {
							for(int tY = 0; tY < ThreadsY; tY++) {
								for(int tZ = 0; tZ < ThreadsZ; tZ++) {
									KernelSimulation(new Vector3Int(tX, tY, tZ), new Vector3Int(gX, gY, gZ));
								}
							}
						}
					}
				}
			}
		}

		public override void Dispatch(int OverrideX, int OverrideY, int OverrideZ) {
			for(int gX = 0; gX < OverrideX; gX++) {
				for(int gY = 0; gY < OverrideY; gY++) {
					for(int gZ = 0; gZ < OverrideZ; gZ++) {
						for(int tX = 0; tX < ThreadsX; tX++) {
							for(int tY = 0; tY < ThreadsY; tY++) {
								for(int tZ = 0; tZ < ThreadsZ; tZ++) {
									KernelSimulation(new Vector3Int(tX, tY, tZ), new Vector3Int(gX, gY, gZ));
								}
							}
						}
					}
				}
			}
		}

		protected virtual void KernelSimulation(Vector3Int tid, Vector3Int gid) {

		}
	}

}