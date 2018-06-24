﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTools;

[RequireComponent(typeof(ForceSource))]
public class DebugForce : MonoBehaviour {

	public float Range = 5f;
	public int MaxArrowCount = 500;
	public float ArrowLength = 0.05f;
	public bool UseVRDebug;

	public Transform VRDebugForceLocation;

	ForceSource MyForceSource;

	void Start() {
		MyForceSource = GetComponent<ForceSource>();
	}

	void DrawArrow(Vector3 Start, Vector3 End, Color Color, float ArrowHeadLength = 0.4f, float Radius = 0.001f)
	{

		Vector3 Offset = End - Start;
		Vector3 Direction = Vector3.Normalize(Offset);
 
		Vector3 Right = Vector3.Normalize(Quaternion.LookRotation(Direction) * Quaternion.Euler(0,180+45,0) * new Vector3(0,0,1));
		Vector3 Left = Vector3.Normalize(Quaternion.LookRotation(Direction) * Quaternion.Euler(0,180-45,0) * new Vector3(0,0,1));
		Vector3 Up = Vector3.Normalize(Quaternion.LookRotation(Direction) * Quaternion.Euler(180+45, 0, 0) * new Vector3(0,0,1));
		Vector3 Down = Vector3.Normalize(Quaternion.LookRotation(Direction) * Quaternion.Euler(180-45, 0, 0) * new Vector3(0,0,1));

		if(UseVRDebug) {
			VRDebug.DrawLine(Start, End, Color, false, 0.001f);
			VRDebug.DrawLine(End, End + Right * Mathf.Min(0.1f, (ArrowHeadLength * Offset.magnitude)), Color, false, 0.001f);
			VRDebug.DrawLine(End, End + Left * Mathf.Min(0.1f, (ArrowHeadLength * Offset.magnitude)), Color, false, 0.001f);
			VRDebug.DrawLine(End, End + Up * Mathf.Min(0.1f, (ArrowHeadLength * Offset.magnitude)), Color, false, 0.001f);
			VRDebug.DrawLine(End, End + Down * Mathf.Min(0.1f, (ArrowHeadLength * Offset.magnitude)), Color, false, 0.001f);
		} else {
			Debug.DrawLine(Start, End, Color);
			Debug.DrawLine(End, End + Right * Mathf.Min(0.1f, (ArrowHeadLength * Offset.magnitude)), Color);
			Debug.DrawLine(End, End + Left * Mathf.Min(0.1f, (ArrowHeadLength * Offset.magnitude)), Color);
			Debug.DrawLine(End, End + Up * Mathf.Min(0.1f, (ArrowHeadLength * Offset.magnitude)), Color);
			Debug.DrawLine(End, End + Down * Mathf.Min(0.1f, (ArrowHeadLength * Offset.magnitude)), Color);
		}
	}
	void Update () {
		if(MyForceSource.StrengthModifier == 0 || MyForceSource.Force.Strength == 0) return;

		if(UseVRDebug) {
			Vector3 Pos = VRDebugForceLocation.position;
			Vector3 Force = ForcesMirror.GetForceAtPoint(MyForceSource.Force.GetForceData(transform.position, transform.eulerAngles, MyForceSource.StrengthModifier), Pos);
			DrawArrow(Pos, Pos + Force * ArrowLength, Color.white);
		} else {
			int ArrowLengthCount = Mathf.RoundToInt(Mathf.Pow((float)MaxArrowCount, 1f/3f));
			for(int x = 0; x < ArrowLengthCount; x++) {
				float XPos = Mathf.Lerp(-Range/2f, Range/2f, Mathf.InverseLerp(0, ArrowLengthCount - 1, x));
				for(int y = 0; y < ArrowLengthCount; y++) {
					float YPos = Mathf.Lerp(-Range/2f, Range/2f, Mathf.InverseLerp(0, ArrowLengthCount - 1, y));
					for(int z = 0; z < ArrowLengthCount; z++) {
						float ZPos = Mathf.Lerp(-Range/2f, Range/2f, Mathf.InverseLerp(0, ArrowLengthCount - 1, z));
						Vector3 Pos = transform.position + new Vector3(XPos, YPos, ZPos);
						Vector3 Force = ForcesMirror.GetForceAtPoint(MyForceSource.Force.GetForceData(transform.position, transform.eulerAngles, MyForceSource.StrengthModifier), Pos);

						DrawArrow(Pos, Pos + Force * ArrowLength, Color.white);
					}
				}
			}
		}
	}
}