using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Psyia {

	public class UpdateGlobalMatrix : MonoBehaviour {

		public Transform SystemParent;

		public GlobalManager Global;
		public RenderingManager Rendering;

		public ComputeShader SimulateShader;
		public ComputeShader DistancesShader;
		public ComputeShader EmitShader;
		
		void Update () {
			Matrix4x4 SystemMatrix = Global.SimulationSpace == Space.Self ? Matrix4x4.TRS(SystemParent.position, SystemParent.rotation, SystemParent.localScale) : Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), Vector3.one);
			Matrix4x4 InverseSystemMatrix = SystemMatrix.inverse;

			SimulateShader.SetMatrix("SystemMatrix", SystemMatrix);
			DistancesShader.SetMatrix("SystemMatrix", SystemMatrix);
			EmitShader.SetMatrix("SystemMatrix", SystemMatrix);

			SimulateShader.SetMatrix("InverseSystemMatrix", InverseSystemMatrix);
			DistancesShader.SetMatrix("InverseSystemMatrix", InverseSystemMatrix);
			EmitShader.SetMatrix("InverseSystemMatrix", InverseSystemMatrix);

			Rendering.ParticleMaterial.SetMatrix("SystemMatrix", SystemMatrix);
			Rendering.ParticleMaterial.SetMatrix("InverseSystemMatrix", InverseSystemMatrix);
		}
	}

}