using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;

public class PhysicsSettings : MonoBehaviour
{

	public PsyiaSettingsApplicator SettingsApplicator;

	public void SetMass(float value)
	{
		SettingsApplicator.SetParticleMass(value);
	}

	public void SetDamping(float value)
	{
		SettingsApplicator.SetParticleDamping(value);
	}

	public void SetFloorCollision(bool value)
	{
		SettingsApplicator.SetFloorCollision(value);
	}
}
