using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class TestFieldParticlesCPU : ComputeDispatcherCPU {
	
	public GpuBuffer OutputBuffer;
	public GpuBuffer ForceField;

	float DeltaTime;
	float Lifespan;
	float Damping;
	float ParticleMass;
	Vector3 FieldEndPos;
	Vector3 FieldStartPos;
	Vector3 FieldCount;


	public ExampleShaderValues Values;

	ComputeStruct[] outputBuffer;
	FieldStruct[] forceField;

	public int TargetIndex = 0;

	private void Update() {
		DeltaTime = Values.DeltaTime;
		Lifespan = Values.Lifespan;
		Damping = Values.Damping;
		ParticleMass = Values.ParticleMass;
		FieldEndPos = Values.FieldEndPos;
		FieldStartPos = Values.FieldStartPos;
		FieldCount = Values.FieldCount;
	}

	public override void Dispatch() {
		outputBuffer = OutputBuffer.GetData<ComputeStruct>();
		forceField = ForceField.GetData<FieldStruct>();

		if(VRTK_Devices.GripPressed(VRDevice.Left)) {
			TargetIndex++;
		} else if(VRTK_Devices.GripPressed(VRDevice.Right)) {
			TargetIndex--;
		}
		TargetIndex = Mathf.Clamp(TargetIndex, 0, forceField.Length);
		VRTools.VRDebug.DrawLine(new Vector3(0f, 1f, 0f), forceField[TargetIndex].pos, Color.green, true, 0.001f);

		base.Dispatch();

		OutputBuffer.SetData(outputBuffer);
	}

	public override void Dispatch(int OverrideX, int OverrideY, int OverrideZ) {
		outputBuffer = OutputBuffer.GetData<ComputeStruct>();
		forceField = ForceField.GetData<FieldStruct>();

		base.Dispatch(OverrideX, OverrideY, OverrideZ);

		OutputBuffer.SetData(outputBuffer);
	}

	protected override void KernelSimulation(Vector3Int tid, Vector3Int gid) {

		//First we calculate our position in the buffer, and pull the relevant particle out for computation
		int id = tid.x + base.ThreadGroupsX * gid.x;
		ComputeStruct ThisParticle;
		try {
			ThisParticle = outputBuffer[id];
		} catch(System.IndexOutOfRangeException e) {
			//this is fine, the GPU doesn't care
			return;
		}

		if (ThisParticle.isAlive == 0) {
			ThisParticle.pos = new Vector3(0, -10000, 0);
			//ThisParticle.pos = float3(((float)id / 1024.) * 2.5, 2, -0.1);
			outputBuffer[id] = ThisParticle;
			//distanceBuffer[id].index = id;
			//distanceBuffer[id].distance = 10000;
			return;
		}

		ThisParticle.age += DeltaTime;

		if (ThisParticle.age > Lifespan) {
			ThisParticle.isAlive = 0;
			outputBuffer[id] = ThisParticle;
			//return;
		}

		Vector3 FieldIndexVector = ThisParticle.pos;
		FieldIndexVector = new Vector3(
			(FieldIndexVector.x - FieldStartPos.x) / (FieldEndPos.x - FieldStartPos.x),
			(FieldIndexVector.y - FieldStartPos.y) / (FieldEndPos.y - FieldStartPos.y),
			(FieldIndexVector.z - FieldStartPos.z) / (FieldEndPos.z - FieldStartPos.z)
		);
		FieldIndexVector = new Vector3(
			FieldIndexVector.x * (FieldCount.x - 0),
			FieldIndexVector.y * (FieldCount.y - 0),
			FieldIndexVector.z * (FieldCount.z - 0)
		);
	 
		int FinalIndex = (int)(Mathf.Floor(FieldIndexVector.x * FieldCount.y * FieldCount.z) + Mathf.Floor(FieldIndexVector.y * FieldCount.z) + Mathf.Floor(FieldIndexVector.z));

		VRTools.VRDebug.DrawLine(ThisParticle.pos, forceField[FinalIndex].pos, ThisParticle.color, true, 0.001f);

		Vector3 FieldForce = forceField[FinalIndex].instantForce + forceField[FinalIndex].attenuatingForce;

		//And we get a damping force
		Vector3 DampingForce = -1.0f * ThisParticle.velocity * Damping;

		//Now we apply the force as acceleration (using newton's second law)
		ThisParticle.velocity += (FieldForce / ParticleMass);
		//ThisParticle.velocity += (DampingForce / ParticleMass);

		Vector3 NewPos = ThisParticle.pos + ThisParticle.velocity;

		//apply walls
		if (NewPos.x < FieldStartPos.x || NewPos.x > FieldEndPos.x) {
			ThisParticle.velocity.x *= -1;
		}
		if (NewPos.y < FieldStartPos.y || NewPos.y > FieldEndPos.y) {
			ThisParticle.velocity.y *= -1;
		}
		if (NewPos.z < FieldStartPos.z || NewPos.z > FieldEndPos.z) {
			ThisParticle.velocity.z *= -1;
		}

		//And we update position
		ThisParticle.pos += ThisParticle.velocity;

		//distanceBuffer[id].index = id;
		//distanceBuffer[id].distance = length(ThisParticle.pos - Headset.xyz);

		//And finally, push the updated particle to the buffer
		outputBuffer[id] = ThisParticle;
	}
}
