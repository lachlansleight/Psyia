using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PsyiaSettingsApplicator))]
public class SettingsDebugDisplay : MonoBehaviour
{
	public PsyiaSettingsApplicator SettingsApplicator;

	public void Awake()
	{
		SettingsApplicator = GetComponent<PsyiaSettingsApplicator>();
	}
}
