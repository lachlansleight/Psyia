using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;

public class ControllerSymmetry : MonoBehaviour
{
	public PsyiaForce SourceForce;
	public PsyiaEmitter SourceEmitter;
	
	public PsyiaForce TargetForce;
	public PsyiaEmitter TargetEmitter;

	public void Update()
	{
		TargetForce.Shape = SourceForce.Shape;
		TargetForce.AttenuationMode = SourceForce.AttenuationMode;
		TargetForce.AttenuationDistance = SourceForce.AttenuationDistance;
		TargetForce.Strength = SourceForce.Strength;

		TargetEmitter.Settings = SourceEmitter.Settings;
		TargetEmitter.EmitOverTime = SourceEmitter.EmitOverTime;
	}
}
