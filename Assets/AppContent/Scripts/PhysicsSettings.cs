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
	public void SetMassVis(int value)
	{
		SettingsApplicator.SetParticleMassVis(value);
	}

	public void SetDamping(float value)
	{
		SettingsApplicator.SetParticleDamping(value);
	}
	public void SetDampingVis(int value)
	{
		SettingsApplicator.SetParticleDampingVis(value);
	}

	public void SetFloorCollision(bool value)
	{
		SettingsApplicator.SetFloorCollision(value);
	}

	public void SetTimeSpeed(float value)
	{
		SettingsApplicator.SetTimeSpeed(value);
	}
	public void SetTimeSpeedVis(int value)
	{
		SettingsApplicator.SetTimeSpeedVis(value);
	}
}
