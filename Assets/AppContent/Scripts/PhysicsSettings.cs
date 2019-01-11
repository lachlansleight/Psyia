using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;

public class PhysicsSettings : MonoBehaviour
{

	public PhysicsManager PhysicsManager;

	public void SetMass(float value)
	{
		PhysicsManager.ParticleMinimumMass = value;
	}

	public void SetDamping(float value)
	{
		PhysicsManager.ParticleDrag = value;
	}

	public void SetFloorCollision(bool value)
	{
		PhysicsManager.FloorCollision = value;
	}
}
